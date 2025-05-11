using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeesController(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Получить данные всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<EmployeeShortResponse>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeesModelList = employees.Select(x =>
                new EmployeeShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                }).ToList();

            return employeesModelList;
        }

        /// <summary>
        /// Получить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = employee.Roles.Select(x => new RoleItemResponse()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return employeeModel;
        }


        /// <summary>
        /// Удаление сотрудника по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteEmployeeByIdAsync(Guid id)
        {
            await _employeeRepository.DeleteByIdAsync(id);
            return Ok();
        }


        [HttpPut]
        public async Task<ActionResult> UpdateEmployeeAsync(EmployeeEditModel model)
        {

            //TODO вынести в AutoMap
            Employee employee = new Employee()
            {
                Id = model.Id,
                Email = model.Email,
                FirstName= model.FirstName,
                LastName = model.LastName,
                Roles = model.Roles.Select(q => new Role() { Id = q.Id, Name = q.Name }).ToList(),
                AppliedPromocodesCount = model.AppliedPromocodesCount
            };

            await _employeeRepository.UpdateAsync(employee);

            return Ok();

        }

        [HttpPost]
        public async Task<ActionResult> EmployeeCreateAsync(EmployeeCreateModel model)
        {

            Employee employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Email = model.Email,
                FirstName= model.FirstName,
                LastName = model.LastName,
                Roles = model.Roles.Select(q => new Role() { Id = q.Id, Name = q.Name }).ToList()
            };

            await _employeeRepository.AddAsync(employee);

            return Ok();

        }
    }
}
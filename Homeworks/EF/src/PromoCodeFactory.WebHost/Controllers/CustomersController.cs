using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers;

/// <summary>
/// Клиенты
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class CustomersController
    : ControllerBase
{
    private readonly IRepository<Customer> customerRepository;
    private readonly IRepository<Preference> preferenceRepository;
    private readonly IRepository<PromoCode> promocodesRepository;
    public CustomersController(IRepository<Customer> customerRepository, IRepository<Preference> preferenceRepository, IRepository<PromoCode> promocodesRepository)
    {
        this.customerRepository = customerRepository;
        this.preferenceRepository = preferenceRepository;
        this.promocodesRepository = promocodesRepository;
    }



    [HttpGet]
    public async Task<ActionResult<List<CustomerShortResponse>>> GetCustomersAsync()
    {
        //TODO: Добавить получение списка клиентов
        var customers = await customerRepository.GetAllAsync();

        var customerModelList = customers.Select(c =>
            new CustomerShortResponse()
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email
            }).ToList();

        return customerModelList;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
    {
        //TODO: Добавить получение клиента вместе с выданными ему промомкодами
        var customer = await customerRepository.GetByIdAsync(id);
        if (customer == null) return NotFound();
        var promoCodes = customer.PromoCodes;



        var customerModel = new CustomerResponse()
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PromoCodes = promoCodes != null ? customer.PromoCodes.Select(p =>
                new PromoCodeShortResponse
                {
                    Id = p.Id,
                    Code = p.Code,
                    ServiceInfo = p.ServiceInfo,
                    BeginDate = p.BeginDate.ToString("yyyy.MM.dd"),
                    EndDate = p.EndDate.ToString("yyyy.MM.dd"),
                    PartnerName = p.PartnerName
                }).ToList() : null
        };

        return Ok(customerModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
    {
        //TODO: Добавить создание нового клиента вместе с его предпочтениями
        var preferences = await preferenceRepository.GetRangeByIdsAsync(request.PreferenceIds);

        var customer = new Customer()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
        };
        customer.Preferences = preferences.Select(p => new CustomerPreference()
        {
            Customer = customer,
            Preference = p
        }).ToList();

        await customerRepository.AddAsync(customer);

        return Ok();
    }

    [HttpPut("{id}")]
    public Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
    {
        //TODO: Обновить данные клиента вместе с его предпочтениями
        throw new NotImplementedException();
    }

    [HttpDelete]
    public Task<IActionResult> DeleteCustomer(Guid id)
    {
        //TODO: Удаление клиента вместе с выданными ему промокодами
        throw new NotImplementedException();
    }
}

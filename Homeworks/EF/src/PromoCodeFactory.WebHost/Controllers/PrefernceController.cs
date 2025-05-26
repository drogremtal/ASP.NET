using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PromoCodeFactory.WebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrefernceController : ControllerBase
    {

        private readonly IRepository<Preference> _preferenceRepository;


        // GET: api/<PrefernceController>
        [HttpGet]
        public async Task<IEnumerable<PrefernceResponse>> Get()
        {
            var _preference = await _preferenceRepository.GetAllAsync();


            var response = _preference.Select(x => new PrefernceResponse
            {
                Id = x.Id,
                Name = x.Name,

            }).ToList();

            return response;
        }

        // GET api/<PrefernceController>/5
        [HttpGet("{id}")]
        public async Task<PrefernceResponse> Get(Guid id)
        {
            var _preference = await _preferenceRepository.GetByIdAsync(id);

            var response = new PrefernceResponse
            {
                Id = id,
                Name = _preference.Name,
            };

            return response;
        }

    }
}

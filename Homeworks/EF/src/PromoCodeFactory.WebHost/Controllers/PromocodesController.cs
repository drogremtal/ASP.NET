using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;

namespace PromoCodeFactory.WebHost.Controllers;

/// <summary>
/// Промокоды
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class PromocodesController
    : ControllerBase
{

    private readonly IRepository<PromoCode> promocodesRepository;
    private readonly IRepository<Preference> preferenceRepository;
    private readonly IRepository<Customer> customerRepository;
    public PromocodesController(IRepository<PromoCode> promocodesRepository, IRepository<Preference> preferenceRepository, IRepository<Customer> customerRepository)
    {
        this.promocodesRepository = promocodesRepository;
        this.preferenceRepository = preferenceRepository;
        this.customerRepository = customerRepository;
    }

    /// <summary>
    /// Получить все промокоды
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
    {
        var promoCodes =  await promocodesRepository.GetAllAsync();
        var promoCodesShort = promoCodes.Select(q =>
        new PromoCodeShortResponse(){
            
            Id = q.Id,
            Code = q.Code,
            BeginDate = q.BeginDate.ToShortDateString(),
            EndDate = q.EndDate.ToShortDateString(),
            PartnerName = q.PartnerName,
            ServiceInfo = q.ServiceInfo

        }).ToList();

        return promoCodesShort;

    }

    /// <summary>
    /// Создать промокод и выдать его клиентам с указанным предпочтением
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
    {
        var preference = await preferenceRepository.FirstOrDefaultAsync(p => p.Name == request.Preference);

        if (preference == null)
        {
            return BadRequest();
        }

        var customer =  await 
            customerRepository.FirstOrDefaultAsync(q => q.Preferences.Any(cp => cp.PreferenceId == preference.Id));

        if (customer is null)
        {
            return NotFound("");
        }

        PromoCode promoCode = new()
        {
            Preference = preference,
            BeginDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(14),
            Code = request.PromoCode,
            PartnerName = request.PartnerName,
            Customer = customer,
            ServiceInfo = request.ServiceInfo
        };

        await promocodesRepository.AddAsync(promoCode);
        
        return Ok();

    }
}

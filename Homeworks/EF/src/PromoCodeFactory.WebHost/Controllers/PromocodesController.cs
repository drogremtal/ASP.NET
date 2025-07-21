using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
    public Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
    {
        //TODO: Получить все промокоды 
        throw new NotImplementedException();
    }

    /// <summary>
    /// Создать промокод и выдать его клиентам с указанным предпочтением
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
    {
        //TODO: Создать промокод и выдать его клиентам с указанным предпочтением
        throw new NotImplementedException();
    }
}

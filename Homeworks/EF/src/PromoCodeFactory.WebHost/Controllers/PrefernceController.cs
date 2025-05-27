using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Предоставляет методы для работы с предпочтениями (Preferences).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PreferenceController : ControllerBase
    {
        private readonly IRepository<Preference> _preferenceRepository;

        public PreferenceController(IRepository<Preference> preferenceRepository)
        {
            _preferenceRepository = preferenceRepository;
        }

        /// <summary>
        /// Получает список всех предпочтений.
        /// </summary>
        /// <returns>Список предпочтений.</returns>
        /// <response code="200">Возвращает список предпочтений.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PreferenceResponse>), 200)]
        public async Task<IEnumerable<PreferenceResponse>> GetAsync()
        {
            var preferences = await _preferenceRepository.GetAllAsync();

            var response = preferences.Select(preference => new PreferenceResponse
            {
                Id = preference.Id,
                Name = preference.Name,
                CustomersLink = preference.CustomersLink
            });

            return response;
        }

        /// <summary>
        /// Получает предпочтение по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор предпочтения.</param>
        /// <returns>Предпочтение с указанным идентификатором.</returns>
        /// <response code="200">Если предпочтение найдено.</response>
        /// <response code="404">Если предпочтение не найдено.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PreferenceResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PreferenceResponse>> GetByIdAsync(Guid id)
        {
            var preference = await _preferenceRepository.GetByIdAsync(id);

            if (preference == null)
                return NotFound();

            var response = new PreferenceResponse
            {
                Id = preference.Id,
                Name = preference.Name,
                CustomersLink = preference.CustomersLink
            };

            return Ok(response);
        }
    }
}
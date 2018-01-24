﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCore.Business.Models;
using NetCore.Business.Services;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace NetCore.Api.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    public class CurrencyController : Controller
    {
        private readonly CurrencyService _service;

        public CurrencyController(CurrencyService service)
        {
            _service = service;
        }

        [HttpGet, Route("")]
        [ProducesResponseType(typeof(List<Currency>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet, Route("settings")]
        [ProducesResponseType(typeof(List<Currency>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllWithSettings()
        {
            return Ok(await _service.GetAllWithSettingsAsync());
        }

        //[AuthorizeDirector]
        [HttpPut, Route("")]
        [ProducesResponseType(typeof(List<Currency>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateAll([FromBody] IEnumerable<Currency> exchangeRates)
        {
            return Ok(await _service.UpdateAllAsync(exchangeRates));
        }
    }
}
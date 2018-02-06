using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCore.Api.Config;
using NetCore.Business.Models;
using NetCore.Business.Services;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace NetCore.Api.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    public class CurrencyController : EntityController<Currency, Currency>
    {
        private readonly CurrencyService _service;

        public CurrencyController(CurrencyService service)
            : base(service)
        {
            _service = service;
        }

        [HttpGet, Route("")]
        [ProducesResponseType(typeof(List<Currency>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [AuthorizeAdmin]
        [HttpPut, Route("")]
        [ProducesResponseType(typeof(List<Currency>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateAll([FromBody] IEnumerable<Currency> exchangeRates)
        {
            return Ok(await _service.UpdateAllAsync(exchangeRates));
        }

        [AuthorizeAdmin]
        [HttpPost, Route("")]
        [ProducesResponseType(typeof(Currency), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody]Currency model)
        {
            return await CreateAsync(model);
        }
    }
}
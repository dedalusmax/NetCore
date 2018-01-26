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
    //[Authorize]
    [Route("api/v1/[controller]")]
    public class CountryController : EntityController<Country, CountryBase>
    {
        private readonly CountryService _service;

        public CountryController(CountryService service)
            : base(service)
        {
            _service = service;
        }

        [HttpGet, Route("")]
        [ProducesResponseType(typeof(List<Country>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            return await GetAllAsync();
        }

        [HttpGet, Route("active")]
        [ProducesResponseType(typeof(List<Country>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetActive()
        {
            return Ok(await _service.GetAllActiveAsync());
        }

        [AuthorizeAdmin]
        [HttpGet, Route("{id:long}")]
        [ProducesResponseType(typeof(Country), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(long id)
        {
            return await GetAsync(id);
        }

       // [AuthorizeAdmin]
        [HttpPost, Route("")]
        [ProducesResponseType(typeof(Country), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody]CountryBase model)
        {
            return await CreateAsync(model);
        }

        [AuthorizeAdmin]
        [HttpPut, Route("{id:long}")]
        [ProducesResponseType(typeof(Country), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update(long id, [FromBody]CountryBase model)
        {
            return await UpdateAsync(id, model);
        }

        [AuthorizeAdmin]
        [HttpDelete, Route("{id:long}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(long id)
        {
            return await DeleteAsync(id);
        }

    }

}
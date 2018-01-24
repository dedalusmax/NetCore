using Microsoft.AspNetCore.Mvc;
using NetCore.Api.Config;
using NetCore.Business.Models;
using NetCore.Business.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCore.Api.Controllers
{
    //[AuthorizeManager]
    [Route("api/v1/[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpGet]
        [Produces(typeof(List<User>))]
        public async Task<IActionResult> Get()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [AuthorizeAdmin]
        [HttpGet("{id:long}")]
        [Produces(typeof(User))]
        public async Task<IActionResult> Get(long id)
        {
            var item = await _service.GetAsync(id);
            return Ok(item);
        }

        [AuthorizeAdmin]
        [HttpPost]
        [Produces(typeof(User))]
        public async Task<IActionResult> Post([FromBody]UserBase model)
        {
            var item = await _service.CreateAsync(model);
            return Ok(item);
        }

        [AuthorizeAdmin]
        [HttpPut("{id:long}")]
        [Produces(typeof(User))]
        public async Task<IActionResult> Put(long id, [FromBody]UserBase model)
        {
            var item = await _service.UpdateAsync(id, model);
            return Ok(item);
        }

        [AuthorizeAdmin]
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
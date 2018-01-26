using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCore.Business.Models;
using NetCore.Business.Services;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace NetCore.Api.Controllers
{
    //[Authorize]
    [Route("api/v1/[controller]")]
    public class RoleController : Controller
    {
        private readonly RoleService _service;

        public RoleController(RoleService service)
        {
            _service = service;
        }

        [HttpGet]
        [Produces(typeof(List<Role>))]
        public async Task<IActionResult> Get()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }
    }
}
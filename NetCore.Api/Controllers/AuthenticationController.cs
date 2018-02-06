using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCore.Api.Config;
using NetCore.Business.Authentication;

namespace NetCore.Api.Controllers
{
    [Route("api/v1/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly AuthenticationService _service;

        public AuthenticationController(AuthenticationService service)
        {
            _service = service;
        }

        [HttpPost, Route("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticationModel model)
        {
            var result = await _service.AuthenticateAsync(model);
            return Ok(result);
        }

        [AuthorizeAdmin]
        [HttpPost, Route("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordModel model)
        {
            await _service.RequestPasswordResetAsync(model);
            return Ok();
        }

        [AuthorizeAdmin]
        [HttpPost, Route("setPassword")]
        public async Task<IActionResult> SetPassword([FromBody]SetPasswordModel model)
        {
            await _service.SetPasswordAsync(model);
            return Ok();
        }

        [HttpPost, Route("refresh")]
        public async Task<IActionResult> Refresh([FromBody]AuthenticationRefreshModel model)
        {
            var result = await _service.RefreshAuthenticationAsync(model);
            return Ok(result);
        }
    }

}
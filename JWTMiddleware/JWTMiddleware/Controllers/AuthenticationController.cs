using JWTMiddleware.Model;
using JWTMiddleware.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWTMiddleware.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(AuthenticateRequest model)
        {
            var response = this.authenticationService.authenticate(model);
            if (response == null)
            {
                return new JsonResult(new { message = "Unauthorized", StatusCode = StatusCodes.Status401Unauthorized });
            }
            else { 
                return Ok(response);
            }

        }
    }
}

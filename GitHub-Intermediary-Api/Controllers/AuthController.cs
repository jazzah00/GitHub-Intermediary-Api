using GitHub_Intermediary_Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GitHub_Intermediary_Api.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase {
        private readonly IAuthService _AuthService;

        public AuthController(IAuthService authService) {
            _AuthService = authService;
        }

        [HttpGet("GenerateToken")]
        public IActionResult GenerateToken() {
            string token = _AuthService.GenerateToken();
            return Ok(new { Token = token });
        }
    }
}

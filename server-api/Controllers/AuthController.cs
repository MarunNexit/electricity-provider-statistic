using electricity_provider_server_api.DTOs.Auth;
using electricity_provider_server_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace electricity_provider_server_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _authService.Register(registerDto);

            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            AppendAuthCookies(result.Token!, result.RefreshToken!);

            if (!result.Success) return BadRequest(result.ErrorMessage);
            return Ok(result.Result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authService.Login(loginDto);
            if (!result.Success) return Unauthorized(result.ErrorMessage);

            AppendAuthCookies(result.Token!, result.RefreshToken!);

            return Ok(result.Result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var oldRefreshToken = Request.Cookies["refreshToken"];
            var result = await _authService.Refresh(oldRefreshToken);
            if (!result.Success) return Unauthorized(result.ErrorMessage);

            AppendAuthCookies(result.NewAccessToken!, result.NewRefreshToken!);

            return Ok(new { message = result.ErrorMessage });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            Response.Cookies.Delete("refreshToken");

            return Ok(new { message = "Logged out successfully" });
        }

        [HttpGet("check")]
        public IActionResult CheckAuth()
        {
            var token = Request.Cookies["jwt"];
            var isAuthenticated = _authService.CheckAuth(token);
            return Ok(new { isAuthenticated });
        }

        private void AppendAuthCookies(string token, string refreshToken)
        {
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            });

            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });
        }
    }
}

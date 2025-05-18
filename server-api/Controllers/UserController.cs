using electricity_provider_server_api.Data;
using electricity_provider_server_api.Models;
using electricity_provider_server_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace electricity_provider_server_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenService _jwtTokenService;

        public UserController(AppDbContext context, JwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyDetails()
        {
            var userId = HttpContext.Items["User"]?.ToString();

            if (userId == null)
            {
                return Unauthorized("No user information found");
            }

            var user = await _context.Users
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id.ToString() == userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(new
            {
                user.Id,
                user.Email,
                user.Name,
                user.Icon,
                Addresses = user.Addresses.Select(address => new
                {
                    address.Street,
                    address.ApartmentNumber,
                    address.City,
                    address.Country
                }).ToList()
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserDetails(int id)
        {
            var user = await _context.Users
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(new
            {
                user.Id,
                user.Email,
                user.Name,
                user.Icon,
                Addresses = user.Addresses.Select(address => new
                {
                    address.Street,
                    address.ApartmentNumber,
                    address.City,
                    address.Country
                }).ToList()
            });
        }
    }
}

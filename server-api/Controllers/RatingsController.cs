using AutoMapper;
using electricity_provider_server_api.Data;
using electricity_provider_server_api.DTOs;
using electricity_provider_server_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace electricity_provider_server_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public RatingsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // 4. Create new rating for existing provider
        [HttpPost]
        public async Task<ActionResult> AddRating([FromBody] ProviderRatingDto ratingDto)
        {
            var rating = _mapper.Map<ProviderRating>(ratingDto);
            _context.ProviderRatings.Add(rating);
            await _context.SaveChangesAsync();
            return Ok();
        }


        // 5. Update a rating
        [HttpPut("{userId:int}/{providerId:int}")]
        public async Task<IActionResult> UpdateRating(int userId, int providerId, [FromBody] double newRating)
        {
            var rating = await _context.ProviderRatings.FirstOrDefaultAsync(r => r.UserId == userId && r.ProviderId == providerId);
            if (rating == null) return NotFound();

            rating.Rating = newRating;
            rating.Date = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // 6. Delete a rating
        [HttpDelete("{userId:int}/{providerId:int}")]
        public async Task<IActionResult> DeleteRating(int userId, int providerId)
        {
            var rating = await _context.ProviderRatings.FirstOrDefaultAsync(r => r.UserId == userId && r.ProviderId == providerId);
            if (rating == null) return NotFound();

            _context.ProviderRatings.Remove(rating);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // 3. Get one provider by specific rating
        [HttpGet("by-rating/{rating:double}")]
        public async Task<ActionResult<IEnumerable<ProviderWithRatingDto>>> GetProvidersByRating(double rating)
        {
            var providers = await _context.Providers
                .Include(p => p.ProviderRatings)
                .Include(p => p.Addresses)
                .Where(p => Math.Round(
                    p.ProviderRatings
                        .Where(r => r.IsActive)
                        .Average(r => (double?)r.Rating) ?? 0, 1) == Math.Round(rating, 1))
                .ToListAsync();

            var providerDtos = _mapper.Map<IEnumerable<ProviderWithRatingDto>>(providers);

            return Ok(providerDtos);
        }

    }
}

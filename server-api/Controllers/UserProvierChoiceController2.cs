using electricity_provider_server_api.DTOs;
using electricity_provider_server_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace electricity_provider_server_api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserProviderChoiceController2 : ControllerBase
    {
        private readonly IUserProviderChoiceService _service;

        public UserProviderChoiceController2(IUserProviderChoiceService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var choices = await _service.GetAllAsync();
            return Ok(choices);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var choice = await _service.GetByIdAsync(id);
            if (choice == null) return NotFound();
            return Ok(choice);
        }

        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var choices = await _service.GetByUserIdAsync(userId);
            return Ok(choices);
        }

        [HttpGet("by-provider/{providerId}")]
        public async Task<IActionResult> GetByProviderId(int providerId)
        {
            var choices = await _service.GetByProviderIdAsync(providerId);
            return Ok(choices);
        }

        [HttpGet("provider/{providerId}/user-count")]
        public async Task<IActionResult> GetUserCountByProvider(int providerId)
        {
            var count = await _service.GetUserCountByProviderAsync(providerId);
            return Ok(new { ProviderId = providerId, UserCount = count });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserProviderChoiceDto dto)
        {
            try
            {
                var newChoice = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = newChoice.Id }, newChoice);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserProviderChoiceDto dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }


    }
}

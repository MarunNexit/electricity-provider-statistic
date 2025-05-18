using AutoMapper;
using AutoMapper.QueryableExtensions;
using electricity_provider_server_api.Data;
using electricity_provider_server_api.DTOs;
using electricity_provider_server_api.Models;
using electricity_provider_server_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace electricity_provider_server_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProvidersController : ControllerBase
    {
        private readonly ProvidersService _providersService;

        public ProvidersController(ProvidersService providersService)
        {
            _providersService = providersService;
        }

        // 1. Get list of all providers (without rating)
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<ProviderWithIdDto>>> GetAllProviders()
        {
            var providers = await _providersService.GetAllProvidersAsync();
            return Ok(providers);
        }

        // 1. Get all providers with average rating
        [HttpGet("with-ratings")]
        public async Task<ActionResult<IEnumerable<ProviderWithRatingAndIdDto>>> GetProvidersWithRatings()
        {
            var providers = await _providersService.GetProvidersWithRatingsAsync();
            return Ok(providers);
        }

        // 2. Get one provider by ID
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProviderWithIdDto>> GetProviderById(int id)
        {
            var provider = await _providersService.GetProviderByIdAsync(id);
            if (provider == null)
                return NotFound();
            return Ok(provider);
        }

        // 2. Get one provider by ID with average rating
        [HttpGet("with-ratings/{id:int}")]
        public async Task<ActionResult<object>> GetProviderByIdWithRatings(int id)
        {
            var provider = await _providersService.GetProviderByIdWithRatingsAsync(id);
            if (provider == null)
                return NotFound();
            return Ok(provider);
        }

        // 3. Get one provider by specific name (without rating)
        [HttpGet("by-name/{name}")]
        public async Task<ActionResult<Provider>> GetProviderByName(string name)
        {
            var provider = await _providersService.GetProviderByNameAsync(name);
            if (provider == null)
                return NotFound();
            return Ok(provider);
        }

        // 4. Create new provider
        [HttpPost("add-provider")]
        public async Task<ActionResult<Provider>> AddProvider([FromBody] ProviderDto providerDto)
        {
            var createdProvider = await _providersService.AddProviderAsync(providerDto);
            return CreatedAtAction(nameof(GetProviderById), new { id = createdProvider.Id }, createdProvider);
        }

        // 5. Update provider (name, website, logo)
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProvider(int id, [FromBody] ProviderDto updatedProvider)
        {
            await _providersService.UpdateProviderAsync(id, updatedProvider);
            return NoContent();
        }

        // 6. Delete a provider
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProvider(int id)
        {
            await _providersService.DeleteProviderAsync(id);
            return NoContent();
        }

    }
}
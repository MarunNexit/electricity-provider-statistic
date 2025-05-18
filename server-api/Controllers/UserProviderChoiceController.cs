using electricity_provider_server_api.DTOs;
using electricity_provider_server_api.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static electricity_provider_server_api.MediatrCQRS.QueriesAndCommands.ChoicesQueriesAndCommands;

namespace electricity_provider_server_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProviderChoiceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserProviderChoiceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _mediator.Send(new GetAllUserProviderChoicesQuery()));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetUserProviderChoiceByIdQuery(id));
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId) => Ok(await _mediator.Send(new GetUserProviderChoicesByUserIdQuery(userId)));

        [HttpGet("by-provider/{providerId}")]
        public async Task<IActionResult> GetByProviderId(int providerId) => Ok(await _mediator.Send(new GetUserProviderChoicesByProviderIdQuery(providerId)));

        [HttpGet("provider/{providerId}/user-count")]
        public async Task<IActionResult> GetUserCountByProvider(int providerId)
            => Ok(new { ProviderId = providerId, UserCount = await _mediator.Send(new GetUserCountByProviderQuery(providerId)) });

        [HttpGet("providers/user-counts")]
        public async Task<IActionResult> GetUserCountsForAllProviders()
        {
            var result = await _mediator.Send(new GetUserCountsForAllProvidersQuery());
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserProviderChoiceDto dto)
        {
            try
            {
                var created = await _mediator.Send(new CreateUserProviderChoiceCommand(dto));
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserProviderChoiceDto dto)
        {
            var success = await _mediator.Send(new UpdateUserProviderChoiceCommand(id, dto));
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _mediator.Send(new DeleteUserProviderChoiceCommand(id));
            return success ? NoContent() : NotFound();
        }
    }

}

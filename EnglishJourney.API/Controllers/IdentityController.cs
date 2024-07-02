using EnglishJourney.Application.Users.Commands.AddUserRole;
using EnglishJourney.Application.Users.Commands.DeleteUserRole;
using EnglishJourney.Application.Users.Commands.UpdateUserDetails;
using EnglishJourney.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishJourney.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController(IMediator mediator) : ControllerBase
    {
        [HttpPatch("user")]
        [Authorize]
        public async Task<IActionResult> UpdateUserDetails(UpdateUserDetailsCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }

        [HttpPost("userRole")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AddUserRole(AddUserRoleCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("userRole")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> RemoveUserRole(DeleteUserRoleCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }
    }
}
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserManager.Api.Extensions;
using UserManager.Application.Features.Users.CreateUser;
using UserManager.Domain.ViewModels;

namespace UserManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<CreateUserResponseViewModel>> CreateUser(
            [FromBody] CreateUserRequest request)
        {
            var result = await _mediator.Send(request);

            return result.ToActionResult();
        }
    }
}

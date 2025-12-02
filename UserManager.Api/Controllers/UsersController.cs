using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserManager.Api.Extensions;
using UserManager.Api.Extensions.User;
using UserManager.Api.Payloads.Users;
using UserManager.Application.Features.Users.CreateUser;
using UserManager.Application.Features.Users.GetUser;
using UserManager.Application.Features.Users.UpdateUser;
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
            [FromBody] CreateUserPayload payload)
        {
            var createUserRequest = payload.ToRequestModel();
            var result = await _mediator.Send(createUserRequest);

            return result.ToActionResult();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetUserResponseViewModel>> GetUser([FromRoute] int id)
        {
            var request = new GetUserRequest { Id = id };
            var result = await _mediator.Send(request);

            return result.ToActionResult();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<GetUserResponseViewModel>> UpdateUser(
            [FromRoute] int id,
            [FromBody] UpdateUserPayload payload)
        {
            var updateUserRequest = payload.ToRequestModel(id);
            var result = await _mediator.Send(updateUserRequest);

            return result.ToActionResult();
        }
    }
}

using FluentResults;
using MediatR;
using UserManager.Domain.ViewModels;

namespace UserManager.Application.Features.Users.GetUser;
public class GetUserRequest : IRequest<Result<GetUserResponseViewModel>>
{
    public int Id { get; set; }
}

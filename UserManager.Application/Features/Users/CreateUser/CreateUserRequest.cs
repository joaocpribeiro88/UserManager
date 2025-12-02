using FluentResults;
using MediatR;
using UserManager.Domain.ViewModels;

namespace UserManager.Application.Features.Users.CreateUser;

public class CreateUserRequest : IRequest<Result<CreateUserResponseViewModel>>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public AddressDto? Address { get; set; }
    public List<EmploymentDto> Employments { get; set; } = [];
}

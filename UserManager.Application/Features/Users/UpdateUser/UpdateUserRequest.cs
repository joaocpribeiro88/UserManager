using FluentResults;
using MediatR;
using UserManager.Domain.ViewModels;

namespace UserManager.Application.Features.Users.UpdateUser;
public class UpdateUserRequest : IRequest<Result<GetUserResponseViewModel>>
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UpdateAddressDto? Address { get; set; }
    public List<UpdateEmploymentDto> Employments { get; set; } = [];
}

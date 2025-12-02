using UserManager.Api.Payloads.Users;
using UserManager.Application.Features.Users.CreateUser;

namespace UserManager.Api.Extensions.User;

public static class CreateUserPayloadExtensions
{
    public static CreateUserRequest ToRequestModel(this CreateUserPayload payload)
        => new()
        {
            FirstName = payload.FirstName,
            LastName = payload.LastName,
            Email = payload.Email,
            Address = payload.Address != null
                ? new AddressDto
                {
                    Street = payload.Address.Street,
                    City = payload.Address.City,
                    PostCode = payload.Address.PostCode
                }
                : null,
            Employments = payload.Employments.ConvertAll(e => new EmploymentDto
            {
                Company = e.Company,
                MonthsOfExperience = e.MonthsOfExperience,
                Salary = e.Salary,
                StartDate = e.StartDate,
                EndDate = e.EndDate
            })
        };
}

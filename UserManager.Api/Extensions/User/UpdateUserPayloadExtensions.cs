using UserManager.Api.Payloads.Users;
using UserManager.Application.Features.Users.UpdateUser;

namespace UserManager.Api.Extensions.User;

public static class UpdateUserPayloadExtensions
{
    public static UpdateUserRequest ToRequestModel(this UpdateUserPayload payload, int id)
        => new()
        {
            Id = id,
            FirstName = payload.FirstName,
            LastName = payload.LastName,
            Email = payload.Email,
            Address = payload.Address == null
                ? null
                : new UpdateAddressDto
                {
                    Street = payload.Address.Street,
                    City = payload.Address.City,
                    PostCode = payload.Address.PostCode
                },
            Employments = payload.Employments.Select(e => new UpdateEmploymentDto
            {
                Id = e.Id,
                Company = e.Company,
                MonthsOfExperience = e.MonthsOfExperience,
                Salary = e.Salary,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                IsDeleted = e.IsDeleted
            }).ToList()
        };
}

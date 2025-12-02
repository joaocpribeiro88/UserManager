using UserManager.Domain.Entities;
using UserManager.Domain.ViewModels;

namespace UserManager.Application.Extensions;
public static class UserExtensions
{
    public static GetUserResponseViewModel ToViewModel(this User user)
    {
        return new GetUserResponseViewModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Address = user.Address != null ? new AddressViewModel
            {
                Street = user.Address.Street,
                City = user.Address.City,
                PostCode = user.Address.PostCode
            } : null,
            Employments = user.Employments.Select(e => new EmploymentViewModel
            {
                Id = e.Id,
                Company = e.Company,
                MonthsOfExperience = e.MonthsOfExperience,
                Salary = e.Salary,
                StartDate = DateOnly.FromDateTime(e.StartDate),
                EndDate = e.EndDate.HasValue
                    ? DateOnly.FromDateTime(e.EndDate.Value)
                    : null
            }).ToList()
        };
    }
}

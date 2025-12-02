using FluentValidation;

namespace UserManager.Application.Features.Users.UpdateUser;
public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("User ID must be greater than 0");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(255).WithMessage("Email cannot exceed 255 characters");

        When(x => x.Address != null, () =>
        {
            RuleFor(x => x.Address!.Street)
                .NotEmpty().WithMessage("Street is required when address is provided")
                .MaximumLength(200).WithMessage("Street cannot exceed 200 characters");

            RuleFor(x => x.Address!.City)
                .NotEmpty().WithMessage("City is required when address is provided")
                .MaximumLength(100).WithMessage("City cannot exceed 100 characters");

            RuleFor(x => x.Address!.PostCode)
                .GreaterThan(0).WithMessage("Post code must be greater than 0");
        });

        RuleForEach(x => x.Employments)
            .Where(dto => !dto.IsDeleted)
            .ChildRules(employment =>
            {
                employment.RuleFor(e => e.Company)
                    .NotEmpty().WithMessage("Company name is required")
                    .MaximumLength(150).WithMessage("Company name cannot exceed 150 characters");

                employment.RuleFor(e => e.MonthsOfExperience)
                    .GreaterThan((uint)0).WithMessage("Months of experience must be greater than 0");

                employment.RuleFor(e => e.Salary)
                    .GreaterThan((uint)0).WithMessage("Salary must be greater than 0");

                employment.RuleFor(e => e.StartDate)
                    .NotEmpty().WithMessage("Start date is required");

                employment.RuleFor(e => e.EndDate)
                    .GreaterThan(e => e.StartDate)
                    .When(e => e.EndDate != null)
                    .WithMessage("End date must be greater than start date.");
            });

        RuleForEach(x => x.Employments)
            .Where(dto => dto.IsDeleted)
            .ChildRules(employment =>
            {
                employment.RuleFor(e => e.Id)
                    .GreaterThan(0).WithMessage("Employment ID must be greater than 0 when IsDeleted is true.");
            });
    }
}

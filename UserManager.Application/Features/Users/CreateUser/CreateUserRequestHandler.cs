using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UserManager.Application.Models;
using UserManager.Domain.Entities;
using UserManager.Domain.ViewModels;
using UserManager.Infrastructure.Data;

namespace UserManager.Application.Features.Users.CreateUser;

public class CreateUserRequestHandler : IRequestHandler<CreateUserRequest, Result<CreateUserResponseViewModel>>
{
    private readonly UserManagerDbContext _context;

    public CreateUserRequestHandler(UserManagerDbContext context)
    {
        _context = context;
    }

    public async Task<Result<CreateUserResponseViewModel>> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (existingUser != null)
            {
                return Result.Fail(new CustomErrorResultDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = $"A user with email '{request.Email}' already exists."
                });
            }

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email
            };

            if (request.Address != null)
            {
                user.Address = new Address
                {
                    Street = request.Address.Street,
                    City = request.Address.City,
                    PostCode = request.Address.PostCode
                };
            }

            if (request.Employments.Any())
            {
                user.Employments = request.Employments.Select(e => new Employment
                {
                    Company = e.Company,
                    MonthsOfExperience = e.MonthsOfExperience,
                    Salary = e.Salary,
                    StartDate = e.StartDate.ToDateTime(TimeOnly.MinValue),
                    EndDate = e.EndDate?.ToDateTime(TimeOnly.MinValue)
                }).ToList();
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Ok(new CreateUserResponseViewModel
            {
                Id = user.Id
            });
        }
        catch (DbUpdateException dbEx)
        {
            return Result.Fail(new CustomErrorResultDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Message = $"Database error occurred while creating user: {dbEx.InnerException?.Message ?? dbEx.Message}"
            });
        }
        catch (Exception ex)
        {
            return Result.Fail(new CustomErrorResultDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Message = $"An unexpected error occurred while creating user: {ex.Message}"
            });
        }
    }
}

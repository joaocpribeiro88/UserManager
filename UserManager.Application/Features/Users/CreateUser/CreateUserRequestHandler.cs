using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserManager.Application.Models;
using UserManager.Domain.Entities;
using UserManager.Domain.ViewModels;
using UserManager.Infrastructure.Data;

namespace UserManager.Application.Features.Users.CreateUser;

public class CreateUserRequestHandler : IRequestHandler<CreateUserRequest, Result<CreateUserResponseViewModel>>
{
    private readonly ILogger<CreateUserRequestHandler> _logger;
    private readonly UserManagerDbContext _context;

    public CreateUserRequestHandler(
        ILogger<CreateUserRequestHandler> logger,
        UserManagerDbContext context)
    {
        _logger = logger;
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

            _logger.LogInformation("User {userId} added.", user.Id);

            return Result.Ok(new CreateUserResponseViewModel
            {
                Id = user.Id
            });
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Database error creating user.");
            return Result.Fail(new CustomErrorResultDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Message = $"Database error occurred while creating user: {dbEx.InnerException?.Message ?? dbEx.Message}"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user.");
            return Result.Fail(new CustomErrorResultDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Message = $"An unexpected error occurred while creating user: {ex.Message}"
            });
        }
    }
}

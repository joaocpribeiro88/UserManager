using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserManager.Application.Extensions;
using UserManager.Application.Models;
using UserManager.Domain.Entities;
using UserManager.Domain.ViewModels;
using UserManager.Infrastructure.Data;

namespace UserManager.Application.Features.Users.UpdateUser;
public class UpdateUserRequestHandler : IRequestHandler<UpdateUserRequest, Result<GetUserResponseViewModel>>
{
    private readonly ILogger<UpdateUserRequestHandler> _logger;
    private readonly UserManagerDbContext _context;

    public UpdateUserRequestHandler(
        ILogger<UpdateUserRequestHandler> logger,
        UserManagerDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<GetUserResponseViewModel>> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.Users
                .Include(u => u.Address)
                .Include(u => u.Employments)
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (user == null)
            {
                return Result.Fail(new CustomErrorResultDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Message = $"User with ID {request.Id} not found."
                });
            }

            if (user.Email != request.Email)
            {
                var existingUserWithEmail = await _context.Users.FirstOrDefaultAsync(
                    u => u.Email == request.Email && u.Id != request.Id,
                    cancellationToken);

                if (existingUserWithEmail != null)
                {
                    return Result.Fail(new CustomErrorResultDetails
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = $"A different user with email '{request.Email}' already exists."
                    });
                }

                _logger.LogInformation("User {userId} email is changing from {oldEmail} to {newEmail}.", user.Id, user.Email, request.Email);
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;

            UpdateAddress(user, request.Address);
            var updateEmploymentsResult = UpdateEmployments(user, request.Employments);
            if (updateEmploymentsResult.IsFailed)
            {
                return updateEmploymentsResult;
            }

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User {userId} updated.", user.Id);

            return Result.Ok(user.ToViewModel());
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Database error updating user {userId}.", request.Id);
            return Result.Fail(new CustomErrorResultDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Message = $"Database error occurred while updating user: {dbEx.InnerException?.Message ?? dbEx.Message}"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user {userId}.", request.Id);
            return Result.Fail(new CustomErrorResultDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Message = $"An unexpected error occurred while updating user: {ex.Message}"
            });
        }
    }

    private void UpdateAddress(User user, UpdateAddressDto? addressDto)
    {
        if (addressDto == null)
        {
            if (user.Address != null)
            {
                _context.Addresses.Remove(user.Address);
                user.Address = null;
            }

            return;
        }

        if (user.Address == null)
        {
            user.Address = new Address
            {
                Street = addressDto.Street,
                City = addressDto.City,
                PostCode = addressDto.PostCode
            };
        }
        else
        {
            user.Address.Street = addressDto.Street;
            user.Address.City = addressDto.City;
            user.Address.PostCode = addressDto.PostCode;
        }
    }

    private Result UpdateEmployments(User user, List<UpdateEmploymentDto> employmentDtos)
    {
        var employmentIdsNotFound = user.Employments
            .Where(e => employmentDtos.All(dto => dto.Id != e.Id))
            .Select(e => e.Id)
            .ToList();
        if (employmentIdsNotFound.Any())
        {
            return Result.Fail(new CustomErrorResultDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Message = $"The employment(s) {string.Join(", ", employmentIdsNotFound)}, which exist for the given user, were not found. Please use IsDeleted=true if the goal is to delete them."
            });
        }

        var employmentIdsToRemove = employmentDtos.Where(dto => dto.IsDeleted)
            .Select(dto => dto.Id)
            .Distinct()
            .ToList();

        foreach (var employmentIdToRemove in employmentIdsToRemove)
        {
            var employment = user.Employments.SingleOrDefault(e => e.Id == employmentIdToRemove);
            if (employment == null)
            {
                continue;
            }

            _context.Employments.Remove(employment);
            user.Employments.Remove(employment);
        }

        foreach (var dto in employmentDtos.Where(dto => !dto.IsDeleted))
        {
            if (dto.Id.HasValue)
            {
                var existingEmployment = user.Employments.SingleOrDefault(e => e.Id == dto.Id.Value);
                if (existingEmployment != null)
                {
                    existingEmployment.Company = dto.Company;
                    existingEmployment.MonthsOfExperience = dto.MonthsOfExperience;
                    existingEmployment.Salary = dto.Salary;
                    existingEmployment.StartDate = dto.StartDate.ToDateTime(TimeOnly.MinValue);
                    existingEmployment.EndDate = dto.EndDate?.ToDateTime(TimeOnly.MinValue);
                }
            }
            else
            {
                var newEmployment = new Employment
                {
                    Company = dto.Company,
                    MonthsOfExperience = dto.MonthsOfExperience,
                    Salary = dto.Salary,
                    StartDate = dto.StartDate.ToDateTime(TimeOnly.MinValue),
                    EndDate = dto.EndDate?.ToDateTime(TimeOnly.MinValue)
                };
                user.Employments.Add(newEmployment);
            }
        }

        return Result.Ok();
    }
}

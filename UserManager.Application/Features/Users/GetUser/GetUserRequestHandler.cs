using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserManager.Application.Extensions;
using UserManager.Application.Models;
using UserManager.Domain.ViewModels;
using UserManager.Infrastructure.Data;

namespace UserManager.Application.Features.Users.GetUser;
public class GetUserRequestHandler : IRequestHandler<GetUserRequest, Result<GetUserResponseViewModel>>
{
    private readonly ILogger<GetUserRequestHandler> _logger;
    private readonly UserManagerDbContext _context;

    public GetUserRequestHandler(
        ILogger<GetUserRequestHandler> logger,
        UserManagerDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<GetUserResponseViewModel>> Handle(GetUserRequest request, CancellationToken cancellationToken)
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

            return Result.Ok(user.ToViewModel());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user {userId}.", request.Id);
            return Result.Fail(new CustomErrorResultDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Message = $"An error occurred while retrieving user: {ex.Message}"
            });
        }
    }
}

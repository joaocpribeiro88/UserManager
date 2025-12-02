using FluentResults;
using Microsoft.AspNetCore.Mvc;
using UserManager.Application.Models;

namespace UserManager.Api.Extensions;

public static class ResultExtensions
{
    public static ActionResult<T> ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(result.Value);
        }

        return ProcessError(result.Errors);
    }

    public static ActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return new OkResult();
        }

        return ProcessError(result.Errors);
    }

    private static ActionResult ProcessError(List<IError> errors)
    {
        var error = errors[0] as CustomErrorResultDetails ?? new CustomErrorResultDetails();

        if (error.Status is StatusCodes.Status400BadRequest)
        {
            return new BadRequestObjectResult(new { error });
        }

        return new StatusCodeResult(error.Status ?? StatusCodes.Status500InternalServerError);
    }
}

using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using UserManager.Application.Models;

namespace UserManager.Application.Behaviors;
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : ResultBase, new()
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        if (!failures.Any())
        {
            return await next();
        }

        var metaData = failures.GroupBy(
                x => x.PropertyName,
                x => x.ErrorMessage,
                (propertyName, errorMessages) => new
                {
                    Key = propertyName,
                    Values = (object)errorMessages.First()
                })
            .ToDictionary(x => x.Key, x => x.Values);

        var response = new TResponse();
        response.Reasons.Add(new CustomErrorResultDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Message = "Validation errors occurred.",
            Metadata = metaData
        });
        return response;

    }
}

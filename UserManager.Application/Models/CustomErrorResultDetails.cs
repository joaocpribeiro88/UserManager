using FluentResults;

namespace UserManager.Application.Models;
public class CustomErrorResultDetails : IError
{
    public int? Status { get; init; }
    public string Message { get; init; } = string.Empty;
    public Dictionary<string, object> Metadata { get; init; } = [];
    public List<IError> Reasons { get; init; } = [];
}

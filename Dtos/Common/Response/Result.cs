using System;

namespace CrochetWorldApi.Dtos.Common.Response;

public sealed record Result<T>(
    bool IsSuccess,
    T? Data,
    string? Error,
    ErrorType? ErrorType,
    SuccessType? SuccessType
)
{
    public static Result<T> Success(T data, SuccessType successType) =>
        new(true, data, null, null, successType);

    public static Result<T> Failure(
        string error,
        ErrorType errorType) =>
        new(false, default, error, errorType, null);
}

public enum ErrorType
{
    None,
    Validation,
    NotFound,
    Conflict,
    Unauthorized,
    Forbidden
}

public enum SuccessType
{
    Ok,
    Created,
    NoContent,
    Accepted
}

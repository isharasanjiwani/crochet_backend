using System;

namespace CrochetWorldApi.Dtos.Common.Response;

public sealed record Result<T>(
    bool IsSuccess,
    T? Data,
    string? Error,
    ErrorType ErrorType
)
{
    public static Result<T> Success(T data) =>
        new(true, data, null, ErrorType.None);

    public static Result<T> Failure(
        string error,
        ErrorType errorType) =>
        new(false, default, error, errorType);
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

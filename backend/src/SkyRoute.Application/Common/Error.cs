namespace SkyRoute.Application.Common;

public enum ErrorType
{
    Validation,
    NotFound,
    Conflict,
    ProviderFailure,
    Unexpected
}

public record Error(string Code, string Message, ErrorType Type)
{
    public static Error NotFound(string code, string message) =>
        new(code, message, ErrorType.NotFound);

    public static Error Validation(string code, string message) =>
        new(code, message, ErrorType.Validation);

    public static Error Conflict(string code, string message) =>
        new(code, message, ErrorType.Conflict);

    public static Error ProviderFailure(string code, string message) =>
        new(code, message, ErrorType.ProviderFailure);

    public static Error Unexpected(string code, string message) =>
        new(code, message, ErrorType.Unexpected);
}

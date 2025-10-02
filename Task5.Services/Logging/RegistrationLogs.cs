using Microsoft.Extensions.Logging;

namespace Task5.Services.Logging;

public partial class RegistrationLogs
{
    [LoggerMessage(EventId = 100, Level = LogLevel.Information, Message = "New user {email} registration process started..")]
    public static partial void UserRegistrationStart(ILogger logger, string email);

    [LoggerMessage(EventId = 200, Level = LogLevel.Information, Message = "User {email} successfuly registered.")]
    public static partial void UserRegistrationSuccess(ILogger logger, string email);

    [LoggerMessage(EventId = 300, Level = LogLevel.Error, Message = "User {email} registration failed.")]
    public static partial void UserRegistrationFailed(ILogger logger, string email);

    [LoggerMessage(EventId = 400, Level = LogLevel.Warning, Message = "{message}")]
    public static partial void LogWarningMessage(ILogger logger, string message);
}

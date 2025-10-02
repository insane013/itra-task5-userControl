using Microsoft.Extensions.Logging;

namespace Task5.Services.Logging;

public partial class VerificationLogs
{
    [LoggerMessage(EventId = 101, Level = LogLevel.Information, Message = "User verification process started..")]
    public static partial void UserVerificationStart(ILogger logger);

    [LoggerMessage(EventId = 201, Level = LogLevel.Information, Message = "Verification service requested..")]
    public static partial void UserVerificationEmail(ILogger logger);

    [LoggerMessage(EventId = 301, Level = LogLevel.Information, Message = "User verification process finished.")]
    public static partial void UserVerificationEnd(ILogger logger);
}

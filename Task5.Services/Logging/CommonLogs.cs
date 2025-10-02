using Microsoft.Extensions.Logging;

namespace Task5.Services.Logging;

public partial class CommonLogs
{
    [LoggerMessage(EventId = 0, Level = LogLevel.Warning, Message = "{message}")]
    public static partial void LogWarningMessage(ILogger logger, string message);
}

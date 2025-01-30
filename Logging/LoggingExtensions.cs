namespace slap.Logging;

public static class LoggingExtensions
{
    public static string ToUpperString(this LogLevel logLevel)
    {
        // the reason we're not using ToString directly is that
        // more logging types could be added soon with different names
        return logLevel switch
        {
            LogLevel.Info => "INFO",
            LogLevel.Warning => "WARNING",
            LogLevel.Error => "ERROR",
            LogLevel.Critical => "CRITICAL",
            _ => "UNKNOWN"
        };
    }
}

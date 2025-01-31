namespace slap.Logging;

public enum LogLevel
{
    Raw,
    Info,
    Success,
    Failure,
    Dialogue,
    Warning,
    Error,
    Critical,
    Blank
}

public static class LogLevelExtensions
{
    public static ConsoleColor GetColor(this LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Raw => ConsoleColor.White,
            LogLevel.Info => ConsoleColor.Cyan,
            LogLevel.Success => ConsoleColor.Green,
            LogLevel.Failure => ConsoleColor.Red,
            LogLevel.Dialogue => ConsoleColor.DarkCyan,
            LogLevel.Warning => ConsoleColor.Yellow,
            LogLevel.Error => ConsoleColor.DarkRed,
            LogLevel.Critical => ConsoleColor.Magenta,
            _ => ConsoleColor.White
        };
    }
    public static string ToUpperString(this LogLevel logLevel)
    {
        // the reason we're not using ToString directly is that
        // more logging types could be added soon with different names
        return logLevel switch
        {
            LogLevel.Raw => "RAW",
            LogLevel.Info => "INFO",
            LogLevel.Success => "SUCCESS",
            LogLevel.Failure => "FAILURE",
            LogLevel.Dialogue => "DIALOGUE",
            LogLevel.Warning => "WARNING",
            LogLevel.Error => "ERROR",
            LogLevel.Critical => "CRITICAL",
            _ => "UNKNOWN"
        };
    }
}

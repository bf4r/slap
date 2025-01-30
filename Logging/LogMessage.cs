namespace slap.Logging;

public class LogMessage
{
    public LogLevel LogLevel { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public LogMessage(LogLevel logLevel, string message)
    {
        LogLevel = logLevel;
        Message = message;
        CreatedAt = DateTime.Now;
    }
}

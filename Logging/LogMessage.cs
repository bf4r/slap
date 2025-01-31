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
        CreatedAt = Simulation.Now;
    }
    public override string ToString()
    {
        if (LogLevel == LogLevel.Blank) return "";
        return $"[{CreatedAt}] ({LogLevel.ToUpperString()}) {Message}";
    }
}

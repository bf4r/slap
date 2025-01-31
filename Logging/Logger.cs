namespace slap.Logging;
using System.Text;

public class Logger
{
    public List<LogMessage> Messages { get; set; }
    public DateTime CreatedAt { get; set; }
    public void Log(LogLevel logLevel, string message)
    {
        Log(new LogMessage(logLevel, message));
    }
    private void Log(LogMessage message)
    {
        Messages.Add(message);
    }
    public Logger()
    {
        Messages = new();
        CreatedAt = Simulation.Now;
    }
    public string GetLogs()
    {
        var sb = new StringBuilder();
        foreach (var message in Messages)
        {
            sb.AppendLine($"[{message.CreatedAt}] ({message.LogLevel.ToUpperString()}) {message.Message}");
        }
        return string.Join(Environment.NewLine, sb.ToString());
    }
    public void PrintLogs()
    {
        var logs = GetLogs();
        Console.WriteLine(logs.TrimEnd());
    }

    // helper methods
    public void Info(string message) { Log(new LogMessage(LogLevel.Info, message)); }
    public void Warning(string message) { Log(new LogMessage(LogLevel.Warning, message)); }
    public void Error(string message) { Log(new LogMessage(LogLevel.Error, message)); }
    public void Critical(string message) { Log(new LogMessage(LogLevel.Critical, message)); }
}

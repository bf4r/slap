namespace slap.Logging;
using System.Text;

public class Logger
{
    public List<LogMessage> Messages { get; set; }
    public DateTime CreatedAt { get; set; }
    public Action<LogMessage>? OnMessage { get; set; }
    public void Log(LogLevel logLevel, string message)
    {
        Log(new LogMessage(logLevel, message));
    }
    private void Log(LogMessage message)
    {
        Messages.Add(message);
        OnMessage?.Invoke(message);
    }
    public Logger()
    {
        Messages = new();
        CreatedAt = Sim.Now;
    }
    public string GetLogs()
    {
        var sb = new StringBuilder();
        foreach (var message in Messages)
        {
            sb.AppendLine(message.ToString());
        }
        return string.Join(Environment.NewLine, sb.ToString());
    }
    public void PrintLogs(bool useColors = false)
    {
        foreach (var message in Messages)
        {
            message.Print(useColors);
        }
    }

    // helper methods
    public void Raw(string message) { Log(new LogMessage(LogLevel.Raw, message)); }
    public void Info(string message) { Log(new LogMessage(LogLevel.Info, message)); }
    public void Success(string message) { Log(new LogMessage(LogLevel.Success, message)); }
    public void Failure(string message) { Log(new LogMessage(LogLevel.Failure, message)); }
    public void Dialogue(string message) { Log(new LogMessage(LogLevel.Dialogue, message)); }
    public void Warning(string message) { Log(new LogMessage(LogLevel.Warning, message)); }
    public void Error(string message) { Log(new LogMessage(LogLevel.Error, message)); }
    public void Critical(string message) { Log(new LogMessage(LogLevel.Critical, message)); }

    // used to separate log messages
    public void Sep(int n = 1)
    {
        for (int i = 0; i < n; i++)
        {
            Log(new LogMessage(LogLevel.Blank, ""));
        }
    }
}

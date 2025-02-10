namespace slap.Logging;

using slap.UI;

public class LogMessage
{
    public LogLevel LogLevel { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public LogMessage(LogLevel logLevel, string message)
    {
        LogLevel = logLevel;
        Message = message;
        CreatedAt = Sim.Now;
    }
    public override string ToString()
    {
        if (LogLevel == LogLevel.Blank) return "";
        return $"[{CreatedAt.ToString("yyyy-MM-dd HH:mm:ss.fff")}] ({LogLevel.ToUpperString()}) {Message}";
    }
    public void Print(bool useColors = false)
    {
        var color = LogLevel.GetColor();
        var colorCode = "";
        if (useColors)
        {
            Console.ForegroundColor = color;
            colorCode = color.ToCustomColorCode();
        }
        Console.WriteLine(ToString());
    }
    // This actually means "add to a logger".
    public void Log()
    {
        Sim.Log.Log(this.LogLevel, this.Message);
    }
}

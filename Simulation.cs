namespace slap;
using slap.Logging;

public static class Simulation
{
    private static TimeSpan _addedTime;
    public static DateTime Now => DateTime.Now + _addedTime;
    private static Logger? _logger { get; set; }
    public static void SetLogger(Logger logger)
    {
        _logger = logger;
    }
    public static void Log(LogLevel logLevel, string message)
    {
        if (_logger != null) { _logger.Log(logLevel, message); }
    }
    // skips in time by the timespan, Simulation.Now is used instead of DateTime.Now across slap
    public static void Wait(TimeSpan timeSpan)
    {
        if (timeSpan < new TimeSpan(0))
        {
            throw new Exception("Time can only move forward.");
        }
        _addedTime += timeSpan;
    }
    public static void WaitYears(double years)
    {
        Wait(TimeSpan.FromDays(years * 365));
    }
    public static void RandomDayTime()
    {
        Wait(
                  TimeSpan.FromHours(Random.Next(0, 60))
                + TimeSpan.FromMinutes(Random.Next(0, 60))
                + TimeSpan.FromSeconds(Random.Next(0, 60))
                + TimeSpan.FromMilliseconds(Random.Next(0, 1000))
            );
    }
    public static Random Random { get; set; } = Random.Shared; // or use a seed
}

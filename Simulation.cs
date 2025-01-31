namespace slap;

public static class Simulation
{
    private static TimeSpan _addedTime;
    public static DateTime Now => DateTime.Now + _addedTime;
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
    public static Random Random { get; set; } = new(1); // use seed 1 for reproducibility
}

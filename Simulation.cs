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
}

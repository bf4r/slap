namespace slap;
using slap.Logging;

public static class Sim
{
    private static TimeSpan _addedTime;
    public static DateTime Now => DateTime.Now + _addedTime;
    public static Logger Log { get; set; } = new();
    // skips in time by the timespan, Sim.Now is used instead of DateTime.Now across slap
    public static void Wait(TimeSpan timeSpan)
    {
        if (timeSpan < new TimeSpan(0))
        {
            throw new Exception("Time can only move forward.");
        }
        _addedTime += timeSpan;
    }
    public static void WaitYears(double years) => Wait(TimeSpan.FromDays(years * 365));
    public static void WaitMonths(double months) => Wait(TimeSpan.FromDays(months * 30.436875));
    public static void WaitWeeks(double weeks) => Wait(TimeSpan.FromDays(weeks * 7));
    public static void WaitDays(double days) => Wait(TimeSpan.FromDays(days));
    public static void WaitHours(double hours) => Wait(TimeSpan.FromHours(hours));
    public static void WaitMinutes(double minutes) => Wait(TimeSpan.FromMinutes(minutes));
    public static void WaitSeconds(double seconds) => Wait(TimeSpan.FromSeconds(seconds));
    public static void WaitMilliseconds(double milliseconds) => Wait(TimeSpan.FromMilliseconds(milliseconds));
    public static void WaitMicroseconds(double microseconds) => Wait(TimeSpan.FromMicroseconds(microseconds));
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

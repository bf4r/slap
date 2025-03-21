namespace slap;

using slap.Logging;
using slap.Things;
using slap.Things.Society.People;
using slap.UI;

public static class Sim
{
    public static List<Thing> Stuff { get; set; } = new();
    private static TimeSpan _addedTime;
    public static TimeSpan UpdateFrequency = TimeSpan.FromMilliseconds(1);
    public static Logger Log { get; set; } = new();
    public static Shared Shared { get; set; } = new();
    public static double Speed
    {
        get
        {
            return CurrentSpeedFactor + 1;
        }
        set
        {
            CurrentSpeedFactor = value - 1;
        }
    }
    private static double CurrentSpeedFactor = 1;
    private static DateTime _lastSpeedChange = DateTime.Now;
    public static List<(DateTime time, Action action)> ScheduledActions = new();

    public static DateTime Now
    {
        get
        {
            UpdateAddedTime();
            return DateTime.Now + _addedTime;
        }
    }

    private class TimeSpeedLog
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double SpeedFactor { get; set; }
    }

    private static void UpdateAddedTime()
    {
        var currentRealTime = DateTime.Now;

        var realTimeSinceLastChange = currentRealTime - _lastSpeedChange;
        var simulatedTimeSinceLastChange = TimeSpan.FromTicks((long)(realTimeSinceLastChange.Ticks * CurrentSpeedFactor));
        _addedTime += simulatedTimeSinceLastChange;

        _lastSpeedChange = currentRealTime;
    }

    public static void ResetTime()
    {
        _addedTime = TimeSpan.Zero;
        CurrentSpeedFactor = 1;
        _lastSpeedChange = DateTime.Now;
    }

    public static TimeSpan GetTotalSimulationTime()
    {
        var totalTime = TimeSpan.Zero;

        var currentRealTime = DateTime.Now;
        var currentPeriodDuration = currentRealTime - _lastSpeedChange;
        totalTime += TimeSpan.FromTicks((long)(currentPeriodDuration.Ticks * CurrentSpeedFactor));

        return totalTime;
    }
    public static void Wait(TimeSpan timeSpan)
    {
        if (timeSpan < TimeSpan.Zero)
        {
            throw new Exception("Time can only move forward.");
        }
        var realTimeToWait = TimeSpan.FromTicks((long)(timeSpan.Ticks / CurrentSpeedFactor));
        var currentRealTime = DateTime.Now;
        _addedTime += timeSpan;
        _lastSpeedChange = currentRealTime + realTimeToWait;
    }
    public static void Schedule(DateTime time, Action action)
    {
        ScheduledActions.Add(new(time, action));
    }
    public static void RunScheduledActions()
    {
        var actionsToRun = ScheduledActions.Where(sa => sa.time <= Sim.Now).ToList();
        ScheduledActions.RemoveAll(sa => sa.time <= Sim.Now);
        foreach (var scheduledAction in actionsToRun)
        {
            scheduledAction.action();
        }
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
    public static Random Random { get; set; } = Random.Shared; // ... or use a seed.

    public static void Run()
    {
        Console.CursorVisible = false;
        Console.Clear();
        Test.All();
        while (true)
        {
            foreach (var thing in Stuff)
            {
                switch (thing)
                {
                    case Person p:
                        p.Update();
                        break;
                    default:
                        thing.Update();
                        break;
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            SimUI.Draw();
            Thread.Sleep(UpdateFrequency);
            if (Console.KeyAvailable)
            {
                var ki = Console.ReadKey(true);
                if (int.TryParse(ki.KeyChar.ToString(), out int tabToSwitchTo))
                {
                    if (SimUI.TabNames.ContainsKey(tabToSwitchTo))
                    {
                        if (tabToSwitchTo != SimUI.CurrentTab)
                        {
                            SimUI.CurrentTab = tabToSwitchTo;
                            Console.Clear();
                        }
                    }
                    continue;
                }
                if (SimUI.IsFocusedOnFilter)
                {
                    var kc = ki.KeyChar;
                    if (ki.Key == ConsoleKey.Backspace)
                    {
                        if (SimUI.LogFilter.Length > 0)
                        {
                            SimUI.LogFilter = SimUI.LogFilter.Substring(0, SimUI.LogFilter.Length - 1);
                        }
                    }
                    else if (ki.Key == ConsoleKey.Escape)
                    {
                        SimUI.IsFocusedOnFilter = false;
                    }
                    else
                    {
                        SimUI.LogFilter += kc;
                    }
                    continue;
                }
                else if (ki.Key == ConsoleKey.Divide)
                {
                    SimUI.IsFocusedOnFilter = !SimUI.IsFocusedOnFilter;
                    continue;
                }
                switch (ki.Key)
                {
                    case ConsoleKey.Q:
                        Console.CursorVisible = true;
                        return;
                }
                switch (SimUI.CurrentTab)
                {
                    case 1:
                        {
                            switch (ki.Key)
                            {
                                case ConsoleKey.J:
                                    if (Sim.Speed > 1)
                                    {
                                        Console.Clear();
                                        Sim.Speed /= 2;
                                        Sim.Log.Success($"The simulation speed has been changed to {Sim.Speed}x.");
                                    }
                                    break;
                                case ConsoleKey.K:
                                    Console.Clear();
                                    Sim.Speed *= 2;
                                    Sim.Log.Success($"The simulation speed has been changed to {Sim.Speed}x.");
                                    break;
                            }
                        }
                        break;
                    case 2:
                        {
                            switch (ki.Key)
                            {
                                case ConsoleKey.H:
                                case ConsoleKey.A:
                                case ConsoleKey.LeftArrow:
                                    SimUI.StatusPage--;
                                    if (SimUI.StatusPage < 0) SimUI.StatusPage = 0;
                                    Console.Clear();
                                    break;
                                case ConsoleKey.L:
                                case ConsoleKey.D:
                                case ConsoleKey.RightArrow:
                                    SimUI.StatusPage++;
                                    var people = Sim.Stuff.Where(x => x is Person).ToList();
                                    var maxPages = people.Count / SimUI.PeoplePerPage;
                                    if (SimUI.StatusPage > maxPages)
                                    {
                                        SimUI.StatusPage = maxPages;
                                    }
                                    Console.Clear();
                                    break;
                            }
                            break;
                        }
                    case 3:
                        {
                            switch (ki.Key)
                            {
                                case ConsoleKey.W:
                                case ConsoleKey.K:
                                    Map.PlayerY--;
                                    break;
                                case ConsoleKey.A:
                                case ConsoleKey.H:
                                    Map.PlayerX--;
                                    break;
                                case ConsoleKey.S:
                                case ConsoleKey.J:
                                    Map.PlayerY++;
                                    break;
                                case ConsoleKey.D:
                                case ConsoleKey.L:
                                    Map.PlayerX++;
                                    break;
                            }
                        }
                        break;
                }
            }
        }
    }
}

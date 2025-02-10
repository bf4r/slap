namespace slap.UI;

using slap.Logging;
using System.Text;
using slap.Things.Society.People;

public static class SimUI
{
    public static StringBuilder TextMessages { get; set; } = new();
    public static string LogFilter { get; set; } = "";
    public static int CurrentTab { get; set; } = 1;
    public static Dictionary<int, string> TabNames = new()
        {
            { 1, "Logs" },
            { 2, "Status" },
        };
    public static bool IsFocusedOnFilter { get; set; } = false;
    public static int SupposedWidth = Console.WindowWidth;
    public static int SupposedHeight = Console.WindowHeight;
    static string GetValueColor(int val)
    {
        return val switch
        {
            < 10 => "&4",
            < 35 => "&c",
            < 70 => "&e",
            _ => "&a"
        };
    }
    public static void Draw()
    {
        var width = Console.WindowWidth;
        var height = Console.WindowHeight;
        if (width != SupposedWidth || height != SupposedHeight)
        {
            SupposedWidth = width;
            SupposedHeight = height;
            Console.Clear();
        }
        var white = ConsoleColor.White;
        var gray = ConsoleColor.DarkGray;

        switch (CurrentTab)
        {
            case 1:
                {
                    // Log window.
                    var filteredLogs = GetFilteredLogs();
                    ConsoleBox.Show(filteredLogs, 0, 0, width, height - 8, white);

                    // Log filter window.
                    var logFilterText = "";
                    if (LogFilter == "" && !IsFocusedOnFilter)
                    {
                        logFilterText = "&8Type '/' to filter logs...";
                    }
                    else if (LogFilter == "" && IsFocusedOnFilter)
                    {
                        logFilterText = "";
                    }
                    else if (LogFilter.Length > 0 && !IsFocusedOnFilter)
                    {
                        logFilterText = "&f" + LogFilter + "&8 (Type '/' to edit...)";
                    }
                    else
                    {
                        logFilterText = "&f" + LogFilter;
                    }
                    ConsoleBox.Show(logFilterText, 0, height - 8, width, 3, white);

                    // Simulation clock.
                    var time = "(" + (Sim.CurrentSpeedFactor + 1).ToString() + "x) " + Sim.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    ConsoleBox.Show(time, 0, height - 3, time.Length + 2, 3, white);
                }
                break;
            case 2:
                var cellHeight = 6;
                var cellsPerRow = 3;
                var it = 0;
                foreach (var thing in Sim.Stuff.Where(x => x is Person))
                {
                    var person = thing as Person;

                    int row = it / cellsPerRow;
                    int col = it % cellsPerRow;
                    int xPos = col * (width / cellsPerRow);
                    int yPos = row * cellHeight;

                    if (person != null)
                    {
                        var sb = new StringBuilder();
                        var sleepingText = " &fzZz";
                        if (!person.IsSleeping) sleepingText = "";
                        sb.AppendLine($"&b{person.Who()}{sleepingText}");
                        sb.AppendLine($"&fEnergy: {GetValueColor(person.Energy)}{person.Energy}%");
                        sb.AppendLine($"&fFood: {GetValueColor(person.Fullness)}{person.Fullness}%");
                        sb.AppendLine($"&fWater: {GetValueColor(person.Hydration)}{person.Hydration}%");

                        ConsoleBox.Show(sb.ToString(),
                            xPos,
                            yPos,
                            width / cellsPerRow - 1,
                            cellHeight,
                            white);
                    }
                    it++;
                }
                break;
        }
        var tabBarWidth = 42;
        var currTabExists = TabNames.ContainsKey(CurrentTab);
        if (currTabExists)
        {
            ConsoleBox.Show("", width / 2 - tabBarWidth / 2, height - 5, tabBarWidth, 5, white);
            // Center (current) tab.
            var tabText = CurrentTab + " " + TabNames[CurrentTab];
            ConsoleBox.Show(tabText, width / 2 - tabText.Length / 2, height - 4, tabText.Length + 2, 3, white);
            if (TabNames.ContainsKey(CurrentTab - 1))
            {
                // Previous tab. (left)
                var prevTabText = (CurrentTab - 1) + " " + TabNames[CurrentTab - 1];
                ConsoleBox.Show(prevTabText, width / 2 - prevTabText.Length / 2 - tabText.Length - 1, height - 4, prevTabText.Length + 2, 3, gray);
            }
            if (TabNames.ContainsKey(CurrentTab + 1))
            {
                // Next tab. (right)
                var nextTabText = (CurrentTab + 1) + " " + TabNames[CurrentTab + 1];
                ConsoleBox.Show(nextTabText, width / 2 - nextTabText.Length / 2 + tabText.Length + 3, height - 4, nextTabText.Length + 2, 3, gray);
            }
        }
    }
    public static string GetFilteredLogs()
    {
        Sim.Log.Filter([LogFilter]);
        var newMessages = new List<LogMessage>();
        foreach (var message in Sim.Log.Messages.OrderBy(x => x.CreatedAt))
        {
            // If there are no filters, add the message.
            // If there are filters that match the message, add the message.
            //
            // If the message has something that contains at least one thing from each list in Filters, add the message.
            bool matchesAllFilters = true;
            foreach (List<string> filter in Sim.Log.Filters)
            {
                bool matchesCurrentFilter = false;
                foreach (string filterWord in filter)
                {
                    if (message.Message.Contains(filterWord, StringComparison.OrdinalIgnoreCase))
                    {
                        matchesCurrentFilter = true;
                        break;
                    }
                }
                if (!matchesCurrentFilter)
                {
                    matchesAllFilters = false;
                    break;
                }
            }
            if (Sim.Log.Filters.Count == 0 || matchesAllFilters)
            {
                newMessages.Add(message);
            }
        }
        var sb = new StringBuilder();
        foreach (var message in newMessages)
        {
            sb.AppendLine(message.LogLevel.GetColor().ToCustomColorCode() + message.ToString());
        }
        return sb.ToString();
    }
}

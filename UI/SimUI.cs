namespace slap.UI;

using System.Text;

public static class SimUI
{
    public static StringBuilder Logs { get; set; } = new();
    public static int CurrentTab { get; set; } = 1;
    public static Dictionary<int, string> TabNames = new()
        {
            { 1, "Logs" },
            { 2, "Status" },
        };
    public static void Draw()
    {
        var width = Console.WindowWidth;
        var height = Console.WindowHeight;
        var white = 0xFFFFFF;
        var gray = 0x777777;


        switch (CurrentTab)
        {
            case 1:
                {
                    // Log window.
                    ConsoleBox.Show(Logs.ToString(), 0, 0, width / 2, height - 5, white);

                    // Simulation clock.
                    var time = "(" + Sim.CurrentSpeedFactor.ToString() + "x) " + Sim.Now.ToString();
                    ConsoleBox.Show(time, 0, height - 3, time.Length + 2, 3, white);
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
}

namespace slap.UI;

using System.Text;

public static class SimUI
{
    public static StringBuilder Logs { get; set; } = new();
    public static void Draw()
    {
        var width = Console.WindowWidth;
        var height = Console.WindowHeight;
        var white = 0xFFFFFF;

        // Log window.
        ConsoleBox.Show(Logs.ToString(), 0, 0, width / 2, height - 3, white);

        // Simulation clock.
        var time = "(" + Sim.CurrentSpeedFactor.ToString() + "x)" + Sim.Now.ToString();
        ConsoleBox.Show(time, 0, height - 3, time.Length + 2, 3, white);
    }
}

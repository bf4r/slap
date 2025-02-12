namespace slap.UI;

using slap.Things.Society.People;
using System.Text;

public static class Map
{
    public static int PlayerX = 0;
    public static int PlayerY = 0;
    public static void Draw()
    {
        var width = Console.WindowWidth - 10;
        var height = Console.WindowHeight - 5;
        var offsetX = -width / 2 + PlayerX;
        var offsetY = -height / 2 + PlayerY;
        var people = Sim.Stuff.Where(x => x is Person).ToList();
        var sb = new StringBuilder();

        Dictionary<(int, int), char> targetPositions = [];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var p = people.FirstOrDefault(p => p.Location != null &&
                                                 p.Location.X == x + offsetX &&
                                                 p.Location.Y == y + offsetY);
                if (p != null)
                {
                    targetPositions[(x, y)] = 'O';
                    targetPositions[(x, y + 1)] = '|';
                    targetPositions[(x - 1, y + 1)] = '/';
                    targetPositions[(x + 1, y + 1)] = '\\';
                    targetPositions[(x - 1, y + 2)] = '/';
                    targetPositions[(x + 1, y + 2)] = '\\';
                }
            }
        }
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (targetPositions.ContainsKey((x, y)))
                {
                    sb.Append(targetPositions[(x, y)]);
                }
                else sb.Append(' ');
            }
            sb.AppendLine();
        }
        Console.SetCursorPosition(0, 0);
        Console.Write(sb.ToString());
        Console.SetCursorPosition(0, 0);
        Console.Write($"[{PlayerX}, {PlayerY}]");
        Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2);
        Console.Write($"P");
    }
}

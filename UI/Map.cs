namespace slap.UI;

using slap.Things.Society.People;
using slap.Things;
using System.Text;
using slap.Things.Society.People.Identity;

public static class Map
{
    public static int PlayerX = 0;
    public static int PlayerY = 0;
    public static void Draw()
    {
        var width = Console.WindowWidth;
        var height = Console.WindowHeight - 5;
        var offsetX = -width / 2 + PlayerX;
        var offsetY = -height / 2 + PlayerY;
        var people = Sim.Stuff.Where(x => x is Person).ToList();
        var sb = new StringBuilder();

        Dictionary<(int, int), (char, ConsoleColor)> targetPositions = [];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var p = people.FirstOrDefault(p => p.Location != null &&
                                                 p.Location.X == x + offsetX &&
                                                 p.Location.Y == y + offsetY) as Person;
                if (p != null)
                {
                    var color = p.Gender switch
                    {
                        Gender.Male => ConsoleColor.Blue,
                        Gender.Female => ConsoleColor.Magenta,
                        _ => ConsoleColor.Yellow
                    };
                    targetPositions[(x, y)] = ('O', color);
                    targetPositions[(x, y + 1)] = ('|', color);
                    targetPositions[(x - 1, y + 1)] = ('/', color);
                    targetPositions[(x + 1, y + 1)] = ('\\', color);
                    targetPositions[(x - 1, y + 2)] = ('/', color);
                    targetPositions[(x + 1, y + 2)] = ('\\', color);
                    var who = p.Who();
                    var nameColor = ConsoleColor.DarkGray;
                    if (p.Location != null && Math.Abs(p.Location.DistanceTo(new Location(null, null, PlayerX, PlayerY))) < 7)
                    {
                        nameColor = ConsoleColor.White;
                    }
                    PutStringAt(x: x - who.Length / 2, y: y - 2, who, nameColor, targetPositions);
                }
            }
        }
        PutStringAt(x: 0, y: 0, $"[{PlayerX}, {PlayerY}]", ConsoleColor.White, targetPositions);
        Console.SetCursorPosition(0, 0);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (targetPositions.ContainsKey((x, y)))
                {
                    var (character, color) = targetPositions[(x, y)];
                    Console.ForegroundColor = color;
                    Console.Write(character);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(' ');
                }
            }
            Console.WriteLine();
        }

        Console.ForegroundColor = ConsoleColor.White;
    }
    private static void PutStringAt(int x, int y, string str, ConsoleColor color, Dictionary<(int, int), (char, ConsoleColor)> targetPositions)
    {
        for (int i = 0; i < str.Length; i++)
        {
            targetPositions[(x + i, y)] = (str[i], color);
        }
    }
}

namespace slap.UI;

using System.Text.RegularExpressions;

public class ConsoleBox
{
    private static readonly Regex ColorCodeRegex = new Regex(@"&([a-f0-9]|#[0-9A-Fa-f]{6})");

    public static void Show(string text, int x, int y, int width, int height, ConsoleColor borderColor, int offset = -1)
    {
        int consoleWidth = Console.WindowWidth;
        int consoleHeight = Console.WindowHeight;

        x = Math.Max(0, Math.Min(x, consoleWidth - 1));
        y = Math.Max(0, Math.Min(y, consoleHeight - 1));
        width = Math.Min(width, consoleWidth - x);
        height = Math.Min(height, consoleHeight - y);

        const char topLeft = '┌';
        const char topRight = '┐';
        const char bottomLeft = '└';
        const char bottomRight = '┘';
        const char horizontal = '─';
        const char vertical = '│';

        width = Math.Max(width, 3);
        height = Math.Max(height, 3);

        int maxTextWidth = width - 2;
        int maxTextHeight = height - 2;

        List<(string Text, ConsoleColor Color)> coloredParts = SplitIntoColoredParts(text);
        List<List<(string Text, ConsoleColor Color)>> wrappedText = WrapText(coloredParts, maxTextWidth);

        if (offset == -1)
        {
            offset = wrappedText.Count > maxTextHeight ? wrappedText.Count - maxTextHeight : 0;
        }

        offset = Math.Max(0, Math.Min(offset, Math.Max(0, wrappedText.Count - maxTextHeight)));

        ConsoleColor originalColor = Console.ForegroundColor;
        Console.ForegroundColor = borderColor;

        SafeWrite(x, y, $"{topLeft}{new string(horizontal, width - 2)}{topRight}");

        for (int i = 0; i < maxTextHeight; i++)
        {
            Console.ForegroundColor = borderColor;
            SafeWrite(x, y + i + 1, $"{vertical}");

            int textIndex = i + offset;
            if (textIndex < wrappedText.Count)
            {
                PrintColoredLine(x + 1, y + i + 1, wrappedText[textIndex], maxTextWidth);
            }
            else
            {
                SafeWrite(x + 1, y + i + 1, new string(' ', maxTextWidth));
            }

            Console.ForegroundColor = borderColor;
            SafeWrite(x + width - 1, y + i + 1, $"{vertical}");
        }

        Console.ForegroundColor = borderColor;
        SafeWrite(x, y + height - 1, $"{bottomLeft}{new string(horizontal, width - 2)}{bottomRight}");
        Console.ForegroundColor = originalColor;
    }

    public static void ShowCenter(string text, int offset = -1)
    {
        int consoleWidth = Console.WindowWidth;
        string[] lines = text.Split('\n');

        List<string> wrappedLines = new List<string>();
        foreach (string line in lines)
        {
            string cleanLine = line.RemoveColorCodes();
            if (cleanLine.Length <= consoleWidth - 4)
            {
                wrappedLines.Add(line);
            }
            else
            {
                int currentIndex = 0;
                while (currentIndex < line.Length)
                {
                    int remainingLength = line.Length - currentIndex;
                    int chunkSize = Math.Min(consoleWidth - 4, remainingLength);
                    wrappedLines.Add(line.Substring(currentIndex, chunkSize));
                    currentIndex += chunkSize;
                }
            }
        }

        int width = Math.Min(wrappedLines.Max(x => x.RemoveColorCodes().Length) + 2, consoleWidth - 2);
        int height = wrappedLines.Count + 2;

        int centerX = (consoleWidth / 2) - width / 2;
        int centerY = (Console.WindowHeight / 2) - height / 2;

        string wrappedText = string.Join("\n", wrappedLines);
        Show(wrappedText, centerX, centerY, width, height, ConsoleColor.White, offset);
    }

    private static List<(string Text, ConsoleColor Color)> SplitIntoColoredParts(string text)
    {
        var parts = new List<(string Text, ConsoleColor Color)>();
        var matches = ColorCodeRegex.Matches(text);
        int lastIndex = 0;
        ConsoleColor currentColor = ConsoleColor.Gray;

        foreach (Match match in matches)
        {
            if (match.Index > lastIndex)
            {
                var subtext = text.Substring(lastIndex, match.Index - lastIndex);
                var lines = subtext.Split(new[] { '\n' }, StringSplitOptions.None);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (i > 0)
                    {
                        parts.Add(("\n", currentColor));
                    }
                    if (!string.IsNullOrEmpty(lines[i]))
                    {
                        parts.Add((lines[i], currentColor));
                    }
                }
            }

            currentColor = ParseConsoleColor(match.Groups[1].Value);
            lastIndex = match.Index + match.Length;
        }

        if (lastIndex < text.Length)
        {
            var subtext = text.Substring(lastIndex);
            var lines = subtext.Split(new[] { '\n' }, StringSplitOptions.None);
            for (int i = 0; i < lines.Length; i++)
            {
                if (i > 0)
                {
                    parts.Add(("\n", currentColor));
                }
                if (!string.IsNullOrEmpty(lines[i]))
                {
                    parts.Add((lines[i], currentColor));
                }
            }
        }

        return parts;
    }

    private static void PrintColoredLine(int x, int y, List<(string Text, ConsoleColor Color)> line, int maxWidth)
    {
        int currentX = x;
        int visibleLength = 0;

        foreach (var part in line)
        {
            int remainingWidth = maxWidth - visibleLength;
            if (remainingWidth <= 0) break;

            Console.ForegroundColor = part.Color;
            string visibleText = part.Text.Length > remainingWidth
                ? part.Text.Substring(0, remainingWidth)
                : part.Text;

            SafeWrite(currentX, y, visibleText);
            currentX += visibleText.Length;
            visibleLength += visibleText.Length;
        }

        if (visibleLength < maxWidth)
        {
            SafeWrite(currentX, y, new string(' ', maxWidth - visibleLength));
        }
    }

    private static void SafeWrite(int x, int y, string text)
    {
        if (x >= 0 && y >= 0 && y < Console.WindowHeight)
        {
            try
            {
                Console.SetCursorPosition(x, y);
                Console.Write(text);
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }
    }

    private static ConsoleColor ParseConsoleColor(string colorCode)
    {
        return colorCode switch
        {
            "0" => ConsoleColor.Black,
            "1" => ConsoleColor.DarkBlue,
            "2" => ConsoleColor.DarkGreen,
            "3" => ConsoleColor.DarkCyan,
            "4" => ConsoleColor.DarkRed,
            "5" => ConsoleColor.DarkMagenta,
            "6" => ConsoleColor.DarkYellow,
            "7" => ConsoleColor.Gray,
            "8" => ConsoleColor.DarkGray,
            "9" => ConsoleColor.Blue,
            "a" => ConsoleColor.Green,
            "b" => ConsoleColor.Cyan,
            "c" => ConsoleColor.Red,
            "d" => ConsoleColor.Magenta,
            "e" => ConsoleColor.Yellow,
            "f" => ConsoleColor.White,
            _ => ConsoleColor.Gray,
        };
    }

    private static List<List<(string Text, ConsoleColor Color)>> WrapText(
        List<(string Text, ConsoleColor Color)> coloredParts, int maxWidth)
    {
        var result = new List<List<(string Text, ConsoleColor Color)>>();
        var currentLine = new List<(string Text, ConsoleColor Color)>();
        int currentLineLength = 0;

        foreach (var part in coloredParts)
        {
            if (part.Text == "\n")
            {
                if (currentLine.Count > 0)
                {
                    result.Add(currentLine);
                }
                currentLine = new List<(string Text, ConsoleColor Color)>();
                currentLineLength = 0;
                continue;
            }

            string[] words = part.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words)
            {
                if (currentLineLength + word.Length + (currentLineLength > 0 ? 1 : 0) > maxWidth)
                {
                    if (currentLine.Count > 0)
                    {
                        result.Add(currentLine);
                        currentLine = new List<(string Text, ConsoleColor Color)>();
                        currentLineLength = 0;
                    }
                }

                if (currentLineLength > 0)
                {
                    currentLine.Add((" ", part.Color));
                    currentLineLength += 1;
                }

                currentLine.Add((word, part.Color));
                currentLineLength += word.Length;
            }
        }

        if (currentLine.Count > 0)
        {
            result.Add(currentLine);
        }

        return result;
    }
}

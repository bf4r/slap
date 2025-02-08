namespace slap.UI;

using System.Text.RegularExpressions;

public class ConsoleBox
{
    private static readonly Regex ColorCodeRegex = new Regex(@"&([a-f0-9]|#[0-9A-Fa-f]{6})");

    public static void Show(string text, int x, int y, int width, int height, int borderColor = 0xFFFFFF, int offset = -1)
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

        List<(string Text, string Color)> coloredParts = SplitIntoColoredParts(text);
        List<List<(string Text, string Color)>> wrappedText = WrapText(coloredParts, maxTextWidth);

        if (offset == -1)
        {
            if (wrappedText.Count > maxTextHeight)
            {
                offset = wrappedText.Count - maxTextHeight;
            }
            else
            {
                offset = 0;
            }
        }

        offset = Math.Max(0, Math.Min(offset, Math.Max(0, wrappedText.Count - maxTextHeight)));

        string resetColor = "\u001b[0m";
        string boxColor = UIHelpers.GetColorCode(borderColor);

        SafeWrite(x, y, $"{boxColor}{topLeft}{new string(horizontal, width - 2)}{topRight}");

        for (int i = 0; i < maxTextHeight; i++)
        {
            SafeWrite(x, y + i + 1, $"{boxColor}{vertical}");

            int textIndex = i + offset;
            if (textIndex < wrappedText.Count)
            {
                PrintColoredLine(x + 1, y + i + 1, wrappedText[textIndex], maxTextWidth);
            }
            else
            {
                SafeWrite(x + 1, y + i + 1, new string(' ', maxTextWidth));
            }

            SafeWrite(x + width - 1, y + i + 1, $"{boxColor}{vertical}");
        }

        SafeWrite(x, y + height - 1, $"{boxColor}{bottomLeft}{new string(horizontal, width - 2)}{bottomRight}{resetColor}");
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

        int centerX = (int)(Console.WindowWidth / 2) - width / 2;
        int centerY = (int)(Console.WindowHeight / 2) - height / 2;

        string wrappedText = string.Join("\n", wrappedLines);
        ConsoleBox.Show(wrappedText, centerX, centerY, width, height, 0xFFFFFF, offset);
    }
    public static string ShowTextForWrapping(string text, int maxLineLength)
    {
        var words = text.Split(' ');
        var wrappedLines = new List<string>();
        var currentLine = "";

        foreach (var word in words)
        {
            if (currentLine.Length + word.Length + 1 > maxLineLength)
            {
                wrappedLines.Add(currentLine);
                currentLine = word;
            }
            else
            {
                currentLine += (currentLine.Length == 0 ? "" : " ") + word;
            }
        }

        if (!string.IsNullOrEmpty(currentLine))
        {
            wrappedLines.Add(currentLine);
        }

        return string.Join("\n", wrappedLines);
    }
    private static List<(string Text, string Color)> SplitIntoColoredParts(string text)
    {
        var parts = new List<(string Text, string Color)>();
        var matches = ColorCodeRegex.Matches(text);
        int lastIndex = 0;
        string currentColor = "\u001b[37m";

        foreach (Match match in matches)
        {
            if (match.Index > lastIndex)
            {
                var subtext = text.Substring(lastIndex, match.Index - lastIndex);
                parts.Add((subtext, currentColor));
            }

            currentColor = ParseColor(match.Groups[1].Value);
            lastIndex = match.Index + match.Length;
        }

        if (lastIndex < text.Length)
        {
            var subtext = text.Substring(lastIndex);
            parts.Add((subtext, currentColor));
        }

        return parts;
    }

    private static List<List<(string Text, string Color)>> WrapText(List<(string Text, string Color)> coloredParts, int maxWidth)
    {
        var result = new List<List<(string Text, string Color)>>();
        var currentLine = new List<(string Text, string Color)>();
        int currentLineVisibleLength = 0;

        foreach (var part in coloredParts)
        {
            string[] lines = part.Text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                if (i > 0)
                {
                    if (currentLine.Count > 0 || currentLineVisibleLength > 0)
                    {
                        result.Add(currentLine);
                        currentLine = new List<(string Text, string Color)>();
                        currentLineVisibleLength = 0;
                    }
                }

                string line = lines[i];
                int lineVisibleLength = GetVisibleLength(line);

                if (currentLineVisibleLength + lineVisibleLength > maxWidth)
                {
                    if (currentLine.Count > 0 || currentLineVisibleLength > 0)
                    {
                        result.Add(currentLine);
                        currentLine = new List<(string Text, string Color)>();
                        currentLineVisibleLength = 0;
                    }

                    while (lineVisibleLength > maxWidth)
                    {
                        int splitIndex = FindSplitIndex(line, maxWidth);
                        currentLine.Add((line.Substring(0, splitIndex), part.Color));
                        result.Add(currentLine);
                        currentLine = new List<(string Text, string Color)>();
                        line = line.Substring(splitIndex);
                        lineVisibleLength = GetVisibleLength(line);
                        currentLineVisibleLength = 0;
                    }
                }

                if (lineVisibleLength > 0)
                {
                    currentLine.Add((line, part.Color));
                    currentLineVisibleLength += lineVisibleLength;
                }
            }
        }

        if (currentLine.Count > 0 || currentLineVisibleLength > 0)
        {
            result.Add(currentLine);
        }

        return result;
    }

    private static void SafeWrite(int x, int y, string text)
    {
        if (x >= 0 && y >= 0 && y < Console.WindowHeight)
        {
            int windowWidth = Console.WindowWidth;

            if (x >= windowWidth)
            {
                return;
            }
            int remainingWidth = windowWidth - x + 19;
            var checkText = text.RemoveColorCodes();
            if (checkText.Length > remainingWidth)
            {
                text = checkText.Substring(0, remainingWidth);
            }

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

    private static void PrintColoredLine(int x, int y, List<(string Text, string Color)> line, int maxWidth)
    {
        int currentX = x;
        int visibleLength = 0;
        foreach (var part in line)
        {
            int remainingWidth = maxWidth - visibleLength;
            if (remainingWidth > 0)
            {
                SafeWrite(currentX, y, part.Color);
                string visibleText = TruncateVisibleLength(part.Text, remainingWidth);
                SafeWrite(currentX, y, visibleText);
                currentX += GetVisibleLength(visibleText);
                visibleLength += GetVisibleLength(visibleText);
            }
            else
            {
                break;
            }
        }
        if (visibleLength < maxWidth)
        {
            SafeWrite(currentX, y, new string(' ', maxWidth - visibleLength));
        }
        SafeWrite(x + maxWidth, y, "\u001b[0m");
    }

    private static int GetVisibleLength(string text)
    {
        return Regex.Replace(text, @"\u001b\[[0-9;]*[mGK]|\u001b\[38;2;\d+;\d+;\d+m", "").Length;
    }

    private static string TruncateVisibleLength(string text, int maxLength)
    {
        int visibleLength = 0;
        int index = 0;
        while (index < text.Length && visibleLength < maxLength)
        {
            if (text[index] == '\u001b')
            {
                int endIndex = text.IndexOf('m', index);
                if (endIndex != -1)
                {
                    if (text.Substring(index, endIndex - index).Contains("38;2;"))
                    {
                        index = endIndex + 1;
                        continue;
                    }
                }
            }
            visibleLength++;
            index++;
        }
        return text.Substring(0, index);
    }

    private static int FindSplitIndex(string text, int maxLength)
    {
        int visibleLength = 0;
        int index = 0;
        while (index < text.Length && visibleLength < maxLength)
        {
            if (text[index] == '\u001b')
            {
                int endIndex = text.IndexOf('m', index);
                if (endIndex != -1)
                {
                    index = endIndex + 1;
                    continue;
                }
            }
            visibleLength++;
            index++;
        }
        return index;
    }

    private static string ParseColor(string colorCode)
    {
        if (colorCode.StartsWith("#"))
        {
            int rgb = Convert.ToInt32(colorCode.Substring(1), 16);
            int r = (rgb >> 16) & 255;
            int g = (rgb >> 8) & 255;
            int b = rgb & 255;
            return $"\u001b[38;2;{r};{g};{b}m";
        }

        return colorCode switch
        {
            "0" => "\u001b[30m", // Black
            "1" => "\u001b[34m", // Dark Blue
            "2" => "\u001b[32m", // Dark Green
            "3" => "\u001b[36m", // Dark Cyan
            "4" => "\u001b[31m", // Dark Red
            "5" => "\u001b[35m", // Dark Magenta
            "6" => "\u001b[33m", // Dark Yellow
            "7" => "\u001b[37m", // Gray
            "8" => "\u001b[90m", // Dark Gray
            "9" => "\u001b[94m", // Blue
            "a" => "\u001b[92m", // Green
            "b" => "\u001b[96m", // Cyan
            "c" => "\u001b[91m", // Red
            "d" => "\u001b[95m", // Magenta
            "e" => "\u001b[93m", // Yellow
            "f" => "\u001b[97m", // White
            _ => "\u001b[37m",   // Default to reset
        };
    }
}

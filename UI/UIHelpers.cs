namespace slap.UI;

using System.Text.RegularExpressions;

public static class UIHelpers
{
    public static string GetColorCode(int rgb)
    {
        int red = (rgb >> 16) & 255;
        int green = (rgb >> 8) & 255;
        int blue = rgb & 255;
        return $"\x1b[38;2;{red};{green};{blue}m";
    }
    public static string RemoveColorCodes(this string str)
    {
        //remove any &x or &#(6 digit hex)
        str = Regex.Replace(str, "&[0-9a-f]", "");
        str = Regex.Replace(str, "&#[0-9a-f]{6}", "");
        return str;
    }
    public static string StripAnsiColorCodes(this string input)
    {
        string pattern = @"
        \u001b\[                  # ESC [
        (?:                       # Group for color codes
          [0-9;]*[mGK]            # Standard ANSI color codes
          |                       # OR
          38;2;\d+;\d+;\d+m       # True color (24-bit) codes
          |                       # OR
          48;2;\d+;\d+;\d+m       # True color (24-bit) background codes
        )
    ";

        return Regex.Replace(input, pattern, "", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
    }
}
public static class ConsoleColorExtensions
{
    public static string ToCustomColorCode(this ConsoleColor color)
    {
        return color switch
        {
            ConsoleColor.Black => "&0",
            ConsoleColor.DarkBlue => "&1",
            ConsoleColor.DarkGreen => "&2",
            ConsoleColor.DarkCyan => "&3",
            ConsoleColor.DarkRed => "&4",
            ConsoleColor.DarkMagenta => "&5",
            ConsoleColor.Yellow => "&e",
            ConsoleColor.White => "&f",
            ConsoleColor.Gray => "&7",
            ConsoleColor.Blue => "&9",
            ConsoleColor.Green => "&a",
            ConsoleColor.Cyan => "&b",
            ConsoleColor.Red => "&c",
            ConsoleColor.Magenta => "&d",
            ConsoleColor.DarkYellow => "&6",
            ConsoleColor.DarkGray => "&8",
            _ => "&r" // Reset
        };
    }
}

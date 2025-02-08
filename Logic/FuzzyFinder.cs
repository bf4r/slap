namespace slap.Logic;

public static class FuzzyFinder
{
    public static bool ContainsInOrder(string str, string chars)
    {
        if (string.IsNullOrEmpty(chars)) return true;

        var remainingChars = chars;
        var i = 0;
        foreach (var c in chars)
        {
            var subs = str.Substring(i);
            if (!subs.Contains(c)) return false;

            i = subs.IndexOf(c) + 1;
            remainingChars = remainingChars.Substring(1);
        }
        return true;
    }
}

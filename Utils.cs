namespace slap;

public static class Utils
{
    public static string CapitalizeFirst(this string str)
    {
        if (str.Length == 0) return str;
        return char.ToUpper(str[0]) + str.Substring(1);
    }
    public static bool HasBeen(this DateTime pointInTime, TimeSpan thisLong)
    {
        return Sim.Now - pointInTime >= thisLong;
    }
    public static string GetRandomWord()
    {
        List<string> words = new()
        {
            "time", "person", "year", "way", "day", "thing", "man", "world", "life", "hand",
            "part", "child", "eye", "woman", "place", "work", "week", "case", "point", "government",
            "company", "number", "group", "problem", "fact", "money", "water", "month", "lot", "right",
            "study", "book", "job", "word", "business", "issue", "side", "kind", "head", "house",
            "service", "friend", "power", "hour", "game", "line", "end", "member", "law", "car"
        };
        return words[Sim.Random.Next(words.Count)];
    }
}

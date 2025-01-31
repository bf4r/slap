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
}

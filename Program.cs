namespace slap;

class Program
{
    static void Main(string[] args)
    {
        Thing thing = new Thing("Thing", "Just a thing.");
        Console.WriteLine($"A thing has been created, its name is {thing.Name ?? "unknown"}.");
        if (thing.Description != null)
        {
            Console.WriteLine($"More about {thing.Name ?? "the unknown thing"}: {thing.Description}");
        }
    }
}

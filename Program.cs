namespace slap;

class Program
{
    static void Main(string[] args)
    {
        // Sim.Log.OnMessage = (message) =>
        // {
        //     message.Print(useColors: true);
        // };

        try
        {
            Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
    public static void Run()
    {
        // Add your tests here or to Tests.cs and run them here.
        MainSetup.Run();
    }
}

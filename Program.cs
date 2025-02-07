namespace slap;

class Program
{
    static void Main(string[] args)
    {
        // This is the preferred way of setting up logging.
        // Add anything you want to happen upon a log message being added here.
        // If you remove this, make sure to Sim.Log.PrintLogs() at the end if you want logs to appear.
        Sim.Log.OnMessage = (message) =>
        {
            message.Print(useColors: true);
        };

        try
        {
            Run();
        }
        catch (Exception ex)
        {
            Sim.Log.Error(ex.ToString());
        }
    }
    public static void Run()
    {
        // Add your tests here or to Tests.cs and run them here.
        Tests.MainTest();
    }
}

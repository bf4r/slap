namespace slap;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Run();
        }
        catch (Exception ex)
        {
            Sim.Log.Error(ex.Message);
        }
        Sim.Log.PrintLogs(useColors: true);
    }
    public static void Run()
    {
        // add your tests here or to Tests.cs and run them here
        Tests.HelloWorldThing();
        Tests.SocietyTest();
    }
}

namespace slap;

using slap.Things.Society.Devices.Printers;

public static class Test
{
    public static void All()
    {
        Printers();
    }
    public static void Printers()
    {
        Printer printer = new Printer("Xerox 123456 Printer", "A printer", PrinterVendor.Xerox, PrinterType.Laser);
        for (int i = 1; i <= 10; i++)
        {
            printer.AddPaper(new Paper("Paper", "A piece of paper", PaperFormat.A4, PaperColor.White));
            printer.EnqueueDocument(new PaperDocument("Document", $"A document with the number {i}", "Hello, World!", PaperFormat.A4));
        }
        for (int i = 1; i <= 10; i++)
        {
            printer.PrintNext();
        }
    }
}

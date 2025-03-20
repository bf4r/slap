namespace slap;

using slap.Things.Society.Devices.Printers;

public static class Test
{
    public static void Printers()
    {
        Printer printer = new Printer("Xerox 214423", "A printer", PrinterVendor.Xerox, PrinterType.Laser);
        printer.AddPaper(new Paper("Paper", "A piece of paper", PaperFormat.A4, PaperColor.White));
        printer.EnqueueDocument(new PaperDocument("Document", "A document", "Hello, World!", PaperFormat.A4));
        printer.PrintNext();
    }
}

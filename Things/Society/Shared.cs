namespace slap.Things;

using slap.Things.Society.Devices.Printers;

public class Shared
{
    public Printer Printer { get; set; }
    public List<Paper> PaperSupply { get; set; }
    public Shared()
    {
        Printer = new Printer("Shared printer", "The shared printer", PrinterVendor.Xerox, PrinterType.Laser);
        PaperSupply = [];
        for (int i = 0; i < 100; i++)
        {
            PaperSupply.Add(new Paper("Paper", "A piece of paper", PaperFormat.A4, PaperColor.White));
        }
    }
}

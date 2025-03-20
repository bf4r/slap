namespace slap.Things.Society.Devices.Printers;

public class Paper : Thing
{
    public PaperFormat Format { get; set; }
    public PaperColor Color { get; set; }
    public Paper(string name, string description, PaperFormat format, PaperColor color) : base(name, description)
    {
        Format = format;
        Color = color;
    }
}

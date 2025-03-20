namespace slap.Things.Society.Devices.Printers;

public class PaperDocument : Thing
{
    public string Text { get; set; }
    public PaperFormat Format { get; set; }
    public PaperDocument(string name, string description, string text, PaperFormat format) : base(name, description)
    {
        Text = text;
        Format = format;
    }
}

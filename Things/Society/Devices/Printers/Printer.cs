namespace slap.Things.Society.Devices.Printers;

public class Printer : Thing
{
    public Queue<PaperDocument> ToPrint { get; set; }
    public Stack<Paper> Papers { get; set; }
    public PrinterVendor Vendor { get; set; }
    public PrinterType Type { get; set; }
    public Printer(string name, string description, PrinterVendor vendor, PrinterType type) : base(name, description)
    {
        Vendor = vendor;
        Type = type;
        ToPrint = [];
        Papers = [];
    }
    public void AddPaper(Paper paper)
    {
        Papers.Push(paper);
    }
    public PaperDocument? PrintNext()
    {
        if (ToPrint.Count == 0) return null;
        var paper = Papers.Last();
        Sim.Log.Success($"{Description} printed a document on a {paper.Color} paper.");
        var document = ToPrint.Dequeue();
        return document;
    }
    public void EnqueueDocument(PaperDocument document)
    {
        if (!ToPrint.Contains(document))
        {
            ToPrint.Enqueue(document);
        }
    }
}

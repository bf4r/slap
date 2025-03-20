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
        if (ToPrint.Count == 0)
        {
            Sim.Log.Failure($"{this.Description} tried to print, but there are no documents in the queue.");
            return null;
        }
        if (Papers.Count == 0)
        {
            Sim.Log.Failure($"{this.Description} tried to print, but there are no papers in the feeder.");
            return null;
        }
        var paper = Papers.Last();
        var document = ToPrint.Dequeue();
        Sim.Log.Success($"{this.Description} printed {document.Description} on {paper.Description}.");
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

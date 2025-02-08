namespace slap.Things;

public class OwnedItem
{
    public Thing Thing { get; set; }
    public int Amount { get; set; }
    public OwnedItem(Thing thing, int amount)
    {
        Thing = thing;
        Amount = amount;
    }
}

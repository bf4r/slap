namespace slap.Things.Society.People;

public partial class Person : Thing
{
    // Like US dollars (for pricing purposes).
    public decimal Money { get; set; } = 0;
    // public decimal Bitcoin { get; set; }
    public List<OwnedItem> Inventory { get; set; } = new();
    public void CheckInventoryForZeroItems()
    {
        var itemsToRemove = Inventory.Where(x => x.Amount < 1);
        foreach (var item in itemsToRemove)
        {
            Inventory.Remove(item);
        }
    }
}

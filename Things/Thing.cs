namespace slap.Things;

// all objects in slap should inherit from this class
// (but not Location etc. because it's here)
// this allows for general actions and properties in all objects
public class Thing : IExistable
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Location? Location { get; set; }
    public Thing(string? name = null, string? description = null, Location? location = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Location = location;
        Sim.Stuff.Add(this);
    }
    public void Move(Location location)
    {
        Location = location;
    }
    public void Update()
    {
        // nothing here because Thing is a general concept for something that exists
    }
}

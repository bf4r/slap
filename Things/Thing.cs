namespace slap.Things;

// all objects in slap should inherit from this class
// this allows for general actions and properties in all objects
public class Thing
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Thing(string? name = null, string? description = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
    }
}

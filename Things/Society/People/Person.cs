namespace slap.Things.Society.People;

public partial class Person : Thing
{
    public Person() : base(nameof(Person), $"A {nameof(Person).ToLower()}.") { }
}

namespace slap.Things.Society.People;

using slap.Things.Society.People.Identity;
using slap.Things.Society.People.Relationships;

public partial class Person : Thing
{
    // Birth
    public DateTime? LastConception { get; set; }
    // A Mother's property representing the child in her womb.
    public Person? InWomb { get; set; }
    public bool IsPregnant => LastConception != null && InWomb != null;
    public Person? Father { get; set; }
    public Person? Mother { get; set; }
    public List<Person> Offsprings { get; set; } = new();
    public bool IsConceived => Conceived != null;
    public bool IsBorn => Born != null;
    public bool IsDead => Died != null;
    public List<Person> GetFamily()
    {
        // Only mother, father, brother, sister, and the person themselves.
        // Should maybe check if they're dead?
        List<Person> list = new();
        list.Add(this);
        if (Father != null) list.Add(Father);
        if (Mother != null)
        {
            list.Add(Mother);

            // Siblings:
            list.AddRange(Mother.Offsprings);
        }
        return list;
    }
    public bool IsRelatedTo(Person person)
    {
        return this.GetFamily().Contains(person);
    }
    public Person GiveBirth()
    {
        if (IsDead) throw new Exception("Cannot give birth because the mother is dead.");
        if (!IsPregnant) throw new Exception("This person is not carrying a baby.");
        if (!InWomb!.IsConceived) throw new Exception("The person needs to have been conceived in order to be born.");
        var person = InWomb;
        if (person.IsBorn) throw new Exception("The person has already been born.");

        person.Born = Sim.Now;

        if (person.Mother != null && person.Mother.Gender == Gender.Female) person.Mother.Offsprings.Add(person);

        var orientationPercentage = Sim.Random.Next(0, 100);
        person.SexualOrientation = orientationPercentage switch
        {
            < 80 => SexualOrientation.Heterosexual, // 80%
            < 83 => SexualOrientation.Homosexual, // 3%
            < 85 => SexualOrientation.Asexual, // 2%
            _ => SexualOrientation.Other, // 15%
        };
        person.ThingsSaid = new();
        this.InWomb = null;

        // todo: force assigning a first name to the baby
        return person;
    }
    // Conception
    public DateTime? Conceived { get; private set; }
    // Conceive as the Father.
    private void Conceive(Person mother)
    {
        var child = new Person();
        child.Conceived = Sim.Now;
        var genders = Enum.GetValues<Gender>().Cast<Gender>().ToList();
        var randomGender = genders[Sim.Random.Next(0, genders.Count)];
        child.Gender = randomGender;
        child.Mother = mother;
        child.Father = this;
        mother.LastConception = Sim.Now;
        mother.InWomb = child;
    }
    public bool MakeLove(Person person2, bool useProtection = false)
    {
        if (useProtection) return false;
        if (this.Gender == person2.Gender) return false;

        List<Person> people = [this, person2];
        var father = people.FirstOrDefault(x => x.Gender == Gender.Male);
        var mother = people.FirstOrDefault(x => x.Gender == Gender.Female);
        if (father == null || mother == null) return false;
        // todo: check if fertile
        father.Conceive(mother); // Conceive the child (not the mother)
        // The Child will be stored in mother.InWomb.
        // Child is then returned by GiveBirth().
        return true;
    }
    public DateTime? Born { get; private set; }

    // Age
    public TimeSpan? GetAge()
    {
        // If the person is dead, this returns the age at which they died.
        var comparedDate = IsDead ? Died : Sim.Now;
        if (!IsBorn) return new TimeSpan(0);
        TimeSpan? difference = comparedDate - Born;
        if (difference == null) throw new Exception("The person has not been born yet.");

        return difference;
    }
    public int GetAgeYears()
    {
        TimeSpan? ageTimeSpan = GetAge();
        if (ageTimeSpan == null || !IsBorn) throw new Exception("The person has not been born yet.");
        return (int)(Math.Floor(ageTimeSpan.Value.TotalDays / 365.0));
    }

    // Death
    public DateTime? Died { get; private set; }
    public string? CauseOfDeath { get; private set; }
    public Person? Killer { get; private set; }

    public void Die(string? causeOfDeath)
    {
        if (IsDead) throw new Exception("The person is already dead.");
        if (!IsConceived) throw new Exception("The person has not been conceived yet.");
        CauseOfDeath = causeOfDeath;
        Died = Sim.Now;
        Sim.Log.Info($"{this.Who()} has died from {CauseOfDeath}.");
        Killer = null;
    }

    public void Die()
    {
        if (IsDead) throw new Exception("The person is already dead.");
        if (!IsConceived) throw new Exception("The person has not been conceived yet.");
        CauseOfDeath = null;
        Killer = null;
        Died = Sim.Now;
        Sim.Log.Info($"{this.Who()} has died.");
    }

    public void Kill(Person person)
    {
        if (IsDead) throw new Exception("The person is already dead.");
        if (!IsConceived) throw new Exception("The person has not been conceived yet.");
        if (this.Location != null && person.Location != null &&
                this.Location.DistanceTo(person.Location) > 5)
        {
            throw new Exception("The person trying to kill is too far away from the victim.");
        }
        if (person.Id == this.Id) CauseOfDeath = "Suicide";
        person.CauseOfDeath = null;
        person.Killer = this;
        person.Died = Sim.Now;
        Sim.Log.Info($"{this.Who()} has killed {person.Who()}.");
    }
}

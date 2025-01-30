namespace slap.Things.Society;
using System.Text;

public class Person : Thing
{
    // Identity
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? GetFullName()
    {
        var sb = new StringBuilder();
        if (FirstName != null) sb.Append(FirstName);

        // only put a space if there's something to put a space between
        if (FirstName != null && LastName != null) sb.Append(' ');

        if (LastName != null) sb.Append(LastName);
        if (sb.Length == 0) return null;
        return sb.ToString();
    }

    // Gender
    public Gender Gender { get; set; }
    public string GetDetails()
    {
        string? fullName = GetFullName();
        int age = GetAgeYears();
        if (fullName != null)
        {
            return $"{fullName} ({(IsDead ? "†" : "")}{age}{Gender.GetOneLetterAbbreviation()})";
        }
        return $"Unnamed Person ({(IsDead ? "†" : "")}{age}{Gender.GetOneLetterAbbreviation()})";
    }

    // Conception
    public DateTime? Conceived { get; private set; }
    public void Conceive()
    {
        if (IsConceived) throw new Exception("The person has already been conceived.");
        if (IsBorn) throw new Exception("The person is already born.");
        Conceived = DateTime.Now;
    }

    // Birth
    public DateTime? Born { get; private set; }
    public void Birth()
    {
        Random random = new();
        if (!IsConceived) throw new Exception("The person needs to have been conceived in order to be born.");
        if (IsBorn) throw new Exception("The person has already been born.");

        Born = DateTime.Now;

        var genders = Enum.GetValues<Gender>().Cast<Gender>().ToList();
        var randomGender = genders[random.Next(0, genders.Count)];
        Gender = randomGender;
    }

    // Age
    public TimeSpan? GetAge()
    {
        // if the person is dead, returns the age at which they died
        var comparedDate = IsDead ? Died : DateTime.Now;
        if (!IsBorn) throw new Exception("The person has not been born yet.");
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
        Died = DateTime.Now;
        Killer = null;
    }

    public void Die()
    {
        if (IsDead) throw new Exception("The person is already dead.");
        if (!IsConceived) throw new Exception("The person has not been conceived yet.");
        CauseOfDeath = null;
        Killer = null;
        Died = DateTime.Now;
    }

    public void Kill(Person person)
    {
        if (IsDead) throw new Exception("The person is already dead.");
        if (!IsConceived) throw new Exception("The person has not been conceived yet.");
        if (person.Id == this.Id) CauseOfDeath = "Suicide";
        person.CauseOfDeath = null;
        person.Killer = this;
        person.Died = DateTime.Now;
    }

    public bool IsConceived => Conceived != null;
    public bool IsBorn => Born != null;
    public bool IsDead => Died != null;

    public Person() : base(nameof(Person), $"A {nameof(Person).ToLower()}.") { }

    public void GiveName(string? firstName = null, string? lastName = null)
    {
        FirstName = firstName;
        LastName = lastName;
    }
    public void ReassignGender(Gender gender)
    {
        Gender = gender;
    }
}

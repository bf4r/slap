namespace slap.Things.Society;
using System.Text;

public class Person : Thing
{
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

    public Gender Gender { get; set; }

    public DateTime? Conceived { get; private set; }
    public DateTime? Born { get; private set; }

    public bool IsConceived => Conceived != null;
    public bool IsBorn => Born != null;

    public Person() : base("Person", "A person.") { }

    public string GetDetails()
    {
        string? fullName = GetFullName();
        int age = GetAgeYears();
        if (fullName != null)
        {
            return $"{fullName} ({age}{Gender.GetOneLetterAbbreviation()})";
        }
        return $"Unnamed Person ({age}{Gender.GetOneLetterAbbreviation()})";
    }

    public void Conceive()
    {
        if (Conceived != null) throw new Exception("The person has already been conceived.");
        Conceived = DateTime.Now;
    }
    public void Birth()
    {
        Random random = new();
        if (Conceived == null) throw new Exception("The person needs to have been conceived in order to be born.");
        if (Born != null) throw new Exception("The person has already been born.");

        Born = DateTime.Now;

        var genders = Enum.GetValues<Gender>().Cast<Gender>().ToList();
        var randomGender = genders[random.Next(0, genders.Count)];
        Gender = randomGender;
    }
    public TimeSpan? GetAge()
    {
        if (!IsBorn) throw new Exception("The person has not been born yet.");
        TimeSpan? difference = DateTime.Now - Born;
        if (difference == null) throw new Exception("The person has not been born yet.");

        return difference;
    }
    public int GetAgeYears()
    {
        TimeSpan? ageTimeSpan = GetAge();
        if (ageTimeSpan == null || !IsBorn) throw new Exception("The person has not been born yet.");
        return (int)(Math.Floor(ageTimeSpan.Value.TotalDays / 365.0));
    }
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

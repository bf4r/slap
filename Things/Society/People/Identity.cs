namespace slap.Things.Society.People;

using slap.Things.Society.People.Identity;
using System.Text;

public partial class Person : Thing
{
    // Identity
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    // Nickname, online gamertag, ..., can be changed depending on the situation.
    public string? PreferredName { get; set; }
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
        if (fullName == null) fullName = "Unnamed Person";
        return $"{fullName} ({(IsDead ? "†" : "")}{age}{Gender.GetOneLetterAbbreviation()})";
    }

    public string GetPronoun(PronounType type)
    {
        return (type, Gender) switch
        {
            (PronounType.Subject, Gender.Male) => "he",
            (PronounType.Object, Gender.Male) => "him",
            (PronounType.Possessive, Gender.Male) => "his",
            (PronounType.PossessiveDeterminer, Gender.Male) => "his",
            (PronounType.Reflexive, Gender.Male) => "himself",

            (PronounType.Subject, Gender.Female) => "she",
            (PronounType.Object, Gender.Female) => "her",
            (PronounType.Possessive, Gender.Female) => "hers",
            (PronounType.PossessiveDeterminer, Gender.Female) => "her",
            (PronounType.Reflexive, Gender.Female) => "herself",

            _ => type switch
            {
                PronounType.Subject => "they",
                PronounType.Object => "them",
                PronounType.Possessive => "theirs",
                PronounType.PossessiveDeterminer => "their",
                PronounType.Reflexive => "themself",
                _ => "they"
            }
        };
    }
    public void AssignGender(Gender gender)
    {
        Gender = gender;
    }
    public void GiveName(string? firstName = null, string? lastName = null)
    {
        FirstName = firstName;
        LastName = lastName;
        PreferredName = firstName;
    }
    public void NameChild(Person person, string firstName)
    {
        person.FirstName = firstName;
        person.LastName = this.LastName;
        person.PreferredName = person.FirstName;
    }
}

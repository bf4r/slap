namespace slap.Things.Society;

using slap.Things.Society.Relationships;
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
        if (fullName == null) fullName = "Unnamed Person";
        return $"{fullName} ({(IsDead ? "†" : "")}{age}{Gender.GetOneLetterAbbreviation()})";
    }

    // Conception
    public DateTime? Conceived { get; private set; }
    public void Conceive()
    {
        if (IsConceived) throw new Exception("The person has already been conceived.");
        if (IsBorn) throw new Exception("The person is already born.");
        Conceived = Simulation.Now;
    }

    // Birth
    public DateTime? Born { get; private set; }
    public void Birth()
    {
        if (!IsConceived) throw new Exception("The person needs to have been conceived in order to be born.");
        if (IsBorn) throw new Exception("The person has already been born.");

        Born = Simulation.Now;

        var genders = Enum.GetValues<Gender>().Cast<Gender>().ToList();
        var randomGender = genders[Simulation.Random.Next(0, genders.Count)];
        Gender = randomGender;

        var orientationPercentage = Simulation.Random.Next(0, 100);
        SexualOrientation = orientationPercentage switch
        {
            < 80 => SexualOrientation.Heterosexual, // 80%
            < 83 => SexualOrientation.Homosexual, // 3%
            < 85 => SexualOrientation.Asexual, // 2%
            _ => SexualOrientation.Other, // 15%
        };
    }

    // Age
    public TimeSpan? GetAge()
    {
        // if the person is dead, returns the age at which they died
        var comparedDate = IsDead ? Died : Simulation.Now;
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
        Died = Simulation.Now;
        Killer = null;
    }

    public void Die()
    {
        if (IsDead) throw new Exception("The person is already dead.");
        if (!IsConceived) throw new Exception("The person has not been conceived yet.");
        CauseOfDeath = null;
        Killer = null;
        Died = Simulation.Now;
    }

    public void Kill(Person person)
    {
        if (IsDead) throw new Exception("The person is already dead.");
        if (!IsConceived) throw new Exception("The person has not been conceived yet.");
        if (this.Location != null && person.Location != null &&
                this.Location.DistanceToInMeters(person.Location) > 100)
        {
            throw new Exception("The person trying to kill is too far away from the victim.");
        }
        if (person.Id == this.Id) CauseOfDeath = "Suicide";
        person.CauseOfDeath = null;
        person.Killer = this;
        person.Died = Simulation.Now;
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
    // Relationships
    public RelationshipStatus RelationshipStatus { get; set; } = RelationshipStatus.Single;
    public Person? Partner { get; set; }
    public Person? Fiance => this.RelationshipStatus == RelationshipStatus.Engaged ? Partner : null;
    public Person? Spouse => this.RelationshipStatus == RelationshipStatus.Married ? Partner : null;
    public Person? Husband => this.RelationshipStatus == RelationshipStatus.Married && Partner != null && Partner.Gender == Gender.Male ? Partner : null;
    public Person? Wife => this.RelationshipStatus == RelationshipStatus.Married && Partner != null && Partner.Gender == Gender.Female ? Partner : null;
    public bool IsInRelationshipWith(Person person)
    {
        return Partner == person && RelationshipStatus != RelationshipStatus.Single;
    }

    public SexualOrientation SexualOrientation { get; set; }
    public AskOutOutcome AskOut(Person person)
    {
        return person.GetAskedOut(this);
    }
    public AskOutOutcome GetAskedOut(Person person)
    {
        // sexuality compatibility
        bool isCurious = Simulation.Random.Next(0, 10) == 0; // 10% chance to ignore their sexual orientation
        bool sexualityMatches = false;
        var s1 = this.SexualOrientation;
        var s2 = person.SexualOrientation;
        var gay = SexualOrientation.Homosexual;
        var straight = SexualOrientation.Heterosexual;
        var bi = SexualOrientation.Other;
        if (!isCurious)
        {
            if (person.Gender != this.Gender)
            {
                // different-sex (traditional) couple
                sexualityMatches = (s1 == straight || s1 == bi);
            }
            else
            {
                // same-sex (gay) couple
                sexualityMatches = (s1 == gay || s1 == bi);
            }
            // ignore sexuality of s2 since they asked s1 out in the first place
        }

        // person is exploring their sexuality and it's automatically compatible
        if (isCurious) sexualityMatches = true;

        // age compatibility
        bool ageMatches = false;
        int ageAsking = person.GetAgeYears();
        int ageAsked = this.GetAgeYears();
        if (ageAsked < 12 || ageAsking < 12) ageMatches = false;
        // 13-15
        else if (ageAsking <= 15 && ageAsked <= 15 && ageAsked >= 13 && ageAsking >= 13) ageMatches = true;
        // 16-18
        else if (ageAsked >= 16 && ageAsked < 18 && ageAsking >= 16 && ageAsking < 18) ageMatches = true;
        else if (ageAsked > 18 && ageAsking > 18) ageMatches = true;
        else ageMatches = false;

        // 70% to accept if all conditions are met
        var chancePicker = Simulation.Random.Next(0, 100);
        bool accept = chancePicker <= 70;

        // rejections
        if (!ageMatches) return AskOutOutcome.RejectedIncompatibleAge;
        if (!sexualityMatches) return AskOutOutcome.RejectedIncompatibleSexuality;
        if (!accept) return AskOutOutcome.RejectedPreference;

        // circular reference
        // person.Partner.Partner.Partner.Partner.Partner :)
        this.RelationshipStatus = RelationshipStatus.Dating;
        this.Partner = person;
        person.RelationshipStatus = RelationshipStatus.Dating;
        person.Partner = this;
        return AskOutOutcome.Accepted;
    }
}

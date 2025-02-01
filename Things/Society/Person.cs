namespace slap.Things.Society;

using slap.Logging;
using slap.Things.Society.Relationships;
using System.Text;

public class Person : Thing
{
    private int _health;
    private int _fullness;
    private int _hydration;
    private int _energy;

    public int Health
    {
        get => _health;
        set => _health = Math.Clamp(value, 0, 100);
    }
    public int Fullness
    {
        get => _fullness;
        set => _fullness = Math.Clamp(value, 0, 100);
    }
    public int Hydration
    {
        get => _hydration;
        set => _hydration = Math.Clamp(value, 0, 100);
    }
    public int Energy
    {
        get => _energy;
        set => _energy = Math.Clamp(value, 0, 100);
    }

    // below are inverse properties for convenience in language:
    // 
    // Example:
    // If the person is hungry, Fullness will be 30 an Hunger will be 70
    // Hunger is not a hunger bar, it's a measure of how hungry the person is from 0 to 100
    // with 0 being the lowest and 100 being the most hungry
    // If hunger reaches 100, the person dies out of starvation
    public int Sickness
    {
        get => 100 - Health;
        set => Health = 100 - value;
    }
    public int Hunger
    {
        get => 100 - Fullness;
        set => Fullness = 100 - value;
    }
    public int Thirst
    {
        get => 100 - Hydration;
        set => Hydration = 100 - value;
    }
    public int Exhaustion
    {
        get => 100 - Energy;
        set => Energy = 100 - value;
    }

    public void CheckHealth()
    {
        if (Fullness == 0) Die("Starvation");
        if (Hydration == 0) Die("Dehydration");

        // once Sleep() is added
        // if (Energy == 0) Sleep("Exhaustion");
    }

    public static void CheckHealth(List<Person> people)
    {
        people.ForEach(p => p.CheckHealth());
    }

    // Identity
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    // nickname, online gamertag, ..., can be changed depending on the situation
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


    // Conception
    public DateTime? Conceived { get; private set; }
    // conceive as Father
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

    // Birth
    public DateTime? LastConception { get; set; }
    public Person? InWomb { get; set; }
    public bool IsPregnant => LastConception != null && InWomb != null;
    public Person? Father { get; set; }
    public Person? Mother { get; set; }
    public List<Person> Offsprings { get; set; } = new();
    public List<Person> GetFamily()
    {
        // only mom, dad, bro, sis, and the person themselves
        // should maybe check if they're dead?
        List<Person> list = new();
        list.Add(this);
        if (Father != null) list.Add(Father);
        if (Mother != null)
        {
            list.Add(Mother);
            // siblings
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
    public void NameChild(Person person, string firstName)
    {
        person.FirstName = firstName;
        person.LastName = this.LastName;
        person.PreferredName = person.FirstName;
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
        father.Conceive(mother); // conceive the child (not the mother, lol)
        // child stored in mother.InWomb
        // returned by GiveBirth()
        return true;
    }
    public static Person GetEve()
    {
        return new Person()
        {
            Conceived = Sim.Now - TimeSpan.FromDays(20 * 365),
            Born = Sim.Now - TimeSpan.FromDays(19 * 365),
            Gender = Gender.Female,
            FirstName = "Eve",
        };
    }
    public static Person GetAdam()
    {
        return new Person()
        {
            Conceived = Sim.Now - TimeSpan.FromDays(20 * 365),
            Born = Sim.Now - TimeSpan.FromDays(19 * 365),
            Gender = Gender.Male,
            FirstName = "Adam"
        };
    }
    public DateTime? Born { get; private set; }

    // Age
    public TimeSpan? GetAge()
    {
        // if the person is dead, returns the age at which they died
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
        Killer = null;
    }

    public void Die()
    {
        if (IsDead) throw new Exception("The person is already dead.");
        if (!IsConceived) throw new Exception("The person has not been conceived yet.");
        CauseOfDeath = null;
        Killer = null;
        Died = Sim.Now;
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
        person.Died = Sim.Now;
    }

    public bool IsConceived => Conceived != null;
    public bool IsBorn => Born != null;
    public bool IsDead => Died != null;

    public Person() : base(nameof(Person), $"A {nameof(Person).ToLower()}.") { }

    public void GiveName(string? firstName = null, string? lastName = null)
    {
        FirstName = firstName;
        LastName = lastName;
        PreferredName = firstName;
    }
    public void AssignGender(Gender gender)
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
    public bool IsDivorced { get; set; } = false;
    public bool IsDating(Person person2)
    {
        return Partner == person2 && RelationshipStatus == RelationshipStatus.Dating && person2.RelationshipStatus == RelationshipStatus.Dating;
    }
    public bool IsMarriedTo(Person person2)
    {
        return Spouse == person2;
    }
    public bool IsEngagedTo(Person person2)
    {
        return Fiance == person2;
    }
    public bool IsEngaged => Fiance != null;
    public bool IsMarried => Spouse != null;
    public bool IsSingle => RelationshipStatus == RelationshipStatus.Single && Partner == null;
    public bool IsInRelationshipWith(Person person)
    {
        return Partner == person && RelationshipStatus != RelationshipStatus.Single && person.RelationshipStatus != RelationshipStatus.Single;
    }

    public SexualOrientation SexualOrientation { get; set; }
    public AskOutOutcome AskOut(Person person)
    {
        return person.GetAskedOut(this);
    }
    public AskOutOutcome GetAskedOut(Person asker)
    {
        if (IsDead) throw new Exception("Dating a corpse is not allowed.");
        // sexuality compatibility
        bool isCurious = Sim.Random.Next(0, 10) == 0; // 10% chance to ignore their sexual orientation
        bool sexualityMatches = false;
        var s1 = this.SexualOrientation;
        var s2 = asker.SexualOrientation;
        var gay = SexualOrientation.Homosexual;
        var straight = SexualOrientation.Heterosexual;
        var bi = SexualOrientation.Other;
        if (!isCurious)
        {
            if (asker.Gender != this.Gender)
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
        int ageAsking = asker.GetAgeYears();
        int ageAsked = this.GetAgeYears();
        if (ageAsked < 12 || ageAsking < 12) ageMatches = false;
        // 13-15
        else if (ageAsking <= 15 && ageAsked <= 15 && ageAsked >= 13 && ageAsking >= 13) ageMatches = true;
        // 16-18
        else if (ageAsked >= 16 && ageAsked < 18 && ageAsking >= 16 && ageAsking < 18) ageMatches = true;
        else if (ageAsked > 18 && ageAsking > 18) ageMatches = true;
        else ageMatches = false;

        var chancePicker = Sim.Random.Next(0, 100);
        var rejectionChance = this.RelationshipStatus switch
        {
            RelationshipStatus.Single => 30,

            RelationshipStatus.Dating => 85,
            RelationshipStatus.Engaged => 98,
            RelationshipStatus.Married => 90,
            _ => 30
        };
        bool accept = chancePicker <= 100 - rejectionChance;

        // rejections
        if (!ageMatches) return AskOutOutcome.RejectedIncompatibleAge;
        if (!sexualityMatches) return AskOutOutcome.RejectedIncompatibleSexuality;
        if (!accept) return AskOutOutcome.RejectedPreference;

        // circular reference
        // person.Partner.Partner.Partner.Partner.Partner :)
        this.RelationshipStatus = RelationshipStatus.Dating;
        this.Partner = asker;
        asker.RelationshipStatus = RelationshipStatus.Dating;
        asker.Partner = this;
        return AskOutOutcome.Accepted;
    }
    public bool Propose(Person person)
    {
        return person.GetProposedTo(this);
    }
    public bool GetProposedTo(Person asker)
    {
        if (IsDead) throw new Exception("A dead person cannot accept a marriage proposal.");
        if (this.RelationshipStatus == RelationshipStatus.Dating && asker.RelationshipStatus == RelationshipStatus.Dating && this.IsInRelationshipWith(asker))
        {
            int choicePercentage = Sim.Random.Next(0, 100);
            if (choicePercentage < 10)
            {
                // rejection
                return false;
            }
            this.RelationshipStatus = RelationshipStatus.Engaged;
            asker.RelationshipStatus = RelationshipStatus.Engaged;
            return true;
        }

        // not dating each other
        return false;
    }
    public bool Marry(Person person2)
    {
        if (this.RelationshipStatus == RelationshipStatus.Engaged && person2.RelationshipStatus == RelationshipStatus.Engaged && this.IsInRelationshipWith(person2))
        {
            List<Person> people = [this, person2];

            // in a traditional marriage, wife takes husband's last name
            var wife = people.FirstOrDefault(x => x.Gender == Gender.Female);
            var husband = people.FirstOrDefault(x => x.Gender == Gender.Male);
            if (wife != null && husband != null)
            {
                wife.LastName = husband.LastName;
            }

            this.RelationshipStatus = RelationshipStatus.Married;
            this.IsDivorced = false;
            person2.RelationshipStatus = RelationshipStatus.Married;
            person2.IsDivorced = false;
            return true;
        }
        return false;
    }
    public void BreakUp(Person person)
    {
        // todo: handle who gets the kids
        if (this.Partner == person)
        {
            this.RelationshipStatus = RelationshipStatus.Single;
            person.RelationshipStatus = RelationshipStatus.Single;
            this.Partner = null;
            person.Partner = null;
            return;
        }
        throw new Exception("The couple must be dating or engaged with each other in order to break up.");
    }
    public void Divorce(Person person)
    {
        // todo: handle who gets the kids
        if (this.RelationshipStatus == RelationshipStatus.Married || this.RelationshipStatus == RelationshipStatus.Married && person.RelationshipStatus == this.RelationshipStatus)
        {
            this.RelationshipStatus = RelationshipStatus.Single;
            this.Partner = null;
            this.IsDivorced = true;

            person.RelationshipStatus = RelationshipStatus.Single;
            person.Partner = null;
            person.IsDivorced = true;
            return;
        }
        throw new Exception("The couple must be married to each other in order to divorce.");
    }
    public List<LogMessage>? ThingsSaid { get; set; }
    public void Say(string message)
    {
        if (IsDead) throw new Exception("The person is dead and therefore cannot speak.");
        if (ThingsSaid != null)
        {
            if (GetAgeYears() < 1) message = "Goo goo ga ga!";
            ThingsSaid.Add(new LogMessage(LogLevel.Dialogue, $"{PreferredName}: \"{message}\""));
            Sim.Log.Dialogue($"{PreferredName}: \"{message}\"");
        }
    }
    public void LogAllThingsSaid()
    {
        if (ThingsSaid != null)
        {
            Console.WriteLine(ThingsSaid.Count);
            foreach (var message in ThingsSaid)
            {
                message.Log();
            }
        }
    }
}

namespace slap.Things.Society.People;
using slap.Things.Society.People.Relationships;
using slap.Things.Society.People.Identity;

public partial class Person : Thing
{
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
        // Sexuality compatibility:
        bool isCurious = Sim.Random.Next(0, 10) == 0; // 10% chance to ignore their sexual orientation.
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
                // Different-sex (traditional) couple:
                sexualityMatches = (s1 == straight || s1 == bi);
            }
            else
            {
                // Same-sex (gay) couple:
                sexualityMatches = (s1 == gay || s1 == bi);
            }
            // Ignore sexuality of s2 since they asked s1 out in the first place.
        }

        // Person is exploring their sexuality, so it's automatically compatible.
        if (isCurious) sexualityMatches = true;

        // Age compatibility:
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

        // Rejections:
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
                // Rejection.
                return false;
            }
            this.RelationshipStatus = RelationshipStatus.Engaged;
            asker.RelationshipStatus = RelationshipStatus.Engaged;
            return true;
        }

        // Not dating each other.
        return false;
    }
    public bool Marry(Person person2)
    {
        if (this.RelationshipStatus == RelationshipStatus.Engaged && person2.RelationshipStatus == RelationshipStatus.Engaged && this.IsInRelationshipWith(person2))
        {
            List<Person> people = [this, person2];

            // In a traditional marriage, the wife takes the husband's last name.
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
}

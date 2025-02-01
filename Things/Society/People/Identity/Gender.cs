namespace slap.Things.Society.People.Identity;

// only 2 genders to simplify things
// not saying there are only 2 genders
// but for the purpose of this program there just are
public enum Gender
{
    Male,
    Female
}

public static class GenderExtensions
{
    public static string GetOneLetterAbbreviation(this Gender gender)
    {
        return gender switch
        {
            Gender.Male => "M",
            Gender.Female => "F",
            _ => ""
        };
    }
}

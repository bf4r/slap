namespace slap.Things.Society.People;

using slap.Things.Society.People.Identity;
using slap.Things.Society.Devices.Printers;
using System.Text;

public partial class Person : Thing
{
    public void Eat(Food food)
    {
        if (!IsConscious) return;
        if (Fullness == 100)
        {
            Sim.Log.Failure($"{this.Who()} tried to eat {food.Description}, but couldn't, because {this.GetPronoun(PronounType.Subject)} is too full.");
            return;
        }
        Do(() =>
        {
            Fullness += food.Nutrition;
            Thirst += food.Dryness;
            Sim.Log.Success($"{this.Who()} ate {food.Description} and it was {food.Nutrition}% nutritious ({this.GetPronoun(PronounType.Subject)} is now {this.Hunger}% hungry) and made {this.GetPronoun(PronounType.Object)} {food.Dryness}% more thirsty (now {this.Thirst}%).");
        }, TimeSpan.FromMinutes(5));
    }
    public void Drink(Beverage beverage)
    {
        if (!IsConscious) return;
        if (Hydration == 100)
        {
            Sim.Log.Failure($"{this.Who()} tried to drink {beverage.Description}, but couldn't, because {this.GetPronoun(PronounType.Subject)} is too hydrated.");
            return;
        }
        Do(() =>
        {
            Hydration += beverage.Hydration;
            Sim.Log.Success($"{this.Who()} drank {beverage.Description} and it quenched {this.GetPronoun(PronounType.PossessiveDeterminer)} thirst by {beverage.Hydration}% (now {this.Thirst}%).");
        }, TimeSpan.FromSeconds(10));
    }
    public void Run(int meters)
    {
        if (!IsConscious) return;
        if (meters <= 0)
        {
            Sim.Log.Failure($"{this.Who()} tried to run a negative or zero distance.");
            return;
        }
        // How many meters to run to lower energy by 1%. The person can run 25 km at once max.
        var onePercentMeters = 250;
        double kmph = 15.0;
        TimeSpan duration = TimeSpan.FromHours(meters / 1000.0 / kmph);
        var takeEnergyPercent = meters / onePercentMeters;
        if (this.Energy < takeEnergyPercent)
        {
            Sim.Log.Failure($"{this.Who()} tried to run {Math.Round(meters / 1000.0, 1)} km, but couldn't, because {this.GetPronoun(PronounType.Subject)} is too tired for that length.");
            return;
        }
        Do(() =>
        {
            this.Energy -= takeEnergyPercent;
            Sim.Log.Success($"{this.Who()} ran {Math.Round(meters / 1000.0, 1)} km.");
        }, duration);
    }
    public void GiveMoney(Person recipient, decimal amount)
    {
        if (!IsConscious) return;
        if (this.Money < amount)
        {
            Sim.Log.Failure($"{this.Who()} tried to pay {recipient.Who()} ${amount}, but only has {this.Money}.");
        }
        this.Money -= amount;
        recipient.Money += amount;
        Sim.Log.Success($"{this.Who()} gave ${amount} to {recipient.Who()}.");
    }
    private DateTime _lastMoved = Sim.Now - TimeSpan.FromMilliseconds(Sim.Random.Next(0, 1000));
    public void Move()
    {
        if (Sim.Now - _lastMoved < TimeSpan.FromSeconds(1)) return;
        if (this.Location == null)
        {
            var initialRandX = Sim.Random.Next(-60, 61);
            var initialRandY = Sim.Random.Next(-30, 31);
            this.Location = new(null, null, initialRandX, initialRandY);
        }
        if (!IsConscious) return;
        var randX = Sim.Random.Next(-1, 2);
        var randY = Sim.Random.Next(-1, 2);
        this.Location.X += randX;
        this.Location.Y += randY;
        _lastMoved = Sim.Now;
    }
    public bool IsNearby(Person other, int proximity = 5)
    {
        if (this.Location == null || other.Location == null) return false;
        return Math.Abs(this.Location.X - other.Location.X) <= proximity &&
               Math.Abs(this.Location.Y - other.Location.Y) <= proximity;
    }

    public void Chat(Person other)
    {
        if (!IsConscious || !other.IsConscious) return;
        Do(() =>
        {
            Energy -= 2;
            other.Energy -= 2;
            Sim.Log.Success($"{this.Who()} had a conversation with {other.Who()}.");
        }, TimeSpan.FromMinutes(15));
    }

    public void Rest()
    {
        if (!IsConscious) return;
        Do(() =>
        {
            Energy += 10;
            Health += 5;
            Sim.Log.Success($"{this.Who()} rested for a while and recovered some energy and health.");
        }, TimeSpan.FromMinutes(30));
    }

    public void MorningRoutine()
    {
        if (!IsConscious) return;
        Do(() =>
        {
            Energy -= 5;
            Health += 2;
            Sim.Log.Success($"{this.Who()} completed their morning routine.");
        }, TimeSpan.FromMinutes(45));
    }

    public void PrintDocument()
    {
        if (!IsConscious) return;
        Do(() =>
        {
            Energy -= 1;
            var sb = new StringBuilder();
            for (int i = 0; i < 100; i++)
            {
                sb.Append(' ' + Utils.GetRandomWord());
            }
            var text = sb.ToString().TrimStart();
            var document = new PaperDocument("Document", "A paper document", text, PaperFormat.A4);
            Sim.Shared.Printer.EnqueueDocument(document);
            Sim.Shared.Printer.PrintNext();
        });
    }
}

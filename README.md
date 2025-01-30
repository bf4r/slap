# slap
Simulation framework

## About
Slap stands for "Simulation app" and is a C# object-oriented framework for simulating a real-time ecosystem using classes, methods and properties and how they interact with each other.
The goal is to represent the world around us in a way you can define with custom sets of objects and provide an API for a fictional world where every entity has the properties it needs to simulate a system.

For example, you could define a `Person` object, but to keep things as close to the real world as possible, you first need to initialize that person's life by making the person get conceived using `person.Conceive()` and born using `mother.Birth(person)`. Then, you can skip ahead to their adulthood by doing `Simulation.TimeTravel(TimeSpan.FromDays(18 * 365));`. The Program.cs file provides a basic example that tests features of the program. You can delete everything in Program.cs and start from a blank slate to create your own world.
Just add this:
```
using slap;
```
You probably also want `slap.Things`, `slap.Things.Society`, etc.

**The project is a work in progress, so these examples might already be heavily outdated. Check the source code for actual guidance because most objects (or "things") are public and accessible.**

## Requirements
[.NET 8.0 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

## Installation & Usage
To use Slap in its early development stage, you can clone the repository, navigate to the directory with Program.cs and type:

```
dotnet run
```

This will run the code in Program.cs based on the current testing program. On the master branch, this program is mostly a showcase of what Slap has to offer.

I encourage you to edit Program.cs and look through the other source files, especially in `Things/`, where you will find all the objects in `slap.Things` which are the main components of the simulation. Program.cs is already filled with examples on how to use them.

## Coming soon
- I have lots of features and new objects planned, so please stay tuned!
- I can't write them here because the todo list is constantly changing.

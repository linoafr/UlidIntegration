namespace Linoa.UlidIntegration.EntityFrameworkCore.Relational.Tests.Models;

public class Person
{
    public Person(string name) => Name = name;

    public Ulid Id { get; private set; } = Ulid.NewUlid();
    public string Name { get; set; }
}

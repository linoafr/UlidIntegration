using Microsoft.EntityFrameworkCore;

namespace Linoa.UlidIntegration.EntityFrameworkCore.Relational.Tests.Models;

public class UlidContext : DbContext
{
    public UlidContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Person> Persons => Set<Person>();
}

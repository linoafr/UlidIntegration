using Linoa.UlidIntegration.EntityFrameworkCore.Relational.Tests.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Xunit;

namespace Linoa.UlidIntegration.EntityFrameworkCore.Relational.Tests;

public interface IUlidDatabaseTestFixture
{
    UlidContext DbContext { get; }
    DbContextOptions<UlidContext> DbContextOptions { get; }
}

public abstract class UlidTests
{
    public UlidTests(IUlidDatabaseTestFixture fixture)
    {
        Db = fixture.DbContext;
        DbContextOptions = fixture.DbContextOptions;
    }

    protected UlidContext Db { get; }
    protected DbContextOptions<UlidContext> DbContextOptions { get; }

    [Fact]
    public async Task Store_entity_with_ulid_and_retrieve_it()
    {
        using var transaction = await Db.Database.BeginTransactionAsync();
        var person = new Person("Test1");
        Db.Add(person);
        await Db.SaveChangesAsync();

        var newContext = new UlidContext(DbContextOptions);
        newContext.Database.UseTransaction(transaction.GetDbTransaction());
        var personDb = await newContext.Persons.SingleOrDefaultAsync(p => p.Id == person.Id);

        Assert.NotNull(personDb);
        Assert.Equal(person.Id, personDb!.Id);
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task Query_with_ulid_new_method()
    {
        var personDb = await Db.Persons.SingleOrDefaultAsync(p => p.Id == Ulid.NewUlid());
        Assert.Null(personDb);
    }

    [Fact]
    public async Task Query_with_ulid_minvalue()
    {
        var personDb = await Db.Persons.SingleOrDefaultAsync(p => p.Id == Ulid.MinValue);
        Assert.Null(personDb);
    }

    [Fact]
    public async Task Query_with_ulid_maxvalue()
    {
        var personDb = await Db.Persons.SingleOrDefaultAsync(p => p.Id == Ulid.MaxValue);
        Assert.Null(personDb);
    }
}

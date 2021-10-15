using Linoa.UlidIntegration.EntityFrameworkCore.Relational.Tests.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

using Xunit;

namespace Linoa.UlidIntegration.EntityFrameworkCore.Relational.Tests;

public class NpgsqlUlidTestFixture : IUlidDatabaseTestFixture
{
    public NpgsqlUlidTestFixture()
    {
        DbContextOptions = new DbContextOptionsBuilder<UlidContext>()
            .UseNpgsql(new NpgsqlConnection("Server=localhost;Port=15432;Database=UlidTests;User Id=sa;Password=Pwd12345!;"), x => x.UseUlidToString())
            .Options;

        DbContext = new UlidContext(DbContextOptions);

        DbContext.Database.EnsureDeleted();
        DbContext.Database.EnsureCreated();
    }

    public UlidContext DbContext { get; }
    public DbContextOptions<UlidContext> DbContextOptions { get; }
}

[CollectionDefinition(nameof(NpgsqlUlidTestCollection))]
public class NpgsqlUlidTestCollection : ICollectionFixture<NpgsqlUlidTestFixture>
{
}

[Collection(nameof(NpgsqlUlidTestCollection))]
public class NpgsqlUlidTests : UlidTests
{
    public NpgsqlUlidTests(NpgsqlUlidTestFixture fixture) : base(fixture)
    {
    }
}

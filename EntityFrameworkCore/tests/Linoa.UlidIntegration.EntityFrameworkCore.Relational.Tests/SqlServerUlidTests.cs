using Linoa.UlidIntegration.EntityFrameworkCore.Relational.Tests.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Linoa.UlidIntegration.EntityFrameworkCore.Relational.Tests;

public class SqlServerDatabaseTestFixture : IUlidDatabaseTestFixture
{
    public SqlServerDatabaseTestFixture()
    {
        DbContextOptions = new DbContextOptionsBuilder<UlidContext>()
            .UseSqlServer(new SqlConnection(@"Server=localhost,11433;Database=UlidTests;User Id=sa;Password=Pwd12345!;Persist Security Info=true"), x => x.UseUlidToString())
            .Options;

        DbContext = new UlidContext(DbContextOptions);
        DbContext.Database.EnsureDeleted();
        DbContext.Database.EnsureCreated();
    }

    public UlidContext DbContext { get; }
    public DbContextOptions<UlidContext> DbContextOptions { get; }
}

[CollectionDefinition(nameof(SqlServerDatabaseTestCollection))]
public class SqlServerDatabaseTestCollection : ICollectionFixture<SqlServerDatabaseTestFixture>
{
}

[Collection(nameof(SqlServerDatabaseTestCollection))]
public class SqlServerUlidTests : UlidTests
{
    public SqlServerUlidTests(SqlServerDatabaseTestFixture fixture) : base(fixture)
    {
    }
}

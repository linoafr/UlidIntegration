using Linoa.UlidIntegration.EntityFrameworkCore.Relational.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Linoa.UlidIntegration.EntityFrameworkCore.Relational.Infrastructure;

internal class RelationalUlidOptionsExtension : IDbContextOptionsExtension
{
    private DbContextOptionsExtensionInfo? _info;
    public UlidStorageMode StorageMode { get; }

    public RelationalUlidOptionsExtension(UlidStorageMode storageMode) => StorageMode = storageMode;

    public virtual void ApplyServices(IServiceCollection services)
    => services.AddEntityFrameworkCoreRelationalUlid(StorageMode);

    public DbContextOptionsExtensionInfo Info
        => _info ??= new ExtensionInfo(this, StorageMode);

    public virtual void Validate(IDbContextOptions options)
    {
        var internalServiceProvider = options.FindExtension<CoreOptionsExtension>()?.InternalServiceProvider;
        if (internalServiceProvider is not null)
        {
            using (var scope = internalServiceProvider.CreateScope())
            {
                if (scope.ServiceProvider.GetService<IEnumerable<IRelationalTypeMappingSourcePlugin>>()
                            ?.Any(s => s is UlidToStringTypeMappingSourcePlugin) != true)
                {
                    throw new InvalidOperationException($"{nameof(RelationalUlidDbContextOptionsBuilderExtensions.UseUlid)} requires {nameof(RelationalUlidServiceCollectionExtensions.AddEntityFrameworkCoreRelationalUlid)} to be called on the internal service provider used.");
                }
            }
        }
    }

    private sealed class ExtensionInfo : DbContextOptionsExtensionInfo
    {
        private readonly UlidStorageMode _storageMode;

        public ExtensionInfo(IDbContextOptionsExtension extension, UlidStorageMode storageMode)
             : base(extension) => _storageMode = storageMode;

        public override bool IsDatabaseProvider => false;

        public override int GetServiceProviderHashCode() => 0;

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
            => true;

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
            => debugInfo["Ulid:" + nameof(RelationalUlidDbContextOptionsBuilderExtensions.UseUlid)] = "1";

        public override string LogFragment => $"using Ulid stored as {_storageMode} ";
    }
}


using Linoa.UlidIntegration.EntityFrameworkCore.Relational.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

internal static class RelationalUlidServiceCollectionExtensions
{
    internal static IServiceCollection AddEntityFrameworkCoreRelationalUlid(this IServiceCollection serviceCollection, UlidStorageMode storageMode)
    {
        if (serviceCollection is null)
        {
            throw new ArgumentNullException(nameof(serviceCollection));
        }

        if (storageMode == UlidStorageMode.String)
        {
            new EntityFrameworkRelationalServicesBuilder(serviceCollection)
                .TryAdd<IRelationalTypeMappingSourcePlugin, UlidToStringTypeMappingSourcePlugin>();
        }
        else if (storageMode == UlidStorageMode.Bytes)
        {
            new EntityFrameworkRelationalServicesBuilder(serviceCollection)
                .TryAdd<IRelationalTypeMappingSourcePlugin, UlidToBytesTypeMappingSourcePlugin>();
        }

        return serviceCollection;
    }
}

using Linoa.UlidIntegration.EntityFrameworkCore.Relational.Infrastructure;
using Linoa.UlidIntegration.EntityFrameworkCore.Relational.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore;

public static class RelationalUlidDbContextOptionsBuilderExtensions
{
    public static RelationalDbContextOptionsBuilder<TBuilder, TExtension> UseUlidToString<TBuilder, TExtension>(this RelationalDbContextOptionsBuilder<TBuilder, TExtension> optionsBuilder)
        where TBuilder : RelationalDbContextOptionsBuilder<TBuilder, TExtension> where TExtension : RelationalOptionsExtension, new()
        => optionsBuilder.UseUlid(UlidStorageMode.String);

    public static RelationalDbContextOptionsBuilder<TBuilder, TExtension> UseUlidToBytes<TBuilder, TExtension>(this RelationalDbContextOptionsBuilder<TBuilder, TExtension> optionsBuilder)
        where TBuilder : RelationalDbContextOptionsBuilder<TBuilder, TExtension> where TExtension : RelationalOptionsExtension, new()
        => optionsBuilder.UseUlid(UlidStorageMode.Bytes);

    internal static RelationalDbContextOptionsBuilder<TBuilder, TExtension> UseUlid<TBuilder, TExtension>(this RelationalDbContextOptionsBuilder<TBuilder, TExtension> optionsBuilder, UlidStorageMode storageMode)
        where TBuilder : RelationalDbContextOptionsBuilder<TBuilder, TExtension> where TExtension : RelationalOptionsExtension, new()
    {
        if (optionsBuilder is null)
        {
            throw new ArgumentNullException(nameof(optionsBuilder));
        }

        var coreOptionsBuilder = ((IRelationalDbContextOptionsBuilderInfrastructure)optionsBuilder).OptionsBuilder;

        var extension = coreOptionsBuilder.Options.FindExtension<RelationalUlidOptionsExtension>();

        if (extension is not null && extension.StorageMode != storageMode)
        {
            throw new InvalidOperationException("You are registering Ulid with two different storage mode");
        }

        if (extension is null)
        {
            extension = new RelationalUlidOptionsExtension(storageMode);
        }

        ((IDbContextOptionsBuilderInfrastructure)coreOptionsBuilder).AddOrUpdateExtension(extension);

        return optionsBuilder;
    }
}

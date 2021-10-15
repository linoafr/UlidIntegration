using Linoa.UlidIntegration.EntityFrameworkCore.Relational.Storage;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore;

public static class RelationalUlidPropertyExtensions
{
    public static PropertyBuilder<Ulid> ConvertUlidToString(this PropertyBuilder<Ulid> propertyBuilder)
        => propertyBuilder.IsUnicode(false)
                          .HasMaxLength(26)
                          .IsFixedLength()
                          .HasConversion<UlidToStringValueConverter>();

    public static PropertyBuilder<Ulid?> ConvertUlidToString(this PropertyBuilder<Ulid?> propertyBuilder)
        => propertyBuilder.IsUnicode(false)
                          .HasMaxLength(26)
                          .IsFixedLength()
                          .HasConversion<UlidToStringValueConverter>();

    public static PropertyBuilder<Ulid> ConvertUlidToBytes(this PropertyBuilder<Ulid> propertyBuilder)
        => propertyBuilder.HasMaxLength(16)
                          .IsFixedLength()
                          .HasConversion<UlidToBytesValueConverter>();

    public static PropertyBuilder<Ulid?> ConvertUlidToBytes(this PropertyBuilder<Ulid?> propertyBuilder)
        => propertyBuilder.HasMaxLength(16)
                          .IsFixedLength()
                          .HasConversion<UlidToBytesValueConverter>();
}

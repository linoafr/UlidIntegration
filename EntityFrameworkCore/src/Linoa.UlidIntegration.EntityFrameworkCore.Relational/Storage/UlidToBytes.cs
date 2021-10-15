using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Linoa.UlidIntegration.EntityFrameworkCore.Relational.Storage;

internal class UlidToBytesValueConverter : ValueConverter<Ulid, byte[]>
{
    private static readonly ConverterMappingHints DefaultHints = new(size: 16);

    public UlidToBytesValueConverter() : this(null)
    {
    }

    public UlidToBytesValueConverter(ConverterMappingHints? mappingHints = null)
        : base(
                convertToProviderExpression: x => x.ToByteArray(),
                convertFromProviderExpression: x => new Ulid(x),
                mappingHints: DefaultHints.With(mappingHints))
    {
    }
}

internal class UlidToBytesTypeMapping : RelationalTypeMapping
{
    private static readonly UlidToBytesValueConverter ValueConverter = new();

    public UlidToBytesTypeMapping()
        : base(new RelationalTypeMappingParameters(new CoreTypeMappingParameters(
                typeof(Ulid),
                ValueConverter),
                "binary",
                StoreTypePostfix.Size,
                System.Data.DbType.Binary,
                size: 16,
                fixedLength: true))
    {
    }

    protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
        => new UlidToBytesTypeMapping();
}

internal class UlidToBytesTypeMappingSourcePlugin : IRelationalTypeMappingSourcePlugin
{
    public RelationalTypeMapping? FindMapping(in RelationalTypeMappingInfo mappingInfo)
    {
        if (mappingInfo.ClrType == typeof(Ulid))
        {
            return new UlidToBytesTypeMapping();
        }

        return null;
    }
}

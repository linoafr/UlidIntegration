using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Linoa.UlidIntegration.EntityFrameworkCore.Relational.Storage;

internal class UlidToStringValueConverter : ValueConverter<Ulid, string>
{
    private static readonly ConverterMappingHints DefaultHints = new(size: 26);

    public UlidToStringValueConverter() : this(null)
    {
    }

    internal UlidToStringValueConverter(ConverterMappingHints? mappingHints = null)
        : base(
                convertToProviderExpression: x => x.ToString(),
                convertFromProviderExpression: x => Ulid.Parse(x),
                mappingHints: DefaultHints.With(mappingHints))
    {
    }
}

internal class UlidToStringTypeMapping : RelationalTypeMapping
{
    private static readonly UlidToStringValueConverter ValueConverter = new();

    public UlidToStringTypeMapping()
        : base(new RelationalTypeMappingParameters(new CoreTypeMappingParameters(
                typeof(Ulid),
                ValueConverter),
                "char",
                StoreTypePostfix.Size,
                System.Data.DbType.AnsiStringFixedLength,
                unicode: false,
                size: 26,
                fixedLength: true))
    {
    }

    protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
        => new UlidToStringTypeMapping();

    protected override string SqlLiteralFormatString
        => "'{0}'";
}

internal class UlidToStringTypeMappingSourcePlugin : IRelationalTypeMappingSourcePlugin
{
    public RelationalTypeMapping? FindMapping(in RelationalTypeMappingInfo mappingInfo)
    {
        if (mappingInfo.ClrType == typeof(Ulid))
        {
            return new UlidToStringTypeMapping();
        }

        return null;
    }
}

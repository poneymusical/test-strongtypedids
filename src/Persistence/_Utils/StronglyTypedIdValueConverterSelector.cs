using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence._Utils;

public class StronglyTypedIdValueConverterSelector(ValueConverterSelectorDependencies dependencies)
    : ValueConverterSelector(dependencies)
{
    // The dictionary in the base type is private, so we need our own one here.
    private readonly ConcurrentDictionary<(Type ModelClrType, Type ProviderClrType), ValueConverterInfo> _converters
        = new();

    public override IEnumerable<ValueConverterInfo> Select(Type modelClrType, Type providerClrType = null)
    {
        var baseConverters = base.Select(modelClrType, providerClrType);
        foreach (var converter in baseConverters)
        {
            yield return converter;
        }

        // Extract the "real" type T from Nullable<T> if required
        var underlyingModelType = UnwrapNullableType(modelClrType);
        var underlyingProviderType = UnwrapNullableType(providerClrType);

        // 'null' means 'get any value converters for the modelClrType'
        if (underlyingProviderType is null || underlyingProviderType == typeof(Guid))
        {
            // Try and get a nested class with the expected name. 
            var converterType = underlyingModelType?.GetNestedType("StronglyTypedIdEfValueConverter");

            if (converterType != null)
            {
                yield return _converters.GetOrAdd((underlyingModelType!, typeof(Guid)),
                    _ =>
                    {
                        // Create an instance of the converter whenever it's requested.
                        Func<ValueConverterInfo, ValueConverter> factory =
                            info => ((ValueConverter) Activator.CreateInstance(converterType, info.MappingHints)!);

                        // Build the info for our strongly-typed ID => Guid converter
                        return new ValueConverterInfo(modelClrType, typeof(Guid), factory);
                    }
                );
            }
        }
    }

    private static Type? UnwrapNullableType(Type? type) =>
        type is null
            ? null
            : Nullable.GetUnderlyingType(type) ?? type;
}
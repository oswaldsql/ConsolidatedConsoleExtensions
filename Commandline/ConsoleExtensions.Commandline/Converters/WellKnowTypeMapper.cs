namespace ConsoleExtensions.Commandline.Converters;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

/// <summary>
/// Maps well known by using build in type converters.
/// </summary>
public class WellKnowTypeMapper : IValueConverter
{
    private static readonly Dictionary<Type, Func<TypeConverter>> converters = new()
    {
        {typeof(bool), () => new BooleanConverter()},
        {typeof(int), () => new Int32Converter()},
        {typeof(byte), () => new ByteConverter()},
        {typeof(char), () => new CharConverter()},
        {typeof(CultureInfo), () => new CultureInfoConverter()},
        {typeof(DateTime), () => new DateTimeConverter()},
        {typeof(DateTimeOffset), () => new DateTimeOffsetConverter()},
        {typeof(decimal), () => new DecimalConverter()},
        {typeof(double), () => new DoubleConverter()},
        {typeof(Guid), () => new GuidConverter()},
        {typeof(short), () => new Int16Converter()},
        {typeof(long), () => new Int64Converter()},
        {typeof(sbyte), () => new SByteConverter()},
        {typeof(float), () => new SingleConverter()},
        {typeof(TimeSpan), () => new TimeSpanConverter()},
    };

    /// <inheritdoc/>
    public ConverterPriority Priority => ConverterPriority.Default;

    /// <inheritdoc/>
    public bool TryConvertToString(object source, ICustomAttributeProvider customAttributeProvider, out string result)
    {
        if (converters.TryGetValue(source.GetType(), out var converter))
        {
            result = converter().ConvertToString(source);
            return true;
        }

        result = null;
        return false;
    }

    /// <inheritdoc/>
    public bool TryConvertToValue(string source, Type type, ICustomAttributeProvider customAttributeProvider, out object result)
    {
        if (converters.TryGetValue(source.GetType(), out var converter))
        {
            result = converter().ConvertTo(source, type);
            return true;
        }

        result = null;
        return false;
    }
}
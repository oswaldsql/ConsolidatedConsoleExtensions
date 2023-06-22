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
        {typeof(int), () => new Int32Converter()},
        {typeof(byte), () => new ByteConverter()},
        {typeof(DateTime), () => new DateTimeConverter()},
        {typeof(double), () => new DoubleConverter()},
        {typeof(short), () => new Int16Converter()},
        {typeof(long), () => new Int64Converter()},
        {typeof(sbyte), () => new SByteConverter()},
        {typeof(float), () => new SingleConverter()},
    };

    private static readonly Dictionary<Type, Func<string, object>> convertFunctions = new()
    {
        {typeof(bool), str => Boolean.Parse(str)},
        {typeof(char), str => Char.Parse(str)},
        {typeof(CultureInfo), str => CultureInfo.GetCultureInfo(str)},
        {typeof(DateTime), str => DateTime.Parse(str)},
        {typeof(DateTimeOffset), str => DateTimeOffset.Parse(str)},
        {typeof(decimal), str => decimal.Parse(str)},
        {typeof(Guid), str => Guid.Parse(str)},
        {typeof(TimeSpan), str => TimeSpan.Parse(str)},
    };


    /// <inheritdoc/>
    public ConverterPriority Priority => ConverterPriority.Default;

    /// <inheritdoc/>
    public bool TryConvertToString(object source, ICustomAttributeProvider customAttributeProvider, out string result)
    {
        if(convertFunctions.TryGetValue(source.GetType(), out var convertFunction))
        {
            result = source.ToString();
            return true;
        }

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
        if(convertFunctions.TryGetValue(type, out var convertFunction))
        {
            result = convertFunction(source);
            return true;
        }

        if (converters.TryGetValue(type, out var converter))
        {
            result = converter().ConvertTo(source, type);
            return true;
        }

        result = null;
        return false;
    }
}
namespace ConsoleExtensions.Commandline.Converters;

using System;
using System.ComponentModel;
using System.Reflection;
using Util;


/// <summary>
/// Converts a value using a type converter.
/// </summary>
public class ConvertUsingTypeConverter : IValueConverter
{
    /// <inheritdoc />
    public ConverterPriority Priority => ConverterPriority.First;

    /// <inheritdoc />
    public bool TryConvertToString(object source, ICustomAttributeProvider customAttributeProvider, out string result)
    {
        if (customAttributeProvider.TryGetCustomAttribute<TypeConverterAttribute>(out var customAttribute))
        {
            var typeName = customAttribute.ConverterTypeName;
            var typeFromGetType = Type.GetType(typeName);

            if (typeFromGetType != null && Activator.CreateInstance(typeFromGetType) is TypeConverter instance)
            {
                if (instance.CanConvertFrom(source.GetType()))
                {
                    result = instance.ConvertToString(source);
                    return true;
                }
            }
        }

        result = null;
        return false;
    }

    /// <inheritdoc />
    public bool TryConvertToValue(string source, Type type, ICustomAttributeProvider customAttributeProvider, out object result)
    {
        if (customAttributeProvider.TryGetCustomAttribute<TypeConverterAttribute>(out var customAttribute))
        {
            var typeName = customAttribute.ConverterTypeName;
            var typeFromGetType = Type.GetType(typeName);

            if (typeFromGetType != null && Activator.CreateInstance(typeFromGetType) is TypeConverter instance)
            {
                if (instance.CanConvertTo(type))
                {
                    result = instance.ConvertTo(source, type);
                    return true;
                }
            }
        }

        result = null;
        return false;
    }
}
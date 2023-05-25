namespace ConsoleExtensions.Commandline.Converters;

using System;
using System.ComponentModel;
using System.Reflection;
using Util;

public class ConvertUsingTypeConverter : IValueConverter
{
    public ConverterPriority Priority => ConverterPriority.First;

    public bool TryConvertToString(object source, ICustomAttributeProvider customAttributeProvider, out string result)
    {
        if (customAttributeProvider.TryGetCustomAttribute<TypeConverterAttribute>(out var customAttribute))
        {
            if (Activator.CreateInstance(customAttribute.GetType()) is TypeConverter instance)
            {
                result = instance.ConvertToString(source);
                return true;
            }
        }

        result = null;
        return false;
    }

    public bool TryConvertToValue(string source, Type type, ICustomAttributeProvider customAttributeProvider, out object result)
    {
        if (customAttributeProvider.TryGetCustomAttribute<TypeConverterAttribute>(out var customAttribute))
        {
            if (Activator.CreateInstance(customAttribute.GetType()) is TypeConverter instance)
            {
                result = instance.ConvertTo(source, type);
                return true;
            }
        }

        result = null;
        return false;
    }
}
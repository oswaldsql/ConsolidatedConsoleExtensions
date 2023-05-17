namespace ConsoleExtensions.Commandline.Converters;

using System;
using System.Reflection;

public class StringConverter : ValueConverterBack
{
    public override ConverterPriority Priority => ConverterPriority.Later;

    protected override bool CanConvert(Type type)
    {
        return type == typeof(string);
    }

    protected override string ConvertToString(object source, ICustomAttributeProvider customAttributeProvider)
    {
        return source.ToString();
    }

    protected override object ConvertToValue(string source, Type type, ICustomAttributeProvider customAttributeProvider)
    {
        return source;
    }
}
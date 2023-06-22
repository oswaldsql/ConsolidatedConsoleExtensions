// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IValueConverter.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Converters;

using System;
using System.Reflection;

public abstract class ValueConverterBack : IValueConverter
{
    public virtual ConverterPriority Priority { get; }

    protected abstract bool CanConvert(Type type);

    protected abstract string ConvertToString(object source, ICustomAttributeProvider customAttributeProvider);

    protected abstract object ConvertToValue(string source, Type type, ICustomAttributeProvider customAttributeProvider);

    public virtual bool TryConvertToString(object source, ICustomAttributeProvider customAttributeProvider, out string result)
    {
        if (!this.CanConvert(source.GetType()))
        {
            result = "";
            return false;
        }

        result = this.ConvertToString(source, customAttributeProvider);
        return true;
    }

    public bool TryConvertToValue(string source, Type type, ICustomAttributeProvider customAttributeProvider, out object result)
    {
        if (!this.CanConvert(type))
        {
            result = "";
            return false;
        }

        result = this.ConvertToValue(source, type, customAttributeProvider);
        return true;
    }
}
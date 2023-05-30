namespace ConsoleExtensions.Commandline.Tests.ConverterTests;

using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Converters;
using Xunit;

public class ConvertUsingTypeConverterTests
{
    [Fact]
    public void WhenATypeConverterIsSpecifiedItIsUsed()
    {
        // Arrange
        var sut = new ConvertUsingTypeConverter();

        // Act
        var mockAttributes = new MockAttributeProvider(new TypeConverterAttribute(typeof(TestTypeConverter))) ;
        var actual = sut.TryConvertToValue("One", typeof(int), mockAttributes, out var result);

        // Assert
        Assert.True(actual, "Should convert.");
        var integer = Assert.IsType<int>(result);
        Assert.Equal(1, integer);
    }

    [Fact]
    public void WhenATypeConverterIsSpecifiedItIsUsedToConvertToString()
    {
        // Arrange
        var sut = new ConvertUsingTypeConverter();

        // Act
        var mockAttributes = new MockAttributeProvider(new TypeConverterAttribute(typeof(TestTypeConverter))) ;
        var actual = sut.TryConvertToString(1, mockAttributes, out var result);

        // Assert
        Assert.True(actual, "Should convert.");
        Assert.Equal("One", result);
    }

    [Fact]
    public void WhenATypeConverterIsNotSpecifiedItIsNotUsedToConvertToInt()
    {
        // Arrange
        var sut = new ConvertUsingTypeConverter();

        // Act
        var mockAttributes = new MockAttributeProvider() ;
        var actual = sut.TryConvertToValue("One", typeof(int), mockAttributes, out var result);

        // Assert
        Assert.False(actual, "Should not convert.");
        Assert.Null(result);
    }

    [Fact]
    public void WhenATypeConverterIsNotSpecifiedItIsNotUsedToConvertToString()
    {
        // Arrange
        var sut = new ConvertUsingTypeConverter();

        // Act
        var mockAttributes = new MockAttributeProvider() ;
        var actual = sut.TryConvertToString(1, mockAttributes, out var result);

        // Assert
        Assert.False(actual, "Should not convert.");
        Assert.Null(result);
    }

    [Fact]
    public void WhenATypeConverterIsSpecifiedWithoutAConverterNoConverterIsRun()
    {
        // Arrange
        var sut = new ConvertUsingTypeConverter();

        // Act
        var mockAttributes = new MockAttributeProvider(new TypeConverterAttribute()) ;
        var actual = sut.TryConvertToString(1, mockAttributes, out var result);

        // Assert
        Assert.False(actual, "Should not convert.");
        Assert.Null(result);
    }

    public class TestTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(int);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(int);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if((value as string) == "One")
                return 1;

            if (value as int? == 1)
                return "One";
            
            throw new NotImplementedException();
        }

    }

    public class MockAttributeProvider : ICustomAttributeProvider
    {
        private readonly object[] attributes;

        public MockAttributeProvider(params Attribute[] attributes)
        {
            this.attributes = attributes;
        }

        public object[] GetCustomAttributes(bool inherit)
        {
            return this.attributes;
        }

        public object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return this.attributes.Where(t => t.GetType() == attributeType).ToArray();
        }

        public bool IsDefined(Type attributeType, bool inherit)
        {
            return this.attributes.Any(t => t.GetType() == attributeType);
        }
    }
}
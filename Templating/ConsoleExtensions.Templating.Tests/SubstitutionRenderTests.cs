// ReSharper disable ExceptionNotDocumented
namespace ConsoleExtensions.Templating.Tests;

using System;
using System.Globalization;

using Proxy.TestHelpers;
using Renderers;

using Xunit;

public class SubstitutionRenderTests
{
  /// <summary>
  /// Given a empty configuration when rendering then substitute with object value.
  /// </summary>
  [Fact]
  public void GivenAConfigWithOnlyFormat_WhenRendering_ThenSubstituteUsesFormatter()
  {
    // Arrange
    var proxy = new TestProxy();

    // Act
    var actual = proxy.WriteTemplate("{:format}", new FormattalbeObject(), new CultureInfo("da-DK"));

    // Assert
    Assert.Equal("format|da-DK", actual.ToString());
  }

  /// <summary>
  /// Given a empty configuration when rendering then substitute with object value.
  /// </summary>
  [Fact]
  public void GivenAEmptyConfig_WhenRendering_ThenSubstituteWithObjectValue()
  {
    // Arrange
    var proxy = new TestProxy();

    // Act
    var actual = proxy.WriteTemplate("{}", new SimpleObject(), CultureInfo.CurrentUICulture);

    // Assert
    Assert.Equal("SimpleObject", actual.ToString());
  }

  /// <summary>
  /// Given a nested property name when rendering then substitute with value.
  /// </summary>
  [Fact]
  public void GivenANestedPropertyName_WhenRendering_ThenSubstituteWithValue()
  {
    // Arrange
    var substitutionRenderer = new SubstitutionRenderer("Value.Length", new Template(new TemplateParser()));
    var consoleProxy = new TestProxy();

    // Act
    substitutionRenderer.Render(consoleProxy, new { Value = "This is the value" }, CultureInfo.CurrentUICulture);

    // Assert
    Assert.Equal("17", consoleProxy.ToString());
  }

  /// <summary>
  /// Given a null value when substituting then replace with empty string.
  /// </summary>
  [Fact]
  public void GivenANullValue_WhenSubstituting_ThenReplaceWithEmptyString()
  {
    // Arrange
    var substitutionRenderer = new SubstitutionRenderer("Value", new Template(new TemplateParser()));
    var consoleProxy = new TestProxy();

    // Act
    substitutionRenderer.Render(consoleProxy, new { Value = (string)null }, CultureInfo.CurrentUICulture);

    // Assert
    Assert.Equal(consoleProxy.ToString(), string.Empty);
  }

  /// <summary>
  /// Given a property name with formatting when rendering then substitute with formatted value.
  /// </summary>
  [Fact]
  public void GivenAPropertyNameWithFormatting_WhenRendering_ThenSubstituteWithFormattedValue()
  {
    // Arrange
    var substitutionRenderer = new SubstitutionRenderer("Value:D5", new Template(new TemplateParser()));
    var consoleProxy = new TestProxy();

    // Act
    substitutionRenderer.Render(consoleProxy, new { Value = 1 }, CultureInfo.CurrentUICulture);

    // Assert
    Assert.Equal("00001", consoleProxy.ToString());
  }

  /// <summary>
  /// Given a simple property name when rendering then substitute with the property value.
  /// </summary>
  [Fact]
  public void GivenASimplePropertyName_WhenRendering_ThenSubstituteWithThePropertyValue()
  {
    // Arrange
    var substitutionRenderer = new SubstitutionRenderer("Value", new Template(new TemplateParser()));
    var consoleProxy = new TestProxy();

    // Act
    substitutionRenderer.Render(consoleProxy, new { Value = "This is the value" }, CultureInfo.CurrentUICulture);

    // Assert
    Assert.Equal("This is the value", consoleProxy.ToString());
  }

  /// <summary>
  /// Given a simple property name with wrong casing when rendering then substitute with empty string.
  /// </summary>
  [Fact]
  public void GivenASimplePropertyNameWithWrongCasing_WhenRendering_ThenSubstituteWithEmptyString()
  {
    // Arrange
    var substitutionRenderer = new SubstitutionRenderer("value", new Template(new TemplateParser()));
    var consoleProxy = new TestProxy();

    // Act
    substitutionRenderer.Render(consoleProxy, new { Value = "This is the value" }, CultureInfo.CurrentUICulture);

    // Assert
    Assert.Equal(string.Empty, consoleProxy.ToString());
  }

  [Fact]
  public void GivenTemplateWithSimpleSub_WhenRender_ThenRenderExpected()
  {
    // Arrange
    var proxy = new TestProxy();

    // Act
    var actual = proxy.WriteTemplate("start|{Name}|end", new { Name = "Tom" });

    // Assert
    Assert.Equal("start|Tom|end", actual.ToString());
  }

  [Fact]
  public void GivenTemplateWithSub_WhenRenderWithNullObject_ThenRenderNothing()
  {
    // Arrange
    var proxy = new TestProxy();

    // Act
    var actual = proxy.WriteTemplate("start|{Unknown}|end");

    // Assert
    Assert.Equal("start||end", actual.ToString());
  }

  [Fact]
  public void GivenTemplateWithSub_WhenRenderWithSpecificCulture_ThenUseCultureToRender()
  {
    // Arrange
    var proxyDk = new TestProxy();
    var proxyUs = new TestProxy();

    // Act
    var actualDk = proxyDk.WriteTemplate("start|{number}|end", new { number = 1.2 }, new CultureInfo("da-dk"));
    var actualUs = proxyUs.WriteTemplate("start|{number}|end", new { number = 1.2 }, new CultureInfo("en-us"));

    // Assert
    Assert.Equal("start|1,2|end", actualDk.ToString());
    Assert.Equal("start|1.2|end", actualUs.ToString());
  }

  /// <summary>
  /// Given a template with unknown sub 
  /// when render 
  /// then render nothing.
  /// </summary>
  [Fact]
  public void GivenTemplateWithUnknownSub_WhenRender_ThenRenderNothing()
  {
    // Arrange
    var proxy = new TestProxy();

    // Act
    var actual = proxy.WriteTemplate("start|{Unknown}|end", new { Name = "Tom" });

    // Assert
    Assert.Equal("start||end", actual.ToString());
  }

  public class FormattalbeObject : IFormattable
  {
    public string ToString(string format, IFormatProvider formatProvider)
    {
      return format + "|" + formatProvider;
    }
  }

  public class SimpleObject
  {
    public override string ToString()
    {
      return "SimpleObject";
    }
  }
}
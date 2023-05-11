// ReSharper disable ExceptionNotDocumented
namespace ConsoleExtensions.Templating.Tests;

using Proxy.TestHelpers;
using Renderers;

using Xunit;

/// <summary>
/// Tests the IfRenderer class
/// </summary>
public class IfRendererTests
{
  [Theory]
  [InlineData(null, false)]
  [InlineData(0d, false)]
  [InlineData(1d, true)]
  public void GivenANullableType_WhenTestingForTruthy_ThenReturnExpectedResult(double? value, bool expected)
  {
    // Arrange
    var render = new IfRender();

    // Act
    var actual = render.ShouldBeRendered(value);

    // Assert
    Assert.Equal(expected, actual);
  }

  [Theory]
  [InlineData(0, false)]
  [InlineData(1, true)]
  public void GivenANumberValue_WhenTestingForTruthy_ThenReturnExpected(int value, bool expected)
  {
    // Arrange
    var render = new IfRender();

    // Act
    var byteActual = render.ShouldBeRendered((byte)value);
    var int16Actual = render.ShouldBeRendered((short)value);
    var int32Actual = render.ShouldBeRendered(value);
    var int64Actual = render.ShouldBeRendered((short)value);
    var decimalActual = render.ShouldBeRendered((decimal)value);
    var doubleActual = render.ShouldBeRendered((double)value);
    var floatActual = render.ShouldBeRendered((float)value);

    // Assert
    Assert.Equal(expected, byteActual);
    Assert.Equal(expected, int16Actual);
    Assert.Equal(expected, int32Actual);
    Assert.Equal(expected, int64Actual);
    Assert.Equal(expected, decimalActual);
    Assert.Equal(expected, doubleActual);
    Assert.Equal(expected, floatActual);
  }

  [Theory]
  [InlineData(false, "false")]
  [InlineData(true, "true")]
  [InlineData(0.000000000001, "true")]
  public void GivenAObject_WhenTestingForTruthy_ThenExpectedResultShouldBeReturned(object source, string expected)
  {
    // Arrange
    var proxy = new TestProxy();

    // Act
    var actual = proxy.WriteTemplate("[if:source]true[/][ifnot:source]false[/]", new { source });

    // Assert
    Assert.Equal(expected, actual.ToString());
  }

  [Theory]
  [InlineData(null, false)]
  [InlineData("", false)]
  [InlineData(" ", true)]
  public void GivenAString_WhenTestingForFalsy_ThenShouldReturnExpected(string source, bool expected)
  {
    // Arrange
    var render = new IfRender();

    // Act
    var actual = render.ShouldBeRendered(source);

    // Assert
    Assert.Equal(expected, actual);
  }

  [Theory]
  [InlineData(new string[0], false)]
  [InlineData(new[] { "" }, true)]
  [InlineData(new string[] { null }, true)]
  public void GivenAStringArray_WhenTestingForFalsy_ThenShouldReturnExpected(string[] source, bool expected)
  {
    // Arrange
    var render = new IfRender();

    // Act
    var actual = render.ShouldBeRendered(source);

    // Assert
    Assert.Equal(expected, actual);
  }

  [Fact]
  public void GivenATemplateWithAUnknownParameter_WhenWritingTheTemplate_ThenTheValueShouldEvaluateToFalse()
  {
    // Arrange
    var proxy = new TestProxy();

    // Act
    var actual = proxy.WriteTemplate("[if:source]true[/][ifnot:source]false[/]", new { });

    // Assert
    Assert.Equal("false", actual.ToString());
  }

  [Fact]
  public void GivenATemplateWithAUnknownObjectType_WhenWritingTheTemplate_ThenTheValueShouldEvaluateToTrue()
  {
    // Arrange
    var proxy = new TestProxy();

    // Act
    var actual = proxy.WriteTemplate("[if]true[/][ifnot]false[/]", new { });

    // Assert
    Assert.Equal("true", actual.ToString());
  }
}
// ReSharper disable ExceptionNotDocumented
namespace ConsoleExtensions.Templating.Tests;

using Proxy.TestHelpers;

using Xunit;

public class WithRenderTests
{
  [Fact]
  public void GivenATemplateContainingWith_WhenGivenKnownValue_ThenRenderValue()
  {
    // Arrange
    var proxy = new TestProxy();

    // Act
    var actual = proxy.WriteTemplate("start|[with:Test]{Length}[/]|end", new { Test = "test" });

    // Assert
    Assert.Equal("start|4|end", actual.ToString());
  }

  [Fact]
  public void GivenATemplateContainingWith_WhenGivenNullValue_ThenNoRender()
  {
    // Arrange
    var proxy = new TestProxy();

    // Act
    var actual = proxy.WriteTemplate("start|[with]T[/]|end");

    // Assert
    Assert.Equal("start||end", actual.ToString());
  }

  [Fact]
  public void GivenATemplateContainingWith_WhenGivenUnknownValue_ThenNoRender()
  {
    // Arrange
    var proxy = new TestProxy();

    // Act
    var actual = proxy.WriteTemplate("start|[with:unknown]T[/]|end", new { });

    // Assert
    Assert.Equal("start||end", actual.ToString());
  }
}
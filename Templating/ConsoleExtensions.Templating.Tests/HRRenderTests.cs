// ReSharper disable ExceptionNotDocumented

namespace ConsoleExtensions.Templating.Tests;

using Proxy.TestHelpers;

using Xunit;

public class HrRenderTests
{
  [Fact]
  public void GivenABlankLine_WhenRenderHr_ThenRenderFullLine()
  {
    // Arrange
    var proxy = new TestProxy();

    // Act
    var actual = proxy.WriteTemplate("[hr/]");

    // Assert
    Assert.Equal(new string('-', 80), actual.ToString());
  }

  [Fact]
  public void GivenABlankLineWithStartStopTag_WhenRenderHr_ThenRenderFullLine()
  {
    // Arrange
    var proxy = new TestProxy();

    // Act
    var actual = proxy.WriteTemplate("[hr][/]");

    // Assert
    Assert.Equal(new string('-', 80), actual.ToString());
  }

  [Fact]
  public void GivenAExistingText_WhenRenderHr_ThenRenderNewLineFirst()
  {
    // Arrange
    var proxy = new TestProxy();

    // Act
    var actual = proxy.WriteTemplate("start|[hr/]|end", new { Test = "test" });

    // Assert
    Assert.Equal("start|\n" + new string('-', 80) + "|end", actual.ToString());
  }

  [Fact]
  public void GivenTextNestedWithinHr_WhenRenderHr_ThenNestedIsIgnored()
  {
    // Arrange
    var proxy = new TestProxy();

    // Act
    var actual = proxy.WriteTemplate("start|[hr]test[/]|end", new { Test = "test" });

    // Assert
    Assert.Equal("start|\n" + new string('-', 80) + "|end", actual.ToString());
  }
}
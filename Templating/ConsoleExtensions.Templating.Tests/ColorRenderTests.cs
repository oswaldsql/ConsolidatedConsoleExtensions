// ReSharper disable ExceptionNotDocumented
namespace ConsoleExtensions.Templating.Tests;

using Proxy.TestHelpers;

using Xunit;

public class ColorRenderTests
{
  [Theory]
  [InlineData("[c:white]value", "[F=White]value[F=Black]")]
  [InlineData("[c:white]value[/c]", "[F=White]value[F=Black]")]
  [InlineData("[c:white]value[/]", "[F=White]value[F=Black]")]
  [InlineData("[c:unknown]value[/]", "value")]
  [InlineData("[c:white]1[c:green]2[/]3[/]", "[F=White]1[F=Green]2[F=White]3[F=Black]")]
  [InlineData("Hello [c:Green]Tom[/C], how are you", "Hello [F=Green]Tom[F=Black], how are you")]
  [InlineData("t[c:Green]e[c:Black]s", "t[F=Green]e[F=Black]s[F=Green][F=Black]")]
  public void GivenATemplateColor_WhenRendering_ThenExpectedColorCommandShouldBeRendered(string source, string expected)
  {
    // Arrange
    var proxy = new TestProxy();

    // Act
    var actual = proxy.WriteTemplate(source).ToString();

    // Assert
    Assert.Equal(expected, actual);
  }
}
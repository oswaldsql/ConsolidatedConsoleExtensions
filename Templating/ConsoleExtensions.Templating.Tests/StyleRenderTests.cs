// ReSharper disable ExceptionNotDocumented
namespace ConsoleExtensions.Templating.Tests;

using System;

using Proxy;
using Proxy.TestHelpers;

using Xunit;

public class StyleRenderTests
{
  [Fact]
  public void GivenATemplateWithUnknownStyle_WhenRender_ThenRenderNoStyle()
  {
    // Arrange
    var proxy = new TestProxy();

    // Act
    var actual = proxy.WriteTemplate("start|[s:test]test[/]|end", new { Test = "test" });

    // Assert
    Assert.Equal("start|test|end", actual.ToString());
  }

  [Fact]
  public void GivenATemplateWithKnownStyle_WhenRender_ThenRenderColorSettings()
  {
    // Arrange
    var parser = new TemplateParser();
    var template = parser.Parse("start|[s:test]test[/s]|end");
    template.Styles.Add("test", new ConsoleStyle("test", ConsoleColor.Green, ConsoleColor.Red));
    var proxy = new TestProxy();

    // Act
    var actual = proxy.WriteTemplate(template, new { Test = "test" });

    // Assert
    Assert.Equal("start|[F=Green][B=Red]test[F=Black][B=Black]|end", actual.ToString());
  }
}
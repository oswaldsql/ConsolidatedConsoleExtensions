// ReSharper disable ExceptionNotDocumented

namespace ConsoleExtensions.Templating.Tests;

using System.Linq;

using Proxy.TestHelpers;

using Xunit;

public class ForEachRendereTests
{
  [Fact]
  public void GivenATemplateWithEmptyRepeater_WhenGivenArray_ThenValuesAreRenderd()
  {
    // Arrange
    var proxy = new TestProxy();

    // Act
    var enumeration = Enumerable.Range(0, 10);
    var actual = proxy.WriteTemplate("start|[ForEach]{}[/]|end", enumeration);

    // Assert
    Assert.Equal("start|0123456789|end", actual.ToString());
  }

  [Fact]
  public void GivenATemplateWithRepeater_WhenGivenANull_ThenNoRenderingIsDone()
  {
    // Arrange
    var proxy = new TestProxy();

    // Act
    var actual = proxy.WriteTemplate("start|[ForEach:test]t[/]|end");

    // Assert
    Assert.Equal("start||end", actual.ToString());
  }

  [Fact]
  public void GivenATemplateWithRepeater_WhenGivenArray_ThenValuesAreRenderd()
  {
    // Arrange
    var proxy = new TestProxy();

    // Act
    var actual = proxy.WriteTemplate("start|[ForEach:Test]'{}'[/]|end", new { Test = new[] { "A", "B", "C" } });

    // Assert
    Assert.Equal("start|'A''B''C'|end", actual.ToString());
  }

  [Fact]
  public void GivenATemplateWithRepeater_WhenGivenEmptyArray_ThenNoRenderIsDone()
  {
    // Arrange
    var proxy = new TestProxy();

    // Act
    var actual = proxy.WriteTemplate("start|[ForEach:Test]t[/]|end", new { Test = new string[0] });

    // Assert
    Assert.Equal("start||end", actual.ToString());
  }

  [Fact]
  public void GivenATemplateWithRepeater_WhenGivenIEnumerable_ThenValuesAreRenderd()
  {
    // Arrange
    var proxy = new TestProxy();

    // Act
    var enumeration = Enumerable.Range(0, 10);
    var actual = proxy.WriteTemplate("start|[ForEach:Test]{}[/]|end", new { Test = enumeration.AsEnumerable() });

    // Assert
    Assert.Equal("start|0123456789|end", actual.ToString());
  }

  [Fact]
  public void GivenATemplateWithRepeater_WhenGivenList_ThenValuesAreRenderd()
  {
    // Arrange
    var proxy = new TestProxy();

    // Act
    var enumeration = new List<string>() { "A", "B", "C" };
    var actual = proxy.WriteTemplate("start|[ForEach:Test]'{}'[/]|end", new { Test = enumeration });

    // Assert
    Assert.Equal("start|'A''B''C'|end", actual.ToString());
  }

  [Fact]
  public void GivenATemplateWithRepeater_WhenGivenSimpleValue_ThenJustTheValueIsRendred()
  {
    // Arrange
    var proxy = new TestProxy();

    // Act
    var actual = proxy.WriteTemplate("start|[ForEach:test]{}[/]|end", new { test = true });

    // Assert
    Assert.Equal("start|True|end", actual.ToString());
  }

  [Fact]
  public void GivenATemplateWithRepeater_WhenGivenUnknownValue_ThenNoRenderIsDone()
  {
    // Arrange
    var proxy = new TestProxy();

    // Act
    var actual = proxy.WriteTemplate("start|[ForEach:Unknown]{}[/]|end", new { });

    // Assert
    Assert.Equal("start||end", actual.ToString());
  }
}
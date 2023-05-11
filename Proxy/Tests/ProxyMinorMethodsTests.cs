namespace ConsoleExtensions.Proxy.Tests;

// ReSharper disable ExceptionNotDocumented
// ReSharper disable ExceptionNotDocumentedOptional
using TestHelpers;

using Xunit;

public class ProxyMinorMethodsTests
{
	[Fact]
	public void GivenATestProxy_WhenCallingBeep_ThenItShouldBeRendered()
	{
		// Arrange
		var proxy = new TestProxy();

		// Act
		proxy.Beep();

		// Assert
		Assert.Equal("[Beep]", proxy.ToString());
	}

	[Fact]
	public void GivenATestProxy_WhenCallingClear_ThenItShouldBeRendered()
	{
		// Arrange
		var proxy = new TestProxy();

		// Act
		proxy.Clear();

		// Assert
		Assert.Equal("[Clear]", proxy.ToString());
	}

	[Fact]
	public void GivenATestProxy_WhenCallingShowHideCursor_ThenItShouldBeRendered()
	{
		// Arrange
		var proxy = new TestProxy();

		// Act
		proxy.HideCursor().ShowCursor();

		// Assert
		Assert.Equal("[HideCursor][ShowCursor]", proxy.ToString());
	}

	[Fact]
	public void GivenATestProxy_WhenSettingAndGettingTitle_ThenItShouldBeRendered()
	{
		// Arrange
		var proxy = new TestProxy();

		// Act
		proxy.SetTitle("Test").GetTitle(out var actual);

		// Assert
		Assert.Equal("[SetTitle:Test]", proxy.ToString());
		Assert.Equal("Test", actual);
	}
}
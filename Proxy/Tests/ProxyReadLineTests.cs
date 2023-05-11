namespace ConsoleExtensions.Proxy.Tests;

// ReSharper disable ExceptionNotDocumented
// ReSharper disable ExceptionNotDocumentedOptional
using TestHelpers;

using Xunit;

public class ProxyReadLineTests
{
	[Fact]
	public void GivenATestProxyWithKeys_WhenReachingANewLine_ThenReadLineShouldReturnTheExpectedResult()
	{
		// Arrange
		var testProxy = new TestProxy();
		testProxy.Keys.Add("Read this\nBut not this");

		// Act
		testProxy.ReadLine(out var actual);

		// Assert
		Assert.Equal("Read this", actual);
	}

	[Fact]
	public void GivenATestProxyWithKeys_WhenReachingEndOfKeys_ThenReadLineShouldThrowExceptions()
	{
		// Arrange
		var testProxy = new TestProxy();
		testProxy.Keys.Add("Read this");

		// Act
		var exception = Record.Exception(() => testProxy.ReadLine(out _));

		// Assert
		Assert.IsType<NoMoreKeysInKeyQueue>(exception);
	}
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClearLineRenderTests.cs" company="Lasse Sj�rup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Templating.Tests;

using Proxy.TestHelpers;

using Xunit;

/// <summary>
///     Class ClearLineRenderTests.
/// </summary>
public class ClearLineRenderTests
{
    /// <summary>
    ///     Given a template with clear line
    ///     when render
    ///     then clear remanding is rendered.
    /// </summary>
    [Fact]
    public void GivenTemplateWithClearLine_WhenRender_ThenClearRemandingIsRendered()
    {
        // Arrange
        var proxy = new TestProxy();

        // Act
        var actual = proxy.WriteTemplate("[ClearLine/]", new { Test = "test" });

        // Assert
        Assert.Equal($"{new string(' ', 80)}", actual.ToString());
    }

    /// <summary>
    ///     Given a template with clear line with content
    ///     when render then
    ///     content is ignored.
    /// </summary>
    [Fact]
    public void GivenTemplateWithClearLineWithContent_WhenRender_ThenContentIsIgnored()
    {
        // Arrange
        var proxy = new TestProxy();

        // Act
        var actual = proxy.WriteTemplate("start|[ClearLine]Tester[/]|end", new { Test = "test" });

        // Assert
        Assert.Equal($"start|{new string(' ', 74)}|end", actual.ToString());
    }
}
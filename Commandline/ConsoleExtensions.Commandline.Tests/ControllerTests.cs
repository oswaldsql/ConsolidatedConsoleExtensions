// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControllerTests.cs" company="Lasse Sjørup">
//   Copyright (c) 2023 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Tests;

using System;
using System.Threading.Tasks;
using Proxy.TestHelpers;

using Xunit;
using Xunit.Abstractions;

/// <summary>
///     Class ControllerTests.
/// </summary>
public class ControllerTests
{
    private readonly ITestOutputHelper testHelperOutput;

    public ControllerTests(ITestOutputHelper testHelperOutput)
    {
        this.testHelperOutput = testHelperOutput;
    }
    // Initialize the controller with a simple class
    // When the model is null or not a class a null exception is thrown
    // Setup is called when the controller is initialized
    // Calling run with no arguments calls the help method
    // Calling run with help calls the help method
    // Calling run with a method that does not exist throws an exception
    // Calling run with a method that exists calls the method
    // Calling run with a method that exists and arguments calls the method with the arguments
    // Calling the generic run will create a controller and call the method

    /// <summary>
    /// Initialize the controller with a simple class.
    /// </summary>
    [Fact]
    public void InitializeTheControllerWithASimpleClass()
    {
        // Arrange
        var model = new DummyModel();
        
        // Act
        var actual = new Controller(model);

        // Assert
        Assert.Equal(model, actual.Model);
        Assert.NotNull(actual.Proxy);
        Assert.NotNull(actual.ModelMap);
        Assert.NotNull(actual.TemplateParser);
    }

    /// <summary>
    /// Invalid models throw a argument exception.
    /// </summary>
    [Fact]
    public void InvalidModelsThrowAArgumentException()
    {
        // Arrange

        // Act
        var exception = Record.Exception(() => new Controller(null));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Equal("model", ((ArgumentException)exception).ParamName);
    }

    /// <summary>
    /// The setup lambda provided is called on new.
    /// </summary>
    [Fact]
    public void TheSetupLambdaProvidedIsCalledOnNew()
    {
        // Arrange
        var model = new DummyModel();
        var setupCalled = false;

        // Act
        var actual = new Controller(model, _ => { setupCalled = true;});

        // Assert
        Assert.NotNull(actual);
        Assert.True(setupCalled);
    }

    /// <summary>
    /// Calling the controller without arguments renders help.
    /// </summary>
    [Fact]
    public void CallingTheControllerWithoutArgumentsRendersHelp()
    {
        // Arrange
        string[] args = {};
        var consoleProxy = new TestProxy();

        // Act
        var exitCode = Controller.Run<DummyModel>(setup =>
        {
            setup.ArgumentsProvider = () => args;
            setup.Proxy = consoleProxy;
        });

        // Assert
        var output = consoleProxy.ToString();
        this.testHelperOutput.WriteLine(output);
        Assert.Equal("Overloaded help message", output);
        
        Assert.Equal(0, exitCode);
    }
   
    /// <summary>
    /// Calling the generic run creates a model and calls it.
    /// </summary>
    [Fact]
    public void CallingTheGenericRunCreatesAModelAndCallsIt()
    {
        // Arrange
        string[] args = {"DummyMethod"};
        var consoleProxy = new TestProxy();
        Controller actual = null;

        // Act
        var exitCode = Controller.Run<DummyModel>(setup =>
        {
            setup.ArgumentsProvider = () => args;
            setup.Proxy = consoleProxy;
            actual = setup;
        });

        // Assert
        this.testHelperOutput.WriteLine(consoleProxy.ToString());

        Assert.NotNull(actual);
        Assert.IsType<DummyModel>(actual.Model);
        Assert.Equal(0, exitCode);
    }

    /// <summary>
    /// Calling the generic asynchronous run creates a model and calls it.
    /// </summary>
    [Fact]
    public async Task CallingTheGenericAsyncRunCreatesAModelAndCallsIt()
    {
        // Arrange
        string[] args = {"DummyMethod"};
        var consoleProxy = new TestProxy();
        Controller actual = null;

        // Act
        var exitCode = await Controller.RunAsync<DummyModel>(setup =>
        {
            setup.ArgumentsProvider = () => args;
            setup.Proxy = consoleProxy;
            actual = setup;
        });

        // Assert
        this.testHelperOutput.WriteLine(consoleProxy.ToString());

        Assert.NotNull(actual);
        Assert.IsType<DummyModel>(actual.Model);
        Assert.Equal(0, exitCode);
    }

    /// <summary>
    /// Calling the model with invalid arguments gives a error code.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <param name="expectedCode">The expected code.</param>
    /// <param name="expectedMessage">The expected message.</param>
    [Theory]
    [InlineData("UnknownMethod",1000,"Unknown command")]
    [InlineData("DummyMethod -Timeout fds",1000,"Invalid argument format")]
    [InlineData("DummyMethod -unknownOption fds",1000,"Unknown option")]
    [InlineData("DummyMethod test",1000,"Too many arguments")]
    [InlineData("MethodWithArgs arg", 1000, "Invalid parameter")]
    [InlineData("MethodWithArgs", 1000, "Missing argument argument")]
    [InlineData("MethodWithArgs 10 arg", 1000, "Too many arguments")]
    public void CallingTheModelWithInvalidArgumentsGivesAErrorCode(string input, int expectedCode, string expectedMessage)
    {
        // Arrange
        string[] args = input.Split(" ");
        var consoleProxy = new TestProxy();

        // Act
        var exitCode = Controller.Run<DummyModel>(setup =>
        {
            setup.ArgumentsProvider = () => args;
            setup.Proxy = consoleProxy;
        });

        // Assert
        var actual = consoleProxy.ToString();
        this.testHelperOutput.WriteLine(actual);

        Assert.Contains(expectedMessage, actual);
        Assert.Equal(expectedCode, exitCode);
    }

    /// <summary>
    /// Exceptions the thrown in the model are reflected in the exit code.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <param name="expected">The expected.</param>
    [Theory]
    [InlineData("ArgumentException",1005)]
    [InlineData("AggregateException",1006)]
    [InlineData("NotImplementedException",1008)]
    [InlineData("OutOfMemoryException",1013)]
    [InlineData("Exception",1001)]
    public void ExceptionsThrownInTheModelAreReflectedInTheExitCode(string exception, int expected)
    {
        // Arrange
        string[] args = {"ThrowException", exception};
        var consoleProxy = new TestProxy();

        // Act
        var exitCode = Controller.Run<DummyModel>(setup =>
        {
            setup.ArgumentsProvider = () => args;
            setup.Proxy = consoleProxy;
        });

        // Assert
        var actual = consoleProxy.ToString();
        this.testHelperOutput.WriteLine(actual);

        Assert.Equal(expected, exitCode);
    }

    /// <summary>
    ///     Given a controller when calling a method then standard converters are used.
    /// </summary>
    [Fact]
    public void GivenAController_WhenCallingAMethod_ThenStandardConvertersAreUsed()
    {
        // Arrange
        var consoleProxy = new TestProxy();
        var setupCalled = false;
        var controller = new Controller(new MyClass(), consoleProxy, _ => setupCalled = true);
        controller.Run("Render", "true", "Some string", "12", "-Uppercase");

        // Act
        var actual = consoleProxy.ToString();

        // Assert
        Assert.Equal("TRUE : SOME STRING : 12", actual);
        Assert.True(setupCalled);
    }

    internal class DummyModel
    {
        public int Timeout { get; set; }

        public string DummyMethod()
        {
            return "";
        }

        public string MethodWithArgs(int argument)
        {
            return argument.ToString();
        }

        public void ThrowException(string exceptionName)
        {
            switch (exceptionName)
            {
                 case    "ArgumentException" : throw new ArgumentException("Test");
                 case    "AggregateException": throw new AggregateException();
                 case    "NotImplementedException": throw new NotImplementedException();
                 case    "OutOfMemoryException": throw new OutOfMemoryException();
                 case    "Exception": throw new ApplicationException();
            }
        }

        public string Help()
        {
            return "Overloaded help message";
        }
    }

    /// <summary>
    ///     Class MyClass.
    /// </summary>
    public class MyClass
    {
        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="MyClass" /> is uppercase.
        /// </summary>
        /// <value><c>true</c> if uppercase; otherwise, <c>false</c>.</value>
        public bool Uppercase { get; set; }

        /// <summary>
        ///     Testings the specified bool value.
        /// </summary>
        /// <param name="boolValue">if set to <c>true</c> [bool value].</param>
        /// <param name="stringValue">The string value.</param>
        /// <param name="intValue">The int value.</param>
        /// <returns>A string containing the values rendered as specified.</returns>
        public string Render(bool boolValue, string stringValue, int intValue)
        {
            if (this.Uppercase)
            {
                return $"{boolValue} : {stringValue} : {intValue}".ToUpper();
            }

            return $"{boolValue} : {stringValue} : {intValue}";
        }
    }
}
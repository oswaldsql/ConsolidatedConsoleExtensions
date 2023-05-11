## Unittest your console output

Injecting a test proxy provided with the test helper nuget you can test how your code reads and writes to the console.

```c#
private static void GreetAndAskForName(IConsoleProxy console)
{
	console.WriteLine("Welcome user.")
		.Write("What is your name? ")
		.ReadLine(out var name)
		.WriteLine()
		.WriteLine($"Welcome {name}")
		.ReadLine(out _);
}

[Fact]
public void GivenACallToGreet_WhenUserGivesName_ThenTheGreetingShouldBeRendered()
{
	// Arrange
	var testProxy = new TestProxy();
	testProxy.Keys.Add("Oswald\n\n");

	// Act
	GreetAndAskForName(testProxy);

	// Assert
	Assert.Equal("Welcome user.\nWhat is your name? \nWelcome Oswald\n", testProxy.ToString());
}
```

Not something you technically can't do with the existing console. But you can't [extend the functionality with extension methods](ExtendTheConsole.md)
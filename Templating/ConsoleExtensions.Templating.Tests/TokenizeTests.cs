namespace ConsoleExtensions.Templating.Tests;

using System;
using System.Linq;

using Token;

using Xunit;

public class TokenizeTests
{
  [Theory]
  [InlineData("RawText", "RawText")]
  [InlineData("Raw{user}Text", "Raw|sub:user|Text")]
  [InlineData("Raw{{user}Text", "Raw|{|user}Text")]
  [InlineData("Raw[style]Text", "Raw|com:style|Text")]
  [InlineData("Raw[[style]Text", "Raw|[|style]Text")]
  [InlineData("[command][command]", "com:command|com:command")]
  [InlineData("{sub}{sub}", "sub:sub|sub:sub")]
  [InlineData("[command]test[/command]", "com:command|test|endCom:command")]
  [InlineData("[[[[ }} {{{{ ]] ] }", "[|[| }} |{|{| ]] ] }")]
  public void TokenizerTests(string source, string expected)
  {
    var tokens = new Tokenizer().Tokenize(source).ToArray();

    var actual = string.Join('|', tokens.Select(t => t.ToString()));
    var tokenOptimizer = string.Join('|', TokenizerExtensions.Optimize(tokens).Select(t => t.ToString()));

    Assert.Equal(expected, actual);
    Console.Write($"{source} -> {actual} = {tokenOptimizer}");
  }
}
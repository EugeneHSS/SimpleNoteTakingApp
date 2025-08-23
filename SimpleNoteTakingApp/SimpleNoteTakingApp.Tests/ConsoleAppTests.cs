using Xunit;
using SimpleNoteTakingApp;
using SimpleNoteTakingApp.Tests.Mocks;
using SimpleNoteTakingApp.Core.ErrorHandling;

public class ConsoleAppTests
{
    [Fact]
    public void CallListCommand()
    {
        var fake = new MockManager();
        var app = ConsoleApp.CreateConsoleApp(() => fake);

        var res = app.ProcessLine("list");

        Assert.Contains(fake.Calls, c => c.Method == "View");
        Assert.Equal(SimpleNoteTakingApp.Core.ErrorHandling.ResultType.Ok, res._result);
    }

    [Fact]
    public void InvalidCommands()
    {
        var fake = new MockManager();
        var app = ConsoleApp.CreateConsoleApp(() => fake);

        var res = app.ProcessLine("wat");

        Assert.Equal(SimpleNoteTakingApp.Core.ErrorHandling.ResultType.InvalidInput, res._result);
        Assert.Contains("Invalid command", res._resultMessage);
    }

    [Fact]
    public void InvalidCommand_IsInvalidInput()
    {
        var fake = new MockManager();
        var app = ConsoleApp.CreateConsoleApp(() => fake);

        var res = app.ProcessLine("wat");

        Assert.Equal(ResultType.InvalidInput, res._result);
        Assert.Contains("Invalid command", res._resultMessage);
    }

    [Theory]
    [InlineData("get 42", "Get", new[] { "42" })]
    [InlineData("view 42", "Get", new[] { "42" })]
    [InlineData("del 42", "Remove", new[] { "42" })]
    [InlineData("delete 42", "Remove", new[] { "42" })]
    public void SingleArgumentCommands(string line, string expectedMethod, string[] expectedArgs)
    {
        var fake = new MockManager();
        var app = ConsoleApp.CreateConsoleApp(() => fake);

        var res = app.ProcessLine(line);

        Assert.Equal(ResultType.Ok, res._result);
        Assert.Contains(fake.Calls, c => c.Method == expectedMethod && c.Args.SequenceEqual(expectedArgs));
    }

    [Theory]
    [InlineData("get")]
    [InlineData(@"get Hello World")]
    [InlineData("delete")]
    public void SingleArgumentCommands_Invalid(string line)
    {
        var app = ConsoleApp.CreateConsoleApp(() => new MockManager());

        var res = app.ProcessLine(line);

        Assert.Equal(ResultType.InvalidInput, res._result);
    }

    [Fact]
    public void SingleArgumentCommands_Quoted()
    {
        var fake = new MockManager();
        var app = ConsoleApp.CreateConsoleApp(() => fake);

        var res = app.ProcessLine(@"get ""Hello My Name Is Jean""");

        Assert.Equal(ResultType.Ok, res._result);
        Assert.Contains(fake.Calls, c => c.Method == "Get" && c.Args.SequenceEqual(new[] { "Hello My Name Is Jean" }));
    }

    [Theory]
    [InlineData(@"add ""Shopping List"" milk eggs bread", "Add", "Shopping List", "milk eggs bread")]
    [InlineData(@"add ""T"" ""C with spaces""", "Add", "T", "C with spaces")]
    [InlineData(@"edit 42 ""New content with spaces""", "Edit", "42", "New content with spaces")]
    [InlineData(@"edit 42 quick fix", "Edit", "42", "quick fix")]
    public void MultiArgumentsCommand_Quoted(string line, string expectedMethod, string first, string second)
    {
        var fake = new MockManager();
        var app = ConsoleApp.CreateConsoleApp(() => fake);

        var res = app.ProcessLine(line);

        Assert.Equal(ResultType.Ok, res._result);
        Assert.Contains(fake.Calls, c =>
            c.Method == expectedMethod &&
            c.Args.Count == 2 &&
            c.Args[0] == first &&
            c.Args[1] == second);
    }

    [Theory]
    [InlineData(@"add ""OnlyTitle""")]
    [InlineData(@"edit 42")]
    public void MultiArgumentsCommand_Invalid(string line)
    {
        var app = ConsoleApp.CreateConsoleApp(() => new MockManager());

        var res = app.ProcessLine(line);

        Assert.Equal(ResultType.InvalidInput, res._result);
    }

    [Theory]
    [InlineData(@"search spaced query terms", "spaced query terms")]
    [InlineData(@"search ""already joined""", "already joined")]
    public void MultiArgumentCommands_Unquoted(string line, string expectedQuery)
    {
        var fake = new MockManager();
        var app = ConsoleApp.CreateConsoleApp(() => fake);

        var res = app.ProcessLine(line);

        Assert.Equal(ResultType.Ok, res._result);
        Assert.Contains(fake.Calls, c => c.Method == "Search" && c.Args.SequenceEqual(new[] { expectedQuery }));
    }


}

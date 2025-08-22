using Xunit;
using SimpleNoteTakingApp;
using SimpleNoteTakingApp.Tests.Mocks;

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
    public void GetCommandWithArguments()
    {
        var fake = new MockManager();
        var app = ConsoleApp.CreateConsoleApp(() => fake);

        var res = app.ProcessLine("get 42");

        Assert.Contains(fake.Calls, c => c.Method == "Get" && c.Arg == "42");
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
}

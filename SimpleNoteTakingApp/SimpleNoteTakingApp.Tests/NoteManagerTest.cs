using Xunit;
using SimpleNoteTakingApp;
using SimpleNoteTakingApp.Core;
using SimpleNoteTakingApp.Core.ErrorHandling;

public class ConsoleAppIntegrationSmolTests
{
    private static ConsoleApp App() => ConsoleApp.CreateConsoleApp(() => new NoteManager());

    [Fact]
    public void AddThenGet()
    {
        var app = App();

        var add = app.ProcessLine(@"add ""Shopping List"" ""eggs, milk""");
        Assert.Equal(ResultType.Ok, add._result);

        var get = app.ProcessLine(@"get ""Shopping List""");
        Assert.Equal(ResultType.Ok, get._result);
        Assert.Contains("Title: Shopping List", get._resultMessage);
        Assert.Contains("eggs, milk", get._resultMessage);
    }

    [Fact]
    public void ListTitles()
    {
        var app = App();
        app.ProcessLine(@"add ""A"" x");
        app.ProcessLine(@"add ""B"" y");

        var list = app.ProcessLine("list");

        Assert.Equal(ResultType.Ok, list._result);

        Assert.Contains("+----------------------+", list._resultMessage);
        Assert.Contains("| Title", list._resultMessage);
        Assert.Contains("| Content", list._resultMessage);

    }


    [Fact]
    public void EditAndDelete()
    {
        var app = App();
        app.ProcessLine(@"add ""Doc"" v1");

        var edit = app.ProcessLine(@"edit ""Doc"" ""v2""");
        Assert.Equal(ResultType.Ok, edit._result);

        var del = app.ProcessLine(@"delete ""Doc""");
        Assert.Equal(ResultType.Ok, del._result);

        var get = app.ProcessLine(@"get ""Doc""");
        Assert.Equal(ResultType.InvalidInput, get._result);
    }

    [Fact]
    public void SearchByTitleAndContent()
    {
        var app = App();
        app.ProcessLine(@"add ""Alpha"" ""status report""");
        app.ProcessLine(@"add ""Report Draft"" ""contains report keyword""");
        app.ProcessLine(@"add ""Notes"" ""misc""");

        var search = app.ProcessLine(@"search report");
        Assert.Equal(ResultType.Ok, search._result);
        Assert.Contains("Alpha", search._resultMessage);
        Assert.Contains("Report Draft", search._resultMessage);
        Assert.DoesNotContain("Notes\n", search._resultMessage);
    }

    [Fact]
    public void AddAllowsUnquotedContent()
    {
        var app = App();
        var add = app.ProcessLine(@"add ""Work"" finish quarterly report");
        Assert.Equal(ResultType.Ok, add._result);

        var get = app.ProcessLine(@"get ""Work""");
        Assert.Equal(ResultType.Ok, get._result);
        Assert.Contains("finish quarterly report", get._resultMessage);
    }

    [Theory]
    [InlineData("")]                
    [InlineData("   ")]            
    [InlineData("wat")]              
    [InlineData("get")]           
    [InlineData(@"get Hello World")] 
    [InlineData("search")]           
    public void InvalidInputsReturn(string line)
    {
        var app = App();
        var res = app.ProcessLine(line);
        Assert.Equal(ResultType.InvalidInput, res._result);
    }
}

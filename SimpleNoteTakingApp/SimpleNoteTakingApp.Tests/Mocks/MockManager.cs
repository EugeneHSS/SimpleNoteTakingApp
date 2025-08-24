using SimpleNoteTakingApp.Core;
using SimpleNoteTakingApp.Core.ErrorHandling;

namespace SimpleNoteTakingApp.Tests.Mocks
{
    public class MockManager : IManager
    {
        public record Call(string Method, IReadOnlyList<string> Args);
        public List<Call> Calls { get; } = new();
        
        public INoteResult Add(IReadOnlyList<string> args) 
        { 
            Calls.Add(new("Add", args)); 
            return NoteResult.Ok("Added (fake)."); 
        }
        public INoteResult Remove(IReadOnlyList<string> args)
        {
            Calls.Add(new("Remove", args));
            return NoteResult.Ok("Removed (fake).");
        }
        public INoteResult Get(IReadOnlyList<string> args) 
        { 
            Calls.Add(new("Get", args)); 
            return NoteResult.Ok($"Got {args} (fake)."); 
        }
        public INoteResult View() 
        { 
            Calls.Add(new("View", Array.Empty<string>())); 
            return NoteResult.Ok("Listed (fake)."); 
        }
        public INoteResult Edit(IReadOnlyList<string> args) 
        {
            Calls.Add(new("Edit", args)); 
            return NoteResult.Ok("Edited (fake).");
        }
        public INoteResult Search(IReadOnlyList<string> args) 
        {
            Calls.Add(new("Search", args)); 
            return NoteResult.Ok("Searched (fake).");
        }


    }
}

using System.Collections.Generic;
using SimpleNoteTakingApp.Core;
using SimpleNoteTakingApp.Core.ErrorHandling;
namespace SimpleNoteTakingApp.Tests.Mocks
{
    public class MockManager : IManager
    {
        public record Call(string Method, string Arg);
        public List<Call> Calls { get; } = new();
        
        public INoteResult Add(string args) 
        { 
            Calls.Add(new("Add", args)); return NoteResult.Ok("Added (fake)."); 
        }
        public INoteResult Remove(string args)
        {
            Calls.Add(new("Remove", args));
            return NoteResult.Ok("Removed (fake).");
        }
        public INoteResult Get(string args) 
        { 
            Calls.Add(new("Get", args)); 
            return NoteResult.Ok($"Got {args} (fake)."); 
        }
        public INoteResult View() 
        { 
            Calls.Add(new("View", "")); 
            return NoteResult.Ok("Listed (fake)."); 
        }
        public INoteResult Edit(string args) 
        {
            Calls.Add(new("Edit", args)); 
            return NoteResult.Ok("Edited (fake).");
        }
        public INoteResult Search(string args) 
        {
            Calls.Add(new("Search", args)); 
            return NoteResult.Ok("Searched (fake).");
        }


    }
}

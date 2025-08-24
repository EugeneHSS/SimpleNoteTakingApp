namespace SimpleNoteTakingApp.Core.ErrorHandling
{
    public class NoteResult : INoteResult
    {
        public ResultType _result {  get; init; }
        public string? _resultMessage { get; init; }

        public static NoteResult Ok(string? message = null) => new()
        {
            _result = ResultType.Ok,
            _resultMessage = message
        };

        public static NoteResult NotFound(string msg = "Not found.") => new()
        {
            _result = ResultType.NotFound,
            _resultMessage = msg
        };

        public static NoteResult Invalid(string msg) => new()
        {
            _result = ResultType.InvalidInput,
            _resultMessage = msg
        };

        public static NoteResult Error(string msg) => new() 
        {
            _result = ResultType.Error,
            _resultMessage = msg 
        };

    }
}

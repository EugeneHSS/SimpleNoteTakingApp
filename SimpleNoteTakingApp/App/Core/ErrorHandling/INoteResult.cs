using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNoteTakingApp.Core.ErrorHandling
{
    public enum ResultType
    {
        Ok, NotFound, InvalidInput, Error
    }

    public interface INoteResult
    {
        ResultType _result { get; }
        string? _resultMessage { get; }
    }


}


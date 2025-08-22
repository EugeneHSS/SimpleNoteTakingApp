using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleNoteTakingApp.Core.ErrorHandling;

namespace SimpleNoteTakingApp.Core
{
    public interface IManager
    {
        INoteResult Add(string Args);

        INoteResult Remove(string Args);

        INoteResult Get(string Args);

        INoteResult View();

        INoteResult Edit(string Args);

        INoteResult Search(string args);
    }
}

using SimpleNoteTakingApp.Core.ErrorHandling;

namespace SimpleNoteTakingApp.Core
{
    public interface IManager
    {
        INoteResult Add(IReadOnlyList<string> args);

        INoteResult Remove(IReadOnlyList<string> args);

        INoteResult Get(IReadOnlyList<string> args);

        INoteResult View();

        INoteResult Edit(IReadOnlyList<string> args);

        INoteResult Search(IReadOnlyList<string> args);
    }
}

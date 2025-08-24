using SimpleNoteTakingApp.Core;

namespace SimpleNoteTakingApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var app = ConsoleApp.CreateConsoleApp<NoteManager>();
            app.Run();
        }
    }
}

using System;
using SimpleNoteTakingApp.Core;
using SimpleNoteTakingApp.Core.ErrorHandling;

namespace SimpleNoteTakingApp
{
    public class ConsoleApp
    {
        private bool _IsRunning;
        private readonly IManager _MgrInst;

        public static ConsoleApp CreateConsoleApp<TManager>() where TManager : IManager, new()
            => new ConsoleApp(new TManager());

        public static ConsoleApp CreateConsoleApp(Func<IManager> factory)
            => new ConsoleApp(factory?.Invoke() ?? throw new ArgumentNullException(nameof(factory)));

        private ConsoleApp(IManager manager)
        {
            _MgrInst = manager ?? throw new ArgumentNullException(nameof(manager));
            _IsRunning = true;
        }

        public void Shutdown() => _IsRunning = false;

        //TODO@ make private, dont expose API
        public INoteResult ProcessLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return NoteResult.Invalid("Empty command.");

            var (cmd, arg) = Parse(line);

            try
            {
                return cmd switch
                {
                    "help" => PrintHelpAndReturn(),
                    "list" => _MgrInst.View(),
                    "get" or "view" => _MgrInst.Get(arg),
                    "del" or "delete" => _MgrInst.Remove(arg),
                    "add" => _MgrInst.Add(arg),
                    "edit" => _MgrInst.Edit(arg),
                    "search" => _MgrInst.Search(arg),
                    "quit" or "exit" => ExitAndReturn(),
                    _ => NoteResult.Invalid("Invalid command. Type 'help' for help.")
                };
            }
            catch (Exception ex)
            {
                return NoteResult.Error($"Unexpected error: {ex.Message}");
            }
        }

        public void Run()
        {
            PrintWelcome();

            while (_IsRunning)
            {
                Console.Write("\n> ");
                var line = Console.ReadLine();

                if (line is null)
                {
                    Console.WriteLine();
                    break;
                }

                var res = ProcessLine(line);
                Render(res);
            }

            Console.WriteLine("Exiting...");
        }

        private static (string cmd, string arg) Parse(string line)
        {
            var token = line.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            var cmd = token.Length > 0 ? token[0].ToLowerInvariant() : "";
            var arg = token.Length > 1 ? token[1] : "";
            return (cmd, arg);
        }

        private static INoteResult PrintHelpAndReturn()
        {
            PrintHelp();
            return NoteResult.Ok();
        }

        private INoteResult ExitAndReturn()
        {
            _IsRunning = false;
            return NoteResult.Ok();
        }

        private static void Render(INoteResult r)
        {
            if (!string.IsNullOrWhiteSpace(r._resultMessage))
                Console.WriteLine(r._resultMessage);
        }

        private static void PrintWelcome()
        {
            Console.WriteLine("Simple Note Tracker App (Console)");
            PrintHelp();
        }

        private static void PrintHelp()
        {
            Console.WriteLine(@"
                Commands:
                  add               -> create a note
                  list              -> list notes
                  get <id>          -> view a note
                  del <id>          -> delete a note
                  edit <id>         -> edit a note
                  search <text>     -> search notes
                  help              -> show help
                  quit | exit       -> exit");
        }
    }
}

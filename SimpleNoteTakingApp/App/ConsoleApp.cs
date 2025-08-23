using System;
using SimpleNoteTakingApp.Core;
using SimpleNoteTakingApp.Core.ErrorHandling;

namespace SimpleNoteTakingApp
{
    public class ConsoleApp
    {
        private bool _IsRunning;
        private readonly IManager _MgrInst;

        private static readonly (string Syntax, string Description)[] HelpCommands =
        {
            ("add \"<title>\" \"<content>\"", "create a note (quotes allow spaces; content can be unquoted)"),
            ("list", "list notes"),
            ("get|view \"<title>\"", "view a note"),
            ("del|delete \"<title>\"", "delete a note"),
            ("edit \"<title>\" \"<content>\"", "edit a note (content can be unquoted)"),
            ("search \"<text>\"", "search notes"),
            ("help", "show help"),
            ("quit | exit", "exit"),
        };


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

        internal INoteResult ProcessLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return NoteResult.Invalid("Empty command.");

            var tokens = CommandParser.Tokenize(line);
            if (tokens.Count == 0)
                return NoteResult.Invalid("Empty command.");

            var cmd = tokens[0].ToLowerInvariant();
            var args = tokens.Skip(1).ToArray();

            try
            {
                switch (cmd)
                {
                    case "help":
                        return PrintHelpAndReturn();

                    case "list":
                        return _MgrInst.View();

                    case "get":
                    case "view":
                        if (args.Length != 1)
                            return NoteResult.Invalid(@"Usage: view ""<iTitle>""");
                        return _MgrInst.Get(args);

                    case "del":
                    case "delete":
                        if (args.Length != 1)
                            return NoteResult.Invalid(@"Usage: delete ""<Title>""");
                        return _MgrInst.Remove(args);

                    case "add":
                        if (args.Length < 2)
                            return NoteResult.Invalid(@"Usage: add ""<title>"" ""<content>""");
                        return _MgrInst.Add(new[] { args[0], string.Join(' ', args.Skip(1)) });

                    case "edit":
                        if (args.Length < 2)
                            return NoteResult.Invalid(@"Usage: edit ""<Title>"" ""<new content>""");
                        return _MgrInst.Edit(new[] { args[0], string.Join(' ', args.Skip(1)) });

                    case "search":
                        if (args.Length < 1)
                            return NoteResult.Invalid(@"Usage: search ""<text>""");
                        return _MgrInst.Search(new[] { string.Join(' ', args) });

                    case "quit":
                    case "exit":
                        return ExitAndReturn();

                    default:
                        return NoteResult.Invalid("Invalid command. Type 'help' for help.");
                }
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

        private static (string cmd, List<string> args) Parse(string line)
        {
            var tokenList = CommandParser.Tokenize(line);
            if (tokenList.Count == 0)
            {
                return ("", new List<string>());
            }

            var cmd = tokenList[0].ToLowerInvariant();
            var args = tokenList.Skip(1).ToList();
            return (cmd, args);
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
            if (string.IsNullOrWhiteSpace(r._resultMessage))
            {
                return;
            }
            switch (r._result)
            {
                case ResultType.Ok:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(r._resultMessage);
                    break;

                case ResultType.InvalidInput:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[Invalid]  " + r._resultMessage);
                    break;

                case ResultType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Error] " + r._resultMessage);
                    break;
            }

            Console.ResetColor();
        }

        private static void PrintWelcome()
        {
            Console.WriteLine("Simple Note Tracker App (Console)");
            PrintHelp();
        }

        private static void PrintHelp()
        {
            Console.WriteLine("\nCommands:");
            int pad = HelpCommands.Max(c => c.Syntax.Length) + 2;
            foreach (var (syntax, desc) in HelpCommands)
                Console.WriteLine($"  {syntax.PadRight(pad)} -> {desc}");
        }

    }
}

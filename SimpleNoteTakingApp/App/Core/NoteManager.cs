using System;
using System.Collections.Generic;
using System.Linq;
using SimpleNoteTakingApp.Core;
using SimpleNoteTakingApp.Core.ErrorHandling;

namespace SimpleNoteTakingApp.Core
{
    public class NoteManager : IManager
    {
        private sealed class Note
        {
            public string Title { get; set; } = "";
            public string Content { get; set; } = "";
            public DateTime CreatedAt { get; init; }
            public DateTime UpdatedAt { get; set; }
        }

        private readonly List<Note> _notes = new();

        public INoteResult Add(IReadOnlyList<string> args)
        {
            if (args.Count != 2)
            {
                return NoteResult.Invalid(@"Usage: add ""<title>"" ""<content>""");
            }

            var title = args[0]?.Trim() ?? "";
            var content = args[1] ?? "";

            if (string.IsNullOrWhiteSpace(title))
            {
                return NoteResult.Invalid("Title cannot be empty.");
            }

            if (_notes.Any(n => string.Equals(n.Title, title, StringComparison.OrdinalIgnoreCase)))
            {
                return NoteResult.Invalid($"A note titled \"{title}\" already exists.");
            }

            var now = DateTime.Now;
            var note = new Note
            {
                Title = title,
                Content = content,
                CreatedAt = now,
                UpdatedAt = now
            };
            _notes.Add(note);

            return NoteResult.Ok($"Added note: \"{note.Title}\"");
        }

        public INoteResult View()
        {
            if (_notes.Count == 0)
            {
                return NoteResult.Ok("No notes yet.");
            }

            var lines = _notes
                .OrderBy(n => n.Title)
                .Select(n => n.Title);

            return NoteResult.Ok("Notes:\n" + string.Join("\n", lines));
        }

        public INoteResult Get(IReadOnlyList<string> args)
        {
            if (args.Count != 1)
            {
                return NoteResult.Invalid(@"Usage: view ""<title>""");
            }

            var key = args[0] ?? "";
            var note = FindByTitle(key);

            if (note is null)
            {
                return NoteResult.Invalid($"Note not found: {key}");
            }

            var body = $@"Title: {note.Title}
                        Created: {note.CreatedAt:G}
                        Updated: {note.UpdatedAt:G}
                        {note.Content}";

            return NoteResult.Ok(body);
        }

        public INoteResult Remove(IReadOnlyList<string> args)
        {
            if (args.Count != 1)
            {
                return NoteResult.Invalid(@"Usage: delete ""<title>""");
            }

            var key = args[0] ?? "";
            var note = FindByTitle(key);

            if (note is null)
            {
                return NoteResult.Invalid($"Note not found: {key}");
            }

            _notes.Remove(note);
            return NoteResult.Ok($"Deleted note: \"{note.Title}\"");
        }

        public INoteResult Edit(IReadOnlyList<string> args)
        {
            if (args.Count != 2)
            {
                return NoteResult.Invalid(@"Usage: edit ""<title>"" ""<new content>""");
            }

            var key = args[0] ?? "";
            var newContent = args[1] ?? "";

            var note = FindByTitle(key);

            if (note is null)
            {
                return NoteResult.Invalid($"Note not found: {key}");
            }

            note.Content = newContent;
            note.UpdatedAt = DateTime.Now;
            return NoteResult.Ok($"Edited note: \"{note.Title}\"");
        }

        public INoteResult Search(IReadOnlyList<string> args)
        {
            if (args.Count != 1)
            {
                return NoteResult.Invalid(@"Usage: search ""<text>""");
            }

            var q = (args[0] ?? "").Trim();

            if (q.Length == 0)
            {
                return NoteResult.Invalid("Search text cannot be empty.");
            }

            var hits = _notes
                .Where(n => n.Title.Contains(q, StringComparison.OrdinalIgnoreCase) || n.Content.Contains(q, StringComparison.OrdinalIgnoreCase))
                .OrderBy(n => n.Title)
                .ToList();

            if (hits.Count == 0)
            { 
                return NoteResult.Ok("No matches.");
            }

            var lines = hits.Select(n => n.Title);
            return NoteResult.Ok("Matches:\n" + string.Join("\n", lines));
        }

        private Note? FindByTitle(string title) => _notes.FirstOrDefault(n => string.Equals(n.Title, title, StringComparison.OrdinalIgnoreCase));
    }
}

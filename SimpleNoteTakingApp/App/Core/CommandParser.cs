using System.Text.RegularExpressions;

namespace SimpleNoteTakingApp.Core
{
    internal static class CommandParser
    {
        private static readonly Regex _argRegex = new Regex(@"
            \s*
            (?:
                ""((?:\\.|[^""])*)""
              | (\S+)        
            )",
            RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace | RegexOptions.CultureInvariant);

        public static List<string> Tokenize(string input)
        {
            var tokenList = new List<string>();

            foreach (Match m in _argRegex.Matches(input))
            {
                var token = m.Groups[1].Success ? m.Groups[1].Value : m.Groups[2].Value;
                if (m.Groups[1].Success)
                {
                    token = token.Replace("\\\"", "\"").Replace("\\\\", "\\");
                }
                tokenList.Add(token);
            }

            return tokenList;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class StringExtensions
    {
        public static string ToQuoteBox(this string text, string? author = null)
        {
            var lines = text.Replace("\r\n", "\n").Split('\n');
            var splitLines = new List<string>();
            foreach (var line in lines)
            {
                var current = line;
                while (current.Length > 50)
                {
                    int lastSpace = current.LastIndexOf(' ', 50);
                    if (lastSpace <= 0) lastSpace = 50; // если нет пробела, рубим по 50
                    splitLines.Add(current.Substring(0, lastSpace));
                    current = current.Substring(lastSpace).TrimStart();
                }
                splitLines.Add(current);
            }
            var formattedLines = splitLines.Select(l => "# `" + l + "`").ToList();
            int maxLen = splitLines.Max(l => l.Length);
            var frameLen = Math.Max(maxLen, 7);
            string top = $"# `╔═ 📃 {new string('═', frameLen - 7)}╗`";
            string bottom = $"# `╚{new string('═', frameLen - 7)} 📃 ═╝`";

            var result = new StringBuilder();
            result.AppendLine(top);
            foreach (var l in formattedLines)
                result.AppendLine(l.PadRight(maxLen + 4)); // +4 для # и ``
            result.AppendLine(bottom);

            if (!string.IsNullOrWhiteSpace(author))
                result.AppendLine($"-# ||автор: {author}||");

            return result.ToString();
        }
    }
}

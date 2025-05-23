using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Wondeluxe
{
	public static class StringExtensions
	{
		public static string ToPascal(this string str)
		{
			StringBuilder builder = new();

			bool upper = true;

			foreach (char ch in str)
			{
				if (char.IsLetter(ch))
				{
					builder.Append(upper ? char.ToUpper(ch) : ch);
					upper = false;
					continue;
				}

				if (char.IsDigit(ch) || ch == '_')
				{
					builder.Append(ch);
				}

				upper = true;
			}

			return builder.ToString();
		}

		public static string InterpretIndentation(this string str, out int level)
		{
			level = 0;

			// Identify indentation sequence (tab or spaces).

			string regex = @"^(\t| +)";

			Match match = Regex.Match(str, regex, RegexOptions.Multiline);

			if (!match.Success)
			{
				// No lines open with whitespace, unable to determine indentation.
				return null;
			}

			string indentation = match.Groups[1].Value;

			// Find indentation on last line.

			regex = @"\n?((#INDENTATION#)+)(.*)(\n*)$".Replace("#INDENTATION#", indentation);

			match = Regex.Match(str, regex);

			if (!match.Success)
			{
				// Last line isn't indented.
				return indentation;
			}

			// Get current level.

			string lastLine = match.Groups[1].Value;

			for (int i = 0; i < lastLine.Length && lastLine.IndexOf(indentation, i) == i; i += indentation.Length)
			{
				level++;
			}

			// Increment level if string ends with new line and has an open bracket.

			if (!match.Groups[3].Success || !match.Groups[4].Success)
			{
				return indentation;
			}

			lastLine = match.Groups[3].Value;

			if (lastLine.IsInsideBrackets())
			{
				level++;
			}

			return indentation;
		}

		public static bool IsInsideBrackets(this string str)
		{
			Dictionary<char, int> openBrackets = new()
			{
				{ '{', 0 },
				{ '[', 0 },
				{ '(', 0 }
			};

			int openBracketCount = 0;

			Dictionary<char, char> matchingOpenBrackets = new()
			{
				{ ')', '(' },
				{ ']', '[' },
				{ '}', '{' }
			};

			foreach (char ch in str)
			{
				if (openBrackets.ContainsKey(ch))
				{
					openBrackets[ch]++;
					openBracketCount++;
				}
				else if (matchingOpenBrackets.TryGetValue(ch, out char br) && openBrackets[ch] > 0)
				{
					openBrackets[br]--;
					openBracketCount--;
				}
			}

			return openBracketCount > 0;
		}
	}
}
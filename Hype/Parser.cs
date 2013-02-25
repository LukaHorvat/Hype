using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hype
{
	class Parser
	{
		public List<char> OpenBrackets = new List<char>() { '(', '{', '[', };
		public List<char> ClosedBrackets = new List<char>() { ')', '}', ']' };
		public List<char> SpecialNonSeparatedChars = new List<char>() { '.', ';' };

		private List<char> brackets;

		public Parser()
		{
			brackets = OpenBrackets.Concat(ClosedBrackets).ToList();
		}

		public List<string> Split(string source)
		{
			/* 
			 * Every separate token has to be spaced out except for some special cases
			 * The spaces are added around those separately
			 */

			//First, we process strings
			int start, end, count = 0;
			string str, id;
			var lookup = new Dictionary<string, string>();
			while ((start = source.IndexOf('"')) != -1)
			{
				if ((end = source.IndexOf('"', start + 1)) != -1)
				{
					str = source.Substring(start, end - start + 1);
					lookup.Add(id = ("strlookup#" + count), str);
					source = source.Remove(start, end - start + 1).Insert(start, id);
					count++;
				}
				else
				{
					throw new Exception("Mismatched quotes");
				}
			}

			var nonSeparated = OpenBrackets.Concat(ClosedBrackets).Concat(SpecialNonSeparatedChars);

			StringBuilder builder = new StringBuilder(source);
			int index = 0;
			while ((index = builder.ToString().IndexOfAny(nonSeparated.ToArray(), index)) != -1)
			{
				builder.Insert(index + 1, ' ');
				builder.Insert(index, ' ');
				index += 2;
			}
			return builder.ToString().Split(new[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(s => (s.Length > 10 && s.Substring(0, 10) == "strlookup#") ? lookup[s] : s).ToList();
		}

		public List<Token> Tokenize(List<string> words)
		{
			words = words.Select(w => w.Trim(new[] { ' ', '\t', '\r', '\n' })).ToList();
			words.RemoveAll(s => s == "");

			var list = new List<Token>();
			var isNumber = new Regex("[0-9]+");
			var type = TokenType.Identifier;
			foreach (var word in words)
			{
				type = TokenType.Identifier;

				if (word.Length == 1 && (brackets.Contains(word[0])))
				{
					type = TokenType.Group;
				}
				else if (isNumber.IsMatch(word))
				{
					type = TokenType.Literal;
				}
				else if (new[] { "true", "false" }.Contains(word))
				{
					type = TokenType.Literal;
				}
				else if (word == ";")
				{
					type = TokenType.Separator;
				}
				else if (word[0] == '"')
				{
					type = TokenType.Literal;
				}

				list.Add(new Token(type, word));
			}

			list.Insert(0, new Token(TokenType.Group, "("));
			list.Add(new Token(TokenType.Group, ")"));

			return list;
		}

		public Expression BuildExpressionTree(List<Token> tokens, int startIndex)
		{
			var res = new Expression(tokens[startIndex]);

			int currentIndex = startIndex + 1;
			while (tokens.Count > currentIndex && !IsClosedBracket(tokens[currentIndex]))
			{
				if (IsOpenBracket(tokens[currentIndex]))
				{
					var subExpression = BuildExpressionTree(tokens, currentIndex);
					if (subExpression.Sequence.Count != 0) res.Sequence.Add(subExpression);
					currentIndex += subExpression.GetNumTokens() - 1;
				}
				else res.Sequence.Add(new ExpressionItem(tokens[currentIndex]));

				currentIndex++;
			}
			res.ParseNodes();
			return res;
		}

		private bool IsClosedBracket(Token t)
		{
			if (t.Type == TokenType.Group) return ClosedBrackets.Contains(t.Content[0]);
			return false;
		}

		private bool IsOpenBracket(Token t)
		{
			if (t.Type == TokenType.Group) return OpenBrackets.Contains(t.Content[0]);
			return false;
		}
	}
}

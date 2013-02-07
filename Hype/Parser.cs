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

			var nonSeparated = OpenBrackets.Concat(ClosedBrackets).Concat(SpecialNonSeparatedChars);

			StringBuilder builder = new StringBuilder(source);
			int index = 0;
			while ((index = builder.ToString().IndexOfAny(nonSeparated.ToArray(), index)) != -1)
			{
				builder.Insert(index + 1, ' ');
				builder.Insert(index, ' ');
				index += 2;
			}
			return builder.ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
		}

		public List<Token> Tokenize(List<string> words)
		{
			words = words.Select(w => w.Trim(new[] { ' ', '\t', '\r', '\n' })).ToList();

			var list = new List<Token>();
			var isNumber = new Regex("[0-9]+");
			var type = TokenType.Identifier;
			foreach (var word in words)
			{
				type = TokenType.Identifier;

				if (word.Length == 1 && (brackets.Contains(word[0]) || word[0] == ';'))
				{
					type = TokenType.Group;
				}
				else if (isNumber.IsMatch(word))
				{
					type = TokenType.Literal;
				}

				list.Add(new Token(type, word));
			}

			list.Add(null);
			SplitExpressions(list, list.Count - 1);

			return list;
		}

		public int SplitExpressions(List<Token> list, int index)
		{
			int level = 0;
			list[index] = new Token(TokenType.Group, ")");
			for (int i = index - 1; i >= 0; --i)
			{
				if (list[i].Type == TokenType.Group)
				{
					if (ClosedBrackets.Contains(list[i].Content[0])) level++;
					else if (OpenBrackets.Contains(list[i].Content[0])) level--;
					else if (list[i].Content == ";")
					{
						if (level != 0)
						{
							int skip = SplitExpressions(list, i);
							i = skip + 1;
						}
						else
						{
							list.Insert(i + 1, new Token(TokenType.Group, "("));
							list[i] = new Token(TokenType.Group, ")");
						}
						continue;
					}
					if (level == -1)
					{
						list.Insert(i + 1, new Token(TokenType.Group, "("));
						return i;
					}
				}
			}
			list.Insert(0, new Token(TokenType.Group, "("));
			list.Insert(0, new Token(TokenType.Group, "{"));
			list.Add(new Token(TokenType.Group, "}"));
			return 0;
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

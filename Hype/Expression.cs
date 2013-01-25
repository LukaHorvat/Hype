using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hype
{
	class Expression : ExpressionItem
	{
		public List<ExpressionItem> Sequence;

		public Expression(Token bracketToken)
			: base(CheckToken(bracketToken))
		{
			Sequence = new List<ExpressionItem>();
		}

		private static Token CheckToken(Token token)
		{
			if (token.Type == TokenType.Group) return token;
			throw new Exception("You must provide a group token to create a new expression");
		}

		public override string ToString()
		{
			return "Expression: " + OriginalToken;
		}

		public void DebugPrint(int tabLevel = 0)
		{
			PrintTabs(tabLevel);
			foreach (var item in Sequence)
			{
				if (item is Expression)
				{
					Console.WriteLine();
					(item as Expression).DebugPrint(tabLevel + 1);
					Console.WriteLine();
				}
				else
				{
					Console.Write(item.OriginalToken.Content + " ");
				}
			}
		}

		private void PrintTabs(int n)
		{
			for (int i = 0; i < n; ++i) Console.Write('\t');
		}
	}
}

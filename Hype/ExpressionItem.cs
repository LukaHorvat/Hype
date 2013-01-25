using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hype
{
	class ExpressionItem
	{
		public Token OriginalToken;

		public ExpressionItem(Token token)
		{
			OriginalToken = token;
		}

		public override string ToString()
		{
			return "Item: " + OriginalToken;
		}
	}
}

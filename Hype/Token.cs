using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hype
{
	enum TokenType
	{
		Group,
		Identifier,
		Literal,
		Separator
	}

	class Token
	{
		public TokenType Type;
		public string Content;

		public Token(TokenType type, string content)
		{
			if (type == TokenType.Group && content.Length > 1) throw new Exception("Group tokens can only consist of one character");
			Type = type;
			Content = content;
		}

		public override string ToString()
		{
			return Type + "[\"" + Content + "\"]";
		}
	}
}

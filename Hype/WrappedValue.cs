using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	/// <summary>
	/// A value that has already been evaluated from an expression item but needs to return to the rpn list
	/// </summary>
	class WrappedValue : ExpressionItem
	{
		public Value Val;

		public WrappedValue(Value val)
			:base(new Token(TokenType.Identifier, val.Var.Name))
		{
			Val = val;
		}		
	}
}

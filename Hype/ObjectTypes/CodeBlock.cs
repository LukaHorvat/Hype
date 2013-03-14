using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class CodeBlock : Value
	{
		public Expression Expression;

		public CodeBlock(Expression expression)
			: base(ValueType.CodeBlock)
		{
			Expression = expression;
		}

		public Value Execute(Interpreter interpreter)
		{
			return Expression.Execute(interpreter);
		}
	}
}

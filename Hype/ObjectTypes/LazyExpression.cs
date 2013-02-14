using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class LazyExpression : Value
	{
		public ExpressionItem Item;

		public LazyExpression(ExpressionItem item)
			:base(ValueType.GetType("LazyExpression"))
		{
			Item = item;
		}
	}
}

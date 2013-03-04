using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class ExpressionCache : LookupCache
	{
		private Expression exp;
		private Interpreter interpreter;

		public override Value Cache
		{
			get
			{
				return exp.Execute(interpreter);
			}
			set
			{
			}
		}

		public ExpressionCache(Expression exp, Interpreter interpreter)
			:base(Void.Instance)
		{
			this.exp = exp;
			this.interpreter = interpreter;
		}
	}
}

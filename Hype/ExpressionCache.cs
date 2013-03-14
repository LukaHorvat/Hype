using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class ExpressionCache : Reference
	{
		private Expression exp;
		private Interpreter interpreter;

		public override Value RefValue
		{
			get
			{
				return exp.Execute(interpreter);
			}
			set { }
		}

		public ExpressionCache(Expression exp, Interpreter interpreter)
			: base(Void.Instance)
		{
			this.exp = exp;
			this.interpreter = interpreter;
		}
	}
}

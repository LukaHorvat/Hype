using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hype
{
	class CSharpFunction : Function
	{
		public Func<List<Value>, Value> Function;

		public CSharpFunction(Func<List<Value>, Value> function, Fixity fixity)
			:base(fixity)
		{
			Function = function;
		}

		public override Value Execute(List<Value> arguments)
		{
			return Function(arguments);
		}
	}
}

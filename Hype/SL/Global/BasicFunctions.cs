using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype.SL.Global
{
	class BasicFunctions
	{
		public Interpreter Interpreter;

		public BasicFunctions(Interpreter interpreter)
		{
			Interpreter = interpreter;
		}

		public Value For(List<Value> arguments)
		{
			(arguments[0] as CodeBlock).Execute(Interpreter);
			while (true)
			{
				var condition = (arguments[1] as CodeBlock).Execute(Interpreter) as Boolean;
				if (condition == null) throw new FunctionCallException();
				if (!condition.Bool) break;
				(arguments[3] as CodeBlock).Execute(Interpreter);
				(arguments[2] as CodeBlock).Execute(Interpreter);
			}
			return Void.Instance;
		}
	}
}

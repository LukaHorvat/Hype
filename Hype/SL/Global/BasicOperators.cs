using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hype.SL.Global
{
	class BasicOperators
	{
		public Interpreter Interpreter;

		public BasicOperators(Interpreter interpreter)
		{
			Interpreter = interpreter;
		}

		public Value Add(List<Value> arguments)
		{
			return new Number((arguments[0] as Number).Num + (arguments[1] as Number).Num);
		}

		public Value Subtract(List<Value> arguments)
		{
			return new Number((arguments[0] as Number).Num - (arguments[1] as Number).Num);
		}

		public Value Multiply(List<Value> arguments)
		{
			return new Number((arguments[0] as Number).Num * (arguments[1] as Number).Num);
		}

		public Value Divide(List<Value> arguments)
		{
			return new Number((arguments[0] as Number).Num / (arguments[1] as Number).Num);
		}

		public Value Assign(List<Value> arguments)
		{
			if (arguments[0].Var.Name != "")
			{ 
				Interpreter.CurrentScopeNode.AddToScope(arguments[0].Var.Name, arguments[1]);
			}
			else
			{
				//Could do something interesting
			}
			return arguments[1];
		}
	}
}

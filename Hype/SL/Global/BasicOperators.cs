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
			if (StandardLibrary.CheckArguments(arguments, typeof(Number), typeof(Number)))
			{
				return new Number((arguments[0] as Number).Num + (arguments[0] as Number).Num);
			}
			else
			{
				throw new Exception("Only numbers can be added together");
			}
		}

		public Value Subtract(List<Value> arguments)
		{
			if (StandardLibrary.CheckArguments(arguments, typeof(Number), typeof(Number)))
			{
				return new Number((arguments[0] as Number).Num - (arguments[0] as Number).Num);
			}
			else
			{
				throw new Exception("Only numbers can be subtracted");
			}
		}

		public Value Multiply(List<Value> arguments)
		{
			if (StandardLibrary.CheckArguments(arguments, typeof(Number), typeof(Number)))
			{
				return new Number((arguments[0] as Number).Num * (arguments[0] as Number).Num);
			}
			else
			{
				throw new Exception("Only numbers can be multiplied");
			}
		}

		public Value Divide(List<Value> arguments)
		{
			if (StandardLibrary.CheckArguments(arguments, typeof(Number), typeof(Number)))
			{
				return new Number((arguments[0] as Number).Num / (arguments[0] as Number).Num);
			}
			else
			{
				throw new Exception("Only numbers can be divided");
			}
		}

		public Value Assign(List<Value> arguments)
		{
			if (arguments[0].Name != "")
			{
				Interpreter.CurrentScopeNode.AddToScope(arguments[0].Name, arguments[1]);
			}
			else
			{
				//Could do something interesting
			}
			return arguments[1];
		}
	}
}

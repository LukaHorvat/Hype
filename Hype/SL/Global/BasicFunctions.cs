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

		public Value If(List<Value> arguments)
		{
			if ((arguments[0] as Boolean).Bool)
			{
				return (arguments[1] as CodeBlock).Execute(Interpreter);
			}
			return new TypedObject("IfFalse");
		}

		public Value Else(List<Value> arguments)
		{
			if (arguments[0].Type == ValueType.GetType("IfFalse"))
			{
				return (arguments[1] as CodeBlock).Execute(Interpreter);
			}
			return arguments[0];
		}

		public Value ElseIf(List<Value> arguments)
		{
			if (arguments[0].Type == ValueType.GetType("IfFalse"))
			{
				if ((arguments[1] as Boolean).Bool)
				{
					//If the previous if was false and the current condition is true,
					//return the exec function that will execute the following CodeBlock
					return Interpreter.CurrentScopeNode.Lookup("exec");
				}
				else
				{
					//Otherwise, return a function that will consume the next CodeBlock,
					//but also return a IfFalse so the next else can operate on it
					return new CSharpFunction(args => new TypedObject("IfFalse"), Fixity.Prefix, 1);
				}
			}
			else
			{
				//If the previous condition was true, just consume the following CodeBlock
				return Interpreter.CurrentScopeNode.Lookup("consume");
			}
		}

		public Value Exec(List<Value> arguments)
		{
			return (arguments[0] as CodeBlock).Execute(Interpreter);
		}

		public Value Consume(List<Value> arguments)
		{
			return Void.Instance;
		}

		public Value PrintLine(List<Value> arguments)
		{
			Console.WriteLine(arguments[0].Show());
			return Void.Instance;
		}

		public Value Print(List<Value> arguments)
		{
			Console.Write(arguments[0].Show());
			return Void.Instance;
		}
	}
}

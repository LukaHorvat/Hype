﻿using System;
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

		[FunctionAttributes(Hype.Fixity.Prefix, "for")]
		public Void For(CodeBlock init, CodeBlock condition, CodeBlock increment, CodeBlock block)
		{
			init.Execute(Interpreter);
			while (true)
			{
				var b = condition.Execute(Interpreter) as Boolean;
				if (b == null) throw new FunctionCallException("The conditional expression of the for loop didn't produce a Boolean value");
				if (!b.Bool) break;
				block.Execute(Interpreter);
				increment.Execute(Interpreter);
			}
			return Void.Instance;
		}

		[FunctionAttributes(Hype.Fixity.Prefix, "if")]
		public Value If(Boolean condition, CodeBlock block)
		{
			if (condition.Bool)
			{
				return block.Execute(Interpreter);
			}
			return new TypedObject("IfFalse");
		}

		[FunctionAttributes(Hype.Fixity.Infix_18, "else")]
		public Value Else(Value previous, CodeBlock block)
		{
			if (previous.Type == ValueType.GetType("IfFalse"))
			{
				return block.Execute(Interpreter);
			}
			return previous;
		}

		[FunctionAttributes(Hype.Fixity.Infix_18, "elseIf")]
		public Functional ElseIf(Value previous, Boolean condition)
		{
			if (previous.Type == ValueType.GetType("IfFalse"))
			{
				if (condition.Bool)
				{
					//If the previous if was false and the current condition is true,
					//return the exec function that will execute the following CodeBlock
					return (Functional)Interpreter.CurrentScopeNode.Lookup("exec");
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
				return (Functional)Interpreter.CurrentScopeNode.Lookup("consume");
			}
		}

		[FunctionAttributes(Hype.Fixity.Prefix, "exec")]
		public Value Exec(CodeBlock block)
		{
			return block.Execute(Interpreter);
		}

		[FunctionAttributes(Hype.Fixity.Prefix, "consume")]
		public Void Consume(Value arg)
		{
			return Void.Instance;
		}

		[FunctionAttributes(Hype.Fixity.Prefix, "printLine")]
		public Void PrintLine(Value arg)
		{
			Console.WriteLine(arg.Show());
			return Void.Instance;
		}

		[FunctionAttributes(Hype.Fixity.Prefix, "print")]
		public Void Print(Value arg)
		{
			Console.Write(arg.Show());
			return Void.Instance;
		}
	}
}

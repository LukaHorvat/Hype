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

		[FunctionAttributes(Hype.Fixity.Infix_6, "+")]
		public Number Add(Number a, Number b)
		{
			return new Number(a.Num + b.Num);
		}

		[FunctionAttributes(Hype.Fixity.Infix_6, "-")]
		public Number Subtract(Number a, Number b)
		{
			return new Number(a.Num - b.Num);
		}

		[FunctionAttributes(Hype.Fixity.Infix_5, "*")]
		public Number Multiply(Number a, Number b)
		{
			return new Number(a.Num * b.Num);
		}

		[FunctionAttributes(Hype.Fixity.Infix_5, "/")]
		public Number Divide(Number a, Number b)
		{
			return new Number(a.Num / b.Num);
		}

		[FunctionAttributes(Hype.Fixity.Infix_8, "<")]
		public Boolean LessThan(Number a, Number b)
		{
			return new Boolean(a.Num < b.Num);
		}

		[FunctionAttributes(Hype.Fixity.Infix_8, ">")]
		public Boolean GreaterThan(Number a, Number b)
		{
			return new Boolean(a.Num > b.Num);
		}

		[FunctionAttributes(Hype.Fixity.Infix_8, "<=")]
		public Boolean LessOrEqual(Number a, Number b)
		{
			return new Boolean(a.Num <= b.Num);
		}

		[FunctionAttributes(Hype.Fixity.Infix_8, ">=")]
		public Boolean GreaterOrEqual(Number a, Number b)
		{
			return new Boolean(a.Num >= b.Num);
		}

		[FunctionAttributes(Hype.Fixity.Infix_9, "==")]
		public Boolean Equal(Number a, Number b)
		{
			return new Boolean(a.Num == b.Num);
		}

		[FunctionAttributes(Hype.Fixity.Infix_9, "!=")]
		public Boolean NotEqual(Number a, Number b)
		{
			return new Boolean(a.Num != b.Num);
		}

		[FunctionAttributes(Hype.Fixity.Infix_15, "=")]
		public Value Assign(Value variable, Value obj)
		{
			if (variable.Var.Name != "")
			{ 
				Interpreter.CurrentScopeNode.AddToScope(variable.Var.Name, obj);
			}
			else
			{
				//Could do something interesting
			}
			return obj;
		}
	}
}

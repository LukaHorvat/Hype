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

		[FunctionAttributes(Hype.Fixity.Infix_17, ",", 0)]
		public Collection Add(Collection a, Collection b)
		{
			var col = new Collection(a.InnerList.Count + b.InnerList.Count);
			a.InnerList.ForEach(v => col.InnerList.Add(v));
			b.InnerList.ForEach(v => col.InnerList.Add(v));
			return col;
		}

		[FunctionAttributes(Hype.Fixity.Infix_17, ",", 1)]
		public Collection Add(Value a, Collection b)
		{
			var col = new Collection(1 + b.InnerList.Count);
			col.InnerList.Add(a);
			b.InnerList.ForEach(v => col.InnerList.Add(v));
			return col;
		}

		[FunctionAttributes(Hype.Fixity.Infix_17, ",", 1)]
		public Collection Add(Collection a, Value b)
		{
			var col = new Collection(1 + a.InnerList.Count);
			a.InnerList.ForEach(v => col.InnerList.Add(v));
			col.InnerList.Add(b);
			return col;
		}

		[FunctionAttributes(Hype.Fixity.Infix_17, ",", 2)]
		public Collection Add(Value a, Value b)
		{
			var col = new Collection(2);
			col.InnerList.Add(a);
			col.InnerList.Add(b);
			return col;
		}

		[FunctionAttributes(Hype.Fixity.Infix_6, "+")]
		public String Add(String a, String b)
		{
			return new String(a.Str + b.Str);
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
		public Value Assign(Identifier variable, Value obj)
		{
			var str = variable.Str;
			if (str.IndexOf('.') != -1)
			{
				int lastIndex = str.LastIndexOf('.');
				var r = Interpreter.CurrentScopeNode.Lookup(str.Substring(0, str.Length - lastIndex - 1));
				if (r == null) throw new ExpressionException("Tried to access a field of a null reference");
				r.RefValue.ScopeNode.AddToScope(str.Substring(lastIndex + 1), obj);
			}
			else
			{
				Interpreter.CurrentScopeNode.AddToScope(variable.Str, obj);
			}
			return obj;
		}
	}
}

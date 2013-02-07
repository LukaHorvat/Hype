using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	enum Side
	{
		Left, Right, NoArgument
	}

	class FunctionGroup : Value
	{
		public List<Function> Functions;
		public Fixity Fixity { get; protected set; }

		public FunctionGroup(Function func)
			: base(ValueType.GetType("FunctionGroup"))
		{
			Functions = new List<Function>();
			AddFunction(func);
			Kind = ValueKind.Function;
		}

		public FunctionGroup(Fixity fixity)
			: base(ValueType.GetType("FunctionGroup"))
		{
			Functions = new List<Function>();
			Kind = ValueKind.Function;
			Fixity = fixity;
		}

		public void AddFunction(Function func)
		{
			if (Functions.Count == 0)
			{
				Fixity = func.Signature.Fixity;
			}
			if (func.Signature.Fixity == Fixity) Functions.Add(func);
			else throw new Exception("Only fuction with the same fixity can be gouped together");
		}

		public void AddWithOverride(Function func)
		{
			if (Functions.Count == 0)
			{
				Fixity = func.Signature.Fixity;
			}
			if (func.Signature.Fixity != Fixity)
			{
				Functions.Clear();
				Fixity = func.Signature.Fixity;
			}
			Functions.Add(func);
		}

		public virtual Value Apply(Value argument, Side side)
		{
			var partial = new PartialApplication(new List<Value>(), Functions, this);
			return partial.Apply(argument, side);
		}

		public void MergeOrReplace(FunctionGroup group)
		{
			if (group.Fixity != Fixity)
			{
				Functions.Clear();
				Fixity = group.Fixity;
			}
			foreach (var func in group.Functions) AddFunction(func);
		}

		public Function MatchesNoArguments
		{
			get
			{
				return Functions.FirstOrDefault(f => f.Signature.InputSignature.Count == 0);
			}
		}
	}
}

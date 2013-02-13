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

	class FunctionGroup : Value, IInvokable
	{
		public List<IInvokable> Functions;
		public Fixity Fixity { get; protected set; }
		public FunctionType Signature { get { return Functions.Count > 0 ? Functions[0].Signature : null; } }

		public FunctionGroup(Function func)
			: base(ValueType.GetType("FunctionGroup"))
		{
			Functions = new List<IInvokable>();
			AddFunction(func);
			Kind = ValueKind.Function;
		}

		public FunctionGroup(Fixity fixity)
			: base(ValueType.GetType("FunctionGroup"))
		{
			Functions = new List<IInvokable>();
			Kind = ValueKind.Function;
			Fixity = fixity;
		}

		public void AddFunction(IInvokable func)
		{
			if (Functions.Count == 0)
			{
				Fixity = func.Signature.Fixity;
			}
			if (func.Signature.Fixity == Fixity) Functions.Add(func);
			else throw new Exception("Only fuction with the same fixity can be gouped together");
		}

		public void AddWithOverride(IInvokable func)
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
			var partial = new PartialApplication(new List<Value>(), Functions.Clone(), this);
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

		public IInvokable MatchesNoArguments
		{
			get
			{
				return Functions.FirstOrDefault(f => f.Signature.InputSignature.Count == 0);
			}
		}

		public override string ToString()
		{
			return "FuctionGroup: " + Var.Name;
		}

		public Value Execute(List<Value> arguments)
		{
			if (arguments.Count == 0)
			{
				var noArgs = MatchesNoArguments;
				if (noArgs != null) return noArgs.Execute(arguments);
				else throw new NoMatchingSignature(SignatureMismatchType.Group);
			}
			else
			{
				foreach (var function in Functions)
				{
					if (function.Signature.MatchesArguments(arguments) == MatchType.FullMatch)
					{
						return function.Execute(arguments);
					}
				}
				throw new NoMatchingSignature(SignatureMismatchType.Group);
			}
		}
	}
}

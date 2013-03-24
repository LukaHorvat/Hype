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

	enum MatchType
	{
		Mismatch,
		PartialMatch,
		FullMatch
	}

	class PartialApplication : Functional, ICurryable
	{
		public List<PartialCall> PotentialMatches;

		private Fixity fixity;
		public override Fixity Fixity
		{
			get { return fixity; }
			set
			{
				if (value != Fixity.Prefix)
				{
					if (PotentialMatches.Any(m => m.ArgumentsLeft != 2))
					{
						throw new ExpressionException("Unable to set fixity to infix because not al overloads require exactly 2 arguments.");
					}
				}
				fixity = value;
			}
		}

		private string funcName = "";

		public PartialApplication(Function func)
			: this(new List<IInvokable>() { func }, func.Fixity, func.Var.OriginalName)
		{
			Type = func.Type;
		}

		public PartialApplication(List<IInvokable> matches, Fixity fixity, string name)
			: this(matches.Select(m => new PartialCall(m)).ToList(), fixity, name) { }

		public PartialApplication(List<PartialCall> matches, Fixity fixity, string name)
			: base(ValueType.FunctionGroup)
		{
			if (fixity != Hype.Fixity.Prefix && matches.Any(m => m.ArgumentsLeft != 2))
			{
				throw new ExpressionException("Those overloads can't be added to an infix function because not all of them require exactly two arguments.");
			}
			PotentialMatches = matches;
			Fixity = fixity;
			funcName = name;
			Kind = ValueKind.Function;
		}

		public Value Apply(Value argument, Side side)
		{
			var potentialMatches = PotentialMatches.Clone();

			for (int i = 0; i < potentialMatches.Count; i++)
			{
				if (Fixity != Hype.Fixity.Prefix && side == Side.Right)
				{
					if (potentialMatches[i].CheckArgumentRight(argument) == MatchType.Mismatch)
					{
						potentialMatches.RemoveAt(i);
						i--;
						continue;
					}
					else
					{
						potentialMatches[i] = potentialMatches[i].AddRight(argument);
					}
				}
				else
				{
					if (potentialMatches[i].CheckArgument(argument) == MatchType.Mismatch)
					{
						potentialMatches.RemoveAt(i);
						i--;
						continue;
					}
					else
					{
						potentialMatches[i] = potentialMatches[i].Add(argument);
						if (potentialMatches[i].Invokable) return potentialMatches[i].Invoke();
					}
				}
			}
			if (potentialMatches.Count == 0)
			{
				if (argument is Functional && (argument as Functional).Fixity != Hype.Fixity.Prefix)
				{
					//PartialCalls will just report a mismatch if they're passed an infix function and they're not looking
					//for an Identifier. In case we do have an infix function an all the potential matches returned a mismatch
					//we finally throw the exception
					throw new NonPrefixFunctionAsArgument();
				}
				else
				{
					return SignatureMismatchValue.Instance;
					//throw new NoMatchingSignature(SignatureMismatchType.Group);
				}
			}

			return new PartialApplication(potentialMatches, Fixity.Prefix, funcName);
		}

		public PartialApplication Merge(PartialApplication other)
		{
			var appl = new PartialApplication(PotentialMatches.Clone(), Fixity, funcName);
			foreach (var function in other.PotentialMatches) appl.AddFunction(function);
			return appl;
		}

		public PartialApplication Merge(IInvokable function)
		{
			var appl = new PartialApplication(PotentialMatches.Clone(), Fixity, funcName);
			appl.AddFunction(function);
			return appl;
		}

		private void AddFunction(IInvokable func)
		{
			var partialCall = new PartialCall(func);
			AddFunction(partialCall);
		}

		private void AddFunction(PartialCall partialCall)
		{
			if (fixity != Hype.Fixity.Prefix && partialCall.ArgumentsLeft != 2)
			{
				throw new ExpressionException("This overload can't be added to an infix function because it does not require exactly two arguments.");
			}
			PotentialMatches.Add(partialCall);
		}

		public override string ToString()
		{
			return "PartialApplication: " + funcName;
		}

		public override IInvokable MatchesNoArguments
		{
			get
			{
				return PotentialMatches.FirstOrDefault(f => f.Invokable).Function;
			}
		}

		public ICurryable PrefixApplication
		{
			get
			{
				return new PartialApplication(PotentialMatches.Clone(), Fixity.Prefix, funcName);
			}
		}
	}
}

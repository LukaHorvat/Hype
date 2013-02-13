using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	enum MatchType
	{
		Mismatch,
		PartialMatch,
		FullMatch
	}

	class PartialApplication : Value, ICurryable
	{
		public List<Value> Arguments;
		public List<IInvokable> PotentialMatches;
		public Fixity Fixity { get; set; }

		private string funcName = "";

		public PartialApplication(List<Value> args, List<IInvokable> matches, FunctionGroup group)
			: base(ValueType.GetType("FunctionGroup"))
		{
			Arguments = args;
			PotentialMatches = matches;
			Fixity = group.Fixity;
			funcName = group.Var.Name;
		}

		public PartialApplication(List<Value> args, List<IInvokable> matches, Function func)
			: base(ValueType.GetType("FunctionGroup"))
		{
			Arguments = args;
			PotentialMatches = matches;
			Fixity = Hype.Fixity.Prefix;
			funcName = func.Var.Name;
		}

		public PartialApplication(List<Value> args, List<IInvokable> matches, Fixity fixity, string name)
			: base(ValueType.GetType("FunctionGroup"))
		{
			Arguments = args;
			PotentialMatches = matches;
			Fixity = Hype.Fixity.Prefix;
			funcName = name;
		}

		public Value Apply(Value argument, Side side)
		{
			var arguments = Arguments.Clone();
			var potentialMatches = PotentialMatches.Clone();

			if (Fixity != Hype.Fixity.Prefix && arguments.Count == 0 && side == Side.Right)
			{
				arguments.Add(null);
				arguments.Add(argument);
			}
			else if (arguments.Count == 2 && arguments[0] == null)
			{
				arguments[0] = argument;
			}
			else if (argument != null) arguments.Add(argument);

			for (int i = 0; i < potentialMatches.Count; ++i)
			{
				var res = potentialMatches[i].Signature.MatchesArguments(arguments);
				if (res == MatchType.FullMatch)
				{
					return potentialMatches[i].Execute(arguments);
				}
				if (res == MatchType.Mismatch)
				{
					potentialMatches.RemoveAt(i);
					--i;
				}
			}
			if (potentialMatches.Count == 0) throw new Exception("No fuctions match the signature.");

			return new PartialApplication(arguments, potentialMatches.Clone(), Fixity, funcName);
		}

		public override string ToString()
		{
			return "PartialApplication: " + funcName;
		}

		public IInvokable MatchesNoArguments
		{
			get
			{
				return PotentialMatches.FirstOrDefault(f => f.Signature.InputSignature.Count == 0);
			}
		}
	}
}

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

	class PartialApplication : Functional, ICurryable
	{
		public List<List<Value>> Arguments;
		public List<IInvokable> PotentialMatches;
		public override Fixity Fixity { get; set; }

		private string funcName = "";
		private Dictionary<IInvokable, List<Value>> functionArgumentLink;

		public PartialApplication(List<Value> args, List<IInvokable> matches, FunctionGroup group)
			: this(args, matches, group.Fixity, group.Var.Name) { }

		public PartialApplication(List<Value> args, Function func)
			: this(args, new List<IInvokable>() { func }, Fixity.Prefix, func.Var.Name) { }

		public PartialApplication(List<Value> args, List<IInvokable> matches, Fixity fixity, string name)
			: base(ValueType.GetType("FunctionGroup"))
		{
			Arguments = new List<List<Value>>();
			functionArgumentLink = new Dictionary<IInvokable, List<Value>>();
			Arguments.Add(args);
			PotentialMatches = matches;
			foreach (var match in PotentialMatches) functionArgumentLink[match] = args;
			Fixity = fixity;
			funcName = name;
			Kind = ValueKind.Function;
		}

		public Value Apply(Value argument, Side side)
		{
			var arguments = Arguments.Clone();
			for (int i = 0; i < arguments.Count; ++i)
			{
				arguments[i] = arguments[i].Clone();
			}
			var potentialMatches = PotentialMatches.Clone();

			if (argument is Functional && ((IFunctionGroup)argument).Fixity != Fixity.Prefix)
			{
				throw new NonPrefixFunctionAsArgument();
			}

			if (Fixity != Fixity.Prefix && arguments.Count == 0 && side == Side.Right)
			{
				foreach (
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

			return new PartialApplication(arguments, potentialMatches.Clone(), Fixity.Prefix, funcName);
		}

		public override string ToString()
		{
			return "PartialApplication: " + funcName;
		}

		public override IInvokable MatchesNoArguments
		{
			get
			{
				return PotentialMatches.FirstOrDefault(f => f.Signature.InputSignature.Count == 0);
			}
		}


		public ICurryable PrefixApplication
		{
			get
			{
				return new PartialApplication(Arguments, PotentialMatches.Clone(), Fixity.Prefix, funcName);
			}
		}
	}
}

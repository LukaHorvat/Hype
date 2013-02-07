using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class PartialApplication : FunctionGroup
	{
		enum MatchType
		{
			Mismatch,
			PartialMatch,
			FullMatch
		}

		public List<Value> Arguments;
		public List<Function> PotentialMatches;

		private FunctionGroup parent;

		public PartialApplication(List<Value> args, List<Function> matches, FunctionGroup group)
			:base(Fixity.Prefix)
		{
			Arguments = args;
			PotentialMatches = matches;
			parent = group;
		}

		public override Value Apply(Value argument, Side side)
		{
			if (parent.Fixity != Hype.Fixity.Prefix && Arguments.Count == 0 && side == Side.Right)
			{
				Arguments.Add(null);
				Arguments.Add(argument);
			}
			else
			{
				if (parent.Fixity != Hype.Fixity.Prefix && Arguments.Count == 2 && Arguments[0] == null)
				{
					Arguments[0] = argument;
				}else if (argument != null) Arguments.Add(argument);

				for (int i = 0; i < PotentialMatches.Count; ++i)
				{
					var res = CheckSignature(PotentialMatches[i], Arguments);
					if (res == MatchType.FullMatch)
					{
						return PotentialMatches[i].Execute(Arguments);
					}
					if (res == MatchType.Mismatch)
					{
						PotentialMatches.RemoveAt(i);
						--i;
					}
				}
				if (PotentialMatches.Count == 0) throw new Exception("No fuctions match the signature.");
			}
			return new PartialApplication(Arguments, PotentialMatches, parent);
		}

		private MatchType CheckSignature(Function func, List<Value> arguments)
		{
			if (arguments.Count > func.Signature.InputSignature.Count) return MatchType.Mismatch;
			bool matchSoFar = arguments.Select((x, i) => new[] { x.Type, ValueType.GetType("Uncertain") }.Contains(func.Signature.InputSignature[i])).All(x => x);
			if (!matchSoFar) return MatchType.Mismatch;
			if (arguments.Count == func.Signature.InputSignature.Count && matchSoFar) return MatchType.FullMatch;
			return MatchType.PartialMatch;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class FunctionType : ValueType
	{
		public Fixity Fixity;
		public List<ValueType> InputSignature;
		public ValueType OutputSignature;
		public int NumArguments { get { return InputSignature.Count; } }

		public FunctionType(Fixity fixity, int numArguments = 2)
			: base("FunctionType")
		{
			Fixity = fixity;
			InputSignature = new List<ValueType>();

			if (fixity != Hype.Fixity.Prefix && numArguments != 2) throw new Exception("Infix functions can only take 2 arguments.");

			for (int i = 0; i < numArguments; ++i)
			{
				InputSignature.Add(ValueType.GetType("Uncertain"));
			}
			OutputSignature = ValueType.GetType("Uncertain");
		}

		/// <summary>
		/// Uncertain parameters in any of the two function types will return false.
		/// Only fully defined fuction types can be compared with useful results.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool MatchesType(FunctionType other)
		{
			foreach (var funcType in new[] { this, other })
			{
				if (funcType.InputSignature.Contains(ValueType.GetType("Uncertain"))) return false;
				if (funcType.OutputSignature == ValueType.GetType("Uncertain")) return false;
			}
			return InputSignature.MemberwiseEquals(other.InputSignature) && OutputSignature == other.OutputSignature;
		}

		public MatchType MatchesArguments(List<Value> arguments)
		{
			if (arguments[0] == null)
			{
				if (Fixity == Hype.Fixity.Prefix) return MatchType.Mismatch;
				if (InputSignature[1] == ValueType.GetType("Uncertain") || arguments[1].Type.IsSubtypeOf(InputSignature[1])) return MatchType.PartialMatch;
				return MatchType.Mismatch;
			}

			if (arguments.Count > InputSignature.Count) return MatchType.Mismatch;
			bool matchSoFar = arguments.Select((x, i) => x.Type.IsSubtypeOf(InputSignature[i]) || InputSignature[i] == ValueType.GetType("Uncertain")).All(x => x);
			if (!matchSoFar) return MatchType.Mismatch;
			if (arguments.Count == InputSignature.Count && matchSoFar) return MatchType.FullMatch;
			return MatchType.PartialMatch;
		}
	}
}

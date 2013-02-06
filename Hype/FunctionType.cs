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
		public List<ValueType> OutputSignature;
		public int NumArguments { get { return InputSignature.Count; } }

		public FunctionType(Fixity fixity, int numArguments = 2)
			: base("FunctionType")
		{
			Fixity = fixity;
			InputSignature = new List<ValueType>();
			OutputSignature = new List<ValueType>();

			if (fixity != Hype.Fixity.Prefix && numArguments != 2) throw new Exception("Infix functions can only take 2 arguments.");

			for (int i = 0; i < numArguments; ++i)
			{
				InputSignature.Add(ValueType.GetType("Uncertain"));
			}
			OutputSignature.Add(ValueType.GetType("Uncertain"));
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
				foreach (var sig in new[] { funcType.InputSignature, funcType.OutputSignature })
				{
					if (sig.Contains(ValueType.GetType("Uncertain"))) return false;
				}
			}
			return InputSignature.MemberwiseEquals(other.InputSignature) && OutputSignature.MemberwiseEquals(other.OutputSignature);
		}
	}
}

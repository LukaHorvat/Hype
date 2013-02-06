using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class FunctionGroup : Value
	{
		public List<Function> Functions;
		public Fixity Fixity;

		public FunctionGroup(Function func)
			: base(ValueType.GetType("FunctionGroup"))
		{
			Functions = new List<Function>();
			AddFunction(func);
			Kind = ValueKind.Function;
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

		public void MergeOrReplace(FunctionGroup group)
		{
			if (group.Fixity != Fixity)
			{
				Functions.Clear();
				Fixity = group.Fixity;
			}
			foreach (var func in group.Functions) AddFunction(func);
		}
	}
}

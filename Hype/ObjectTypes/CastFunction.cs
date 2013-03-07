using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class CastFunction : Function
	{
		public ValueType CastType;

		public CastFunction(ValueType type)
			: base(Fixity.Prefix, 1)
		{
			CastType = type;
			Signature.InputSignature.Clear();
			Signature.InputSignature.Add(ValueType.BlankIdentifier);
			Signature.OutputSignature = ValueType.BlankIdentifier;
		}

		public override Value Execute(List<Value> arguments)
		{
			arguments[0].Type = CastType;
			return arguments[0];
		}

		public override string ToString()
		{
			return "CastFunction: " + CastType.TypeName;
		}
	}
}

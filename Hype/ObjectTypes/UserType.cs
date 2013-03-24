using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class UserType : CastFunction
	{
		public CodeBlock PropertyBlock;

		public UserType(CodeBlock propertyBlock)
			: base(ValueType.GetNewType())
		{
			PropertyBlock = propertyBlock;
			Type = ValueType.UserType;
		}

		public override string ToString()
		{
			return "Type: " + Var.OriginalName;
		}
	}
}
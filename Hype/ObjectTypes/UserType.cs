using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class UserType : Value
	{
		public CodeBlock PropertyBlock;

		public UserType(CodeBlock propertyBlock)
			: base(ValueType.UserType)
		{
			PropertyBlock = propertyBlock;
		}
	}
}

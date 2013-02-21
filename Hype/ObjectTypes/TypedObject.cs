using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class TypedObject : Value
	{
		public TypedObject(string type)
			: base(ValueType.GetType(type)) { }

		public override string ToString()
		{
			return "TypedObject: " + Type.TypeName;
		}
	}
}

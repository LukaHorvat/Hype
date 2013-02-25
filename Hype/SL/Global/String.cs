using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype.SL.Global
{
	class String : Value
	{
		public string Str;

		public String(string str)
			: base(ValueType.GetType("String"))
		{
			Str = str;
		}

		public override string ToString()
		{
			return "String: " + Str;
		}

		public override string Show()
		{
			return Str;
		}
	}
}

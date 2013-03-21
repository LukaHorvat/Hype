using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class ReturnValue : Value
	{
		public Value InnerValue;

		public ReturnValue(Value val)
			: base(ValueType.ReturnValue)
		{
			InnerValue = val;
		}
	}
}

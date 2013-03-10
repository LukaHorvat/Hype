using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class ProxyValue : Value
	{
		public Func<Value> Getter;

		public ProxyValue(Func<Value> getFunc)
			: base(ValueType.ProxyValue)
		{
			Getter = getFunc;
		}
	}
}

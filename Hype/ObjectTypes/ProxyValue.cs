using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class ProxyValue : Value
	{
		public Func<Value> Getter;
		public Action<Value> Setter;

		public ProxyValue(Func<Value> getFunc)
			: base(ValueType.ProxyValue)
		{
			Getter = delegate{
				var val = getFunc();
				val.Var.LastName = Var.LastName;
				return val;	
			};
		}
	}

	class ProxyWithSetter<T> : ProxyValue where T : Value
	{
		public ProxyWithSetter(Func<Value> getFunc, Action<T> setFunction)
			: base(getFunc)
		{
			Setter = delegate (Value arg)
			{
				Var.LastName = arg.Var.LastName;
				setFunction((T)arg);
			};
			Type = ValueType.ProxyWithSetter;
		}
	}
}

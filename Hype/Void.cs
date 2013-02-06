using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hype
{
	class Void : Value
	{
		public static Void Instance = new Void();

		public Void()
			: base(ValueType.GetType("Void")) { }
	}
}

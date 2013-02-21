using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class List : Value
	{
		public List()
			: base(ValueType.GetType("List")) { }
	}
}

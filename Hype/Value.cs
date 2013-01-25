using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hype
{
	class Value
	{
		public string Name;

		public virtual ValueKind GetKind()
		{
			return ValueKind.Object;
		}
	}
}

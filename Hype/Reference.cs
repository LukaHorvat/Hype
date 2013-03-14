using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class Reference
	{
		public virtual Value RefValue { get; set; }

		public Reference(Value initialValue)
		{
			RefValue = initialValue;
		}
	}
}

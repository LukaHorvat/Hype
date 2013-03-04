using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class LookupCache
	{
		public virtual Value Cache { get; set; }

		public LookupCache(Value initialValue)
		{
			Cache = initialValue;
		}
	}
}

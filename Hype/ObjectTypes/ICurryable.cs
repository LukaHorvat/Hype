using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	interface ICurryable : IFunctionGroup
	{
		Value Apply(Value val, Side side);

		/// <summary>
		/// Should return the same function but in prefix form.
		/// </summary>
		ICurryable PrefixApplication { get; }
	}
}

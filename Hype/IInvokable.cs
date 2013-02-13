using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	interface IInvokable : ICurryable
	{
		Value Execute(List<Value> arguments);

		/// <summary>
		/// Complete, invokable functions need to also have their entire signatures visible.
		/// </summary>
		FunctionType Signature { get; }
	}
}

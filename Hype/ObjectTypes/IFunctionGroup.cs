using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	interface IFunctionGroup
	{
		/// <summary>
		/// All function like objects have to at least have a visible Fixity.
		/// </summary>
		Fixity Fixity { get; }

		/// <summary>
		/// Returns a IInvokable that can operate with no arguments or null if none exsist in this function group.
		/// </summary>
		IInvokable MatchesNoArguments { get; }
	}
}

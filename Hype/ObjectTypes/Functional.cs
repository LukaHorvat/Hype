using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	abstract class Functional : Value, IFunctionGroup
	{
		public Functional(ValueType type)
			: base(type) { }

		public abstract Fixity Fixity
		{
			get;
			set;
		}

		public abstract IInvokable MatchesNoArguments
		{
			get;
		}
	}
}

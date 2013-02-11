using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	interface IInvokable
	{
		public Value Apply(Value val, Side side);
		public Value Execute(List<Value> arguments);
		public Fixity Fixity;
	}
}

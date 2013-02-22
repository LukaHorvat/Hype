using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype.SL
{
	class FunctionAttributes : Attribute
	{
		public Fixity Fixity;
		public string Identifier;
		public int Priority;

		public FunctionAttributes(Fixity fixity, string identifier, int priority = 0)
		{
			Fixity = fixity;
			Identifier = identifier;
			Priority = priority;
		}
	}
}

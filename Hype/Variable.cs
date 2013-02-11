using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class Variable
	{
		public string Name;

		public Variable(string name)
		{
			Name = name;
		}

		public override string ToString()
		{
			return "\"" + Name + "\"";
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class Variable
	{
		public List<string> Names;

		public Variable(string name)
		{
			Names = new List<string>() { name };
		}

		public Variable()
		{
			Names = new List<string>();
		}

		public override string ToString()
		{
			return "\"" + Names + "\"";
		}
	}
}

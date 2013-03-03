using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hype.SL.Global;

namespace Hype
{
	class Identifier : Hype.SL.Global.String
	{
		public Identifier(string str)
			: base(str)
		{
			Type = ValueType.Identifier;
		}
	}
}

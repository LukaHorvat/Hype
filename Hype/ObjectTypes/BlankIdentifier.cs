﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hype
{
	class BlankIdentifier : Value
	{
		public BlankIdentifier(string name)
			:base(ValueType.BlankIdentifier)
		{
			Var.OriginalName = name;
		}

		public override string ToString()
		{
			return "BlankIdentifier: " + Var.OriginalName;
		}
	}
}

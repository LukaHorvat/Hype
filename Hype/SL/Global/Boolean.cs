﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype.SL.Global
{
	class Boolean : Value
	{
		public bool Bool;

		public Boolean(bool b)
			:base(ValueType.GetType("Boolean"))
		{
			Bool = b;
		}

		public override string ToString()
		{
			return "Boolean: " + Bool;
		}
	}
}

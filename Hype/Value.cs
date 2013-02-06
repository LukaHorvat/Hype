using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hype
{
	class Value
	{
		/// <summary>
		/// A changable string that represents the name of the last variable accessed that referenced this Value
		/// </summary>
		public Variable Var = new Variable("");

		public ValueType Type { get; protected set; }
		public ValueKind Kind { get; protected set; }

		public Value(ValueType type)
		{
			Type = type;
			Kind = ValueKind.Object;
		}
	}
}

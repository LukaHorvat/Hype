using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hype.SL.Global
{
	class Number : Value
	{
		public int Num;

		public Number(int num)
			: base(ValueType.Number)
		{
			Num = num;
		}

		public override string ToString()
		{
			return "Number: " + Num;
		}

		public override string Show()
		{
			return "" + Num;
		}
	}
}

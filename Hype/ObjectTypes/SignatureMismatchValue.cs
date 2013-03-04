using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class SignatureMismatchValue : Value
	{
		public static SignatureMismatchValue Instance = new SignatureMismatchValue();

		public SignatureMismatchValue()
			: base(ValueType.GetType("SignatureMismatchValue")) { }
	}
}

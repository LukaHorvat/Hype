using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	enum SignatureMismatchType
	{
		Group,
		Individual
	}

	class NoMatchingSignature : Exception
	{
		string message;

		public override string Message
		{
			get
			{
				return message;
			}
		}

		public NoMatchingSignature(SignatureMismatchType type)
		{
			if (type == SignatureMismatchType.Group)
			{
				
				message = "No functions of the group match this signature";
			}
			else
			{
				message = "The function doesn't match this signature";
			}
		}
	}
}

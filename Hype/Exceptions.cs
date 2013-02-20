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

	class ExpressionException : Exception
	{
		public ExpressionException(string message)
			: base(message) { }
	}

	class MultipleValuesLeft : ExpressionException
	{
		public MultipleValuesLeft()
			: base("More than one value was left after the expression executed.") { }
	}

	class FunctionCallException : Exception
	{
		public FunctionCallException() { }

		public FunctionCallException(string message)
			: base(message) { }
	}

	class NonPrefixFunctionAsArgument : FunctionCallException
	{
		string message;

		public override string Message
		{
			get
			{
				return message;
			}
		}

		public NonPrefixFunctionAsArgument()
		{
			message = "Function passed as an argument was in infix form. Surround it with brackets.";
		}
	}

	class RightArgumentPassedToNonInfixFunction : FunctionCallException
	{
		string message;

		public override string Message
		{
			get
			{
				return message;
			}
		}

		public RightArgumentPassedToNonInfixFunction()
		{
			message = "The right side arguments was passed to a function that's not in infix form.";
		}
	}

	class NoMatchingSignature : FunctionCallException
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

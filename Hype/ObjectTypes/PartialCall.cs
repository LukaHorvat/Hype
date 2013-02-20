using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class PartialCall
	{
		public IInvokable Function;
		public Value Argument;
		public int ArgumentsLeft;

		private PartialCall last;
		private bool right;

		public bool Invokable
		{
			get { return ArgumentsLeft == 0; }
		}

		public ValueType Expecting
		{
			get
			{
				if (Invokable) return null;
				int n;
				var sig = Function.Signature.InputSignature;
				if (right)
				{
					n = 2;
				}
				else
				{
					n = ArgumentsLeft;
				}
				return sig[sig.Count - n];
			}
		}

		public PartialCall(IInvokable function)
		{
			Function = function;
			ArgumentsLeft = function.Signature.InputSignature.Count;
		}

		private PartialCall(Value arg, PartialCall last, bool right = false)
		{
			ArgumentsLeft = last.ArgumentsLeft - 1;
			if (right && ArgumentsLeft != 1) throw new RightArgumentPassedToNonInfixFunction();
			Function = last.Function;
			Argument = arg;
			this.last = last;
			this.right = right;
		}

		public PartialCall Add(Value argument)
		{
			if (CheckArgument(argument) == MatchType.Mismatch) throw new NoMatchingSignature(SignatureMismatchType.Individual);
			var call = new PartialCall(argument, this);
			if (right)
			{
				var temp = new PartialCall(Argument, call);
				temp.ArgumentsLeft = 0; //ArgumentsLeft needs to be fixed to 0 because this additional PartialCall doesn't actually need to decrement it
				call.last = last;
				return temp;
			}
			return call;
		}

		public PartialCall AddRight(Value argument)
		{
			if (CheckArgument(argument) == MatchType.Mismatch) throw new NoMatchingSignature(SignatureMismatchType.Individual);
			return new PartialCall(argument, this, true);
		}

		public MatchType CheckArgument(Value argument)
		{
			if (Expecting == ValueType.GetType("Uncertain") || Expecting == argument.Type)
			{
				if (ArgumentsLeft == 1) return MatchType.FullMatch;
				else return MatchType.PartialMatch;
			}
			return MatchType.Mismatch;
		}

		public MatchType CheckArgumentRight(Value argument)
		{
			if (ArgumentsLeft != 2) throw new RightArgumentPassedToNonInfixFunction();
			if (Function.Signature.InputSignature.Last() == ValueType.GetType("Uncertain") || Function.Signature.InputSignature.Last() == argument.Type)
			{
				return MatchType.PartialMatch;
			}
			return MatchType.Mismatch;
		}

		public Value Invoke()
		{
			if (!Invokable) throw new NoMatchingSignature(SignatureMismatchType.Individual);
			var args = new List<Value>();
			for (var call = this; call.last != null; call = call.last) args.Add(call.Argument);
			args.Reverse();

			return Function.Execute(args);
		}
	}
}

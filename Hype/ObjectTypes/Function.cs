using Hype.SL.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hype
{
	public enum Fixity : int
	{
		/// <summary>
		/// Highest fixity. Always executes first.
		/// </summary>
		Prefix = 0,
		Infix_1 = 1,
		/// <summary>
		/// Postfix increment/decrement...
		/// </summary>
		Infix_2 = 2,
		/// <summary>
		/// Prefix increment/decrement...
		/// </summary>
		Infix_3 = 3,
		Infix_4 = 4,
		/// <summary>
		/// Multiplication, division and modulus
		/// </summary>
		Infix_5 = 5,
		/// <summary>
		/// Addition and subtraction
		/// </summary>
		Infix_6 = 6,
		Infix_7 = 7,
		/// <summary>
		/// Comparison operators
		/// </summary>
		Infix_8 = 8,
		/// <summary>
		/// Equality and inequality
		/// </summary>
		Infix_9 = 9,
		Infix_10 = 10,
		Infix_11 = 11,
		Infix_12 = 12,
		/// <summary>
		/// Logical AND
		/// </summary>
		Infix_13 = 13,
		/// <summary>
		/// Logical OR
		/// </summary>
		Infix_14 = 14,
		/// <summary>
		/// Various assignment operators
		/// </summary>
		Infix_15 = 15,
		Infix_16 = 16,
		/// <summary>
		/// Comma
		/// </summary>
		Infix_17 = 17,
		/// <summary>
		/// Other
		/// </summary>
		Infix_18 = 18
	}

	class Function : Functional, IInvokable
	{
		public FunctionType Signature { get; set; }
		public ScopeTreeNode ScopeNode;

		public override Fixity Fixity { get { return Signature.Fixity; } set { Signature.Fixity = value; } }

		public Function(Fixity fixity)
			: base(ValueType.Function)
		{
			if (fixity == 0) throw new Exception("This constructor can only be used for infix functions");
			Signature = new FunctionType(fixity);
			Kind = ValueKind.Function;
		}

		public Function(Fixity fixity, int numArguments)
			: base(ValueType.Function)
		{
			if (fixity > 0 && numArguments != 2) throw new Exception("This constructor can only be used for prefix functions");
			Signature = new FunctionType(fixity, numArguments);
			Kind = ValueKind.Function;
		}

		public virtual Value Execute(List<Value> arguments)
		{
			return Void.Instance;
		}

		public Value Apply(Value val, Side side)
		{
			var partial = new PartialApplication(this);
			return partial.Apply(val, side);
		}

		public override IInvokable MatchesNoArguments
		{
			get { return Signature.InputSignature.Count == 0 ? this : null; }
		}


		public ICurryable PrefixApplication
		{
			get { return new PartialApplication(new List<IInvokable>() { this }, Hype.Fixity.Prefix, Var.Names[0]); }
		}
	}
}

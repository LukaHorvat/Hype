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
		public Variable Var = new Variable();

		public ValueType Type { get; set; }
		public ValueKind Kind { get; set; }

		public ScopeTreeNode ScopeNode;

		public Value(ValueType type)
		{
			Type = type;
			Kind = ValueKind.Object;
			ScopeNode = new ScopeTreeNode(Permanency.Permanent);

			if (!(Type <= ValueType.Functional))
			{
				var method = new CSharpFunction(
					l =>
					{
						var fun = l[0] as Function;
						fun.FunctionScope.Parent = ScopeNode;
						ScopeNode.AddToScope(fun.Var.LastName, fun);
						return Void.Instance;
					}, Fixity.Prefix, 1);
				method.Signature.InputSignature[0] = ValueType.Function;
				ScopeNode.AddToScope("method", method);
			}
		}

		public virtual string Show()
		{
			return "[Var: " + Var.OriginalName + ", Type: " + Type + "]";
		}
	}
}

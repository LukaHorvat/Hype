using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hype.SL.Global;

namespace Hype.SL
{
	class StandardLibrary
	{
		public static void Load(Interpreter interpreter)
		{
			var basicOperators = new BasicOperators(interpreter);

			var add = new CSharpFunction(basicOperators.Add, Fixity.Infix_6)
			{
				InputSignature = new List<Type>() { typeof(Number), typeof(Number) },
				OutputSignature = new List<Type>() { typeof(Number) }
			};
			interpreter.ScopeTreeRoot.AddToScope("+", add);
			var subtract = new CSharpFunction(basicOperators.Subtract, Fixity.Infix_6)
			{
				InputSignature = new List<Type>() { typeof(Number), typeof(Number) },
				OutputSignature = new List<Type>() { typeof(Number) }
			};
			interpreter.ScopeTreeRoot.AddToScope("-", subtract);
			var multiply = new CSharpFunction(basicOperators.Multiply, Fixity.Infix_5)
			{
				InputSignature = new List<Type>() { typeof(Number), typeof(Number) },
				OutputSignature = new List<Type>() { typeof(Number) }
			};
			interpreter.ScopeTreeRoot.AddToScope("*", multiply);
			var divide = new CSharpFunction(basicOperators.Divide, Fixity.Infix_5)
			{
				InputSignature = new List<Type>() { typeof(Number), typeof(Number) },
				OutputSignature = new List<Type>() { typeof(Number) }
			};
			interpreter.ScopeTreeRoot.AddToScope("/", divide);
		}

		public static bool CheckArguments(List<Value> values, params Type[] types)
		{
			if (types.Length <= values.Count)
			{
				for (int i = 0; i < types.Length; ++i)
				{
					if (values[i].GetType() != types[i])
					{
						return false;
					}
				}
			}
			return false;
		}
	}
}

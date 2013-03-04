using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hype.SL.Global;
using System.Reflection;

namespace Hype.SL
{
	class StandardLibrary
	{
		public static void Load(Interpreter interpreter)
		{
			var basicOperators = new BasicOperators(interpreter);
			var basicFunctions = new BasicFunctions(interpreter);

			var methods = new object[] { basicFunctions, basicOperators }
				.Select(t => new Tuple<object, MethodInfo[]>(t, t.GetType().GetMethods()))
				.Select(t => t.Item2.Select(m => new Tuple<MethodInfo, object>(m, t.Item1)))
				.Aggregate((a, m) => a.Concat(m).ToArray())
				.Where(m => m.Item1.GetCustomAttributes(typeof(FunctionAttributes), false).Length == 1)
				.OrderBy(m => (m.Item1.GetCustomAttributes(typeof(FunctionAttributes), false)[0] as FunctionAttributes).Priority)
				.ToArray();

			foreach (var methodPair in methods)
			{
				var method = methodPair.Item1;
				var obj = methodPair.Item2;
				var attribs = method.GetCustomAttributes(typeof(FunctionAttributes), false)[0] as FunctionAttributes;
				var arguments = method.GetParameters().Select(p => p.ParameterType).ToArray();
				var returnType = method.ReturnType;

				Func<List<Value>, Value> del = delegate(List<Value> args)
				{
					return (Value)method.Invoke(obj, args.ToArray());
				};

				var func = new CSharpFunction(del, attribs.Fixity, arguments.Length);
				func.Signature.InputSignature.Clear();
				foreach (var type in arguments)
				{
					if (type == typeof(Value)) func.Signature.InputSignature.Add(ValueType.Uncertain);
					else func.Signature.InputSignature.Add(ValueType.GetType(type.Name));
				}
				func.Signature.OutputSignature = ValueType.GetType(returnType == typeof(Value) ? "Uncertain" : returnType.Name);

				Value val;
				if ((val = interpreter.ScopeTreeRoot.LookupNoCache(attribs.Identifier)) is PartialApplication)
				{
					interpreter.ScopeTreeRoot.AddToScope(attribs.Identifier, (val as PartialApplication).Merge(func));
				}
				else
				{
					interpreter.ScopeTreeRoot.AddToScope(attribs.Identifier, func);
				}
			}
		}

		public static bool CheckArguments(List<Value> values, params ValueType[] types)
		{
			if (types.Length <= values.Count)
			{
				for (int i = 0; i < types.Length; ++i)
				{
					if (values[i].Type != types[i])
					{
						return false;
					}
				}
			}
			return false;
		}
	}
}

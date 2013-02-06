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
		private static string signatures = 
@"+ | Add | Number Number | Number | 6
- | Subtract | Number Number | Number | 6
* | Multiply | Number Number | Number | 5
/ | Divide | Number Number | Number | 5
= | Assign | Uncertain Uncertain | Uncertain | 15
";

		public static void Load(Interpreter interpreter)
		{
			var basicOperators = new BasicOperators(interpreter);

			var lines = signatures.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var line in lines)
			{
				var parts = line.Split('|');
				//var method = typeof(BasicOperators).GetMethod(parts[1].Trim(), BindingFlags.Public | BindingFlags.Instance);
				var func = new CSharpFunction((Func<List<Value>, Value>)Delegate.CreateDelegate(typeof(Func<List<Value>,Value>),basicOperators, parts[1].Trim()), (Fixity)int.Parse(parts[4].Trim()));
				func.Signature.InputSignature.Clear();
				func.Signature.OutputSignature.Clear();
				foreach (var input in parts[2].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
				{
					func.Signature.InputSignature.Add(ValueType.GetType(input));
				}
				foreach (var input in parts[3].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
				{
					func.Signature.OutputSignature.Add(ValueType.GetType(input));
				}

				interpreter.ScopeTreeRoot.AddToScope(parts[0].Trim(), func);
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

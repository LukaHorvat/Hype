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
		private static string signaturesOperators =
@"+ | Add | Number Number | Number | 6
- | Subtract | Number Number | Number | 6
* | Multiply | Number Number | Number | 5
/ | Divide | Number Number | Number | 5
< | LessThan | Number Number | Boolean | 8
> | GreaterThan | Number Number | Boolean | 8
<= | LessOrEqual | Number Number | Boolean | 8
>= | GreaterOrEqual | Number Number | Boolean | 8
== | Equal | Number Number | Boolean | 9
!= | NotEqual | Number Number | Boolean | 9
= | Assign | Uncertain Uncertain | Uncertain | 15
";

		private static string signaturesFunctions =
@"for | For | CodeBlock CodeBlock CodeBlock CodeBlock | Void | 0
";

		public static void Load(Interpreter interpreter)
		{
			var basicOperators = new BasicOperators(interpreter);

			var lines = signaturesOperators.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var line in lines)
			{
				var parts = line.Split('|');

				var func = new CSharpFunction((Func<List<Value>, Value>)Delegate.CreateDelegate(typeof(Func<List<Value>, Value>), basicOperators, parts[1].Trim()), (Fixity)int.Parse(parts[4].Trim()));
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

			var basicFunctions = new BasicFunctions(interpreter);

			lines = signaturesFunctions.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var line in lines)
			{
				var parts = line.Split('|');

				var inputTypes = parts[2].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

				var func = new CSharpFunction((Func<List<Value>, Value>)Delegate.CreateDelegate(typeof(Func<List<Value>, Value>), basicFunctions, parts[1].Trim()), (Fixity)int.Parse(parts[4].Trim()), inputTypes.Length);
				func.Signature.InputSignature.Clear();
				func.Signature.OutputSignature.Clear();
				foreach (var input in inputTypes)
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

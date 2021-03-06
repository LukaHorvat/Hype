﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype.SL.Global
{
	class BasicFunctions
	{
		public Interpreter Interpreter;

		public BasicFunctions(Interpreter interpreter)
		{
			Interpreter = interpreter;
		}

		[FunctionAttributes(Hype.Fixity.Prefix, "for")]
		public Void For(CodeBlock init, CodeBlock condition, CodeBlock increment, CodeBlock block)
		{
			init.Execute(Interpreter);
			while (true)
			{
				var b = condition.Execute(Interpreter) as Boolean;
				if (b == null) throw new FunctionCallException("The conditional expression of the for loop didn't produce a Boolean value");
				if (!b.Bool) break;
				block.Execute(Interpreter);
				increment.Execute(Interpreter);
			}
			return Void.Instance;
		}

		[FunctionAttributes(Hype.Fixity.Prefix, "if")]
		public Value If(Boolean condition, CodeBlock block)
		{
			if (condition.Bool)
			{
				return block.Execute(Interpreter);
			}
			return new TypedObject("IfFalse");
		}

		[FunctionAttributes(Hype.Fixity.Infix_18, "else")]
		public Value Else(Value previous, CodeBlock block)
		{
			if (previous.Type == ValueType.IfFalse)
			{
				return block.Execute(Interpreter);
			}
			return previous;
		}

		[FunctionAttributes(Hype.Fixity.Infix_18, "elseIf")]
		public Functional ElseIf(Value previous, Boolean condition)
		{
			if (previous.Type == ValueType.IfFalse)
			{
				if (condition.Bool)
				{
					//If the previous if was false and the current condition is true,
					//return the exec function that will execute the following CodeBlock
					return (Functional)Interpreter.CurrentScopeNode.LookupNoRef("exec");
				}
				else
				{
					//Otherwise, return a function that will consume the next CodeBlock,
					//but also return a IfFalse so the next else can operate on it
					return new CSharpFunction(args => new TypedObject("IfFalse"), Fixity.Prefix, 1);
				}
			}
			else
			{
				//If the previous condition was true, just consume the following CodeBlock
				return (Functional)Interpreter.CurrentScopeNode.LookupNoRef("eat");
			}
		}

		[FunctionAttributes(Hype.Fixity.Prefix, "exec")]
		public Value Exec(CodeBlock block)
		{
			return block.Execute(Interpreter);
		}

		[FunctionAttributes(Hype.Fixity.Prefix, "eat")]
		public Void Eat(Value arg)
		{
			return Void.Instance;
		}

		[FunctionAttributes(Hype.Fixity.Prefix, "printLine")]
		public Void PrintLine(Value arg)
		{
			if (arg is String && ((String)arg).Str == "break")
			{

			}
			Console.WriteLine(arg.Show());
			return Void.Instance;
		}

		[FunctionAttributes(Hype.Fixity.Prefix, "print")]
		public Void Print(Value arg)
		{
			Console.Write(arg.Show());
			return Void.Instance;
		}

		[FunctionAttributes(Hype.Fixity.Prefix, "function")]
		public Function Function(List args, CodeBlock block)
		{
			var functionsScope = new ScopeTreeNode(Permanency.NonPermanent, Interpreter.CurrentScopeNode);
			Func<List<Value>, Value> code = delegate(List<Value> list)
			{
				var tempScope = new ScopeTreeNode(Permanency.NonPermanent, functionsScope.Parent);
				Interpreter.EnterScope(tempScope);
				for (int i = 0; i < list.Count; ++i)
				{
					var name = args.InnerList[i].Var.OriginalName;
					Interpreter.CurrentScopeNode.AddToScope(name, list[i]);
				}
				var ret = block.Execute(Interpreter);
				if (ret.Type <= ValueType.ReturnValue) ret = (ret as ReturnValue).InnerValue;
				Interpreter.ExitScope();

				return ret;
			};
			var fun = new CSharpFunction(code, Fixity.Prefix, args.InnerList.Count) { FunctionScope = functionsScope };
			fun.Signature.InputSignature.Clear();
			args.Select(v => v.Type).ToList().ForEach(t => fun.Signature.InputSignature.Add(t == ValueType.BlankIdentifier ? ValueType.Uncertain : t));

			return fun;
		}

		[FunctionAttributes(Hype.Fixity.Prefix, "type")]
		public UserType Type(CodeBlock block)
		{
			return new UserType(block);
		}

		[FunctionAttributes(Hype.Fixity.Prefix, "new")]
		public Value New(UserType type)
		{
			var obj = new TypedObject(type.Var.OriginalName);
			Interpreter.EnterScope(obj.ScopeNode);
			type.PropertyBlock.Execute(Interpreter);
			Interpreter.ExitScope();
			return obj;
		}

		[FunctionAttributes(Hype.Fixity.Prefix, "return")]
		public ReturnValue Return(Value val)
		{
			return new ReturnValue(val);
		}
	}
}

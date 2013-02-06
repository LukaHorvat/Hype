using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KindFixityPair = System.Tuple<Hype.ValueKind, Hype.Fixity>;
using Hype.SL.Global;

namespace Hype
{
	class Expression : ExpressionItem
	{
		public List<ExpressionItem> Sequence;

		private List<Tuple<ExpressionItem, KindFixityPair>> rpn;
		private Stack<ExpressionItem> operatorStack;

		private List<KindFixityPair> rpnInfo;
		private Stack<Fixity> infoStack;

		private Stack<Value> solvingStack;

		public Expression(Token bracketToken)
			: base(CheckToken(bracketToken))
		{
			Sequence = new List<ExpressionItem>();
			rpnInfo = new List<KindFixityPair>();
			operatorStack = new Stack<ExpressionItem>();
			infoStack = new Stack<Fixity>();
			rpn = new List<Tuple<ExpressionItem, KindFixityPair>>();
			solvingStack = new Stack<Value>();
		}

		public Value Execute(Interpreter interpreter)
		{
			ToRPN(interpreter);
			solvingStack.Clear();
			for (int i = 0; i < rpn.Count; ++i)
			{
				if (rpn[i].Item2.Item1 == ValueKind.Object)
				{
					if (rpn[i].Item1 is Expression)
					{
						//solvingStack.Push(new LazyExpression(rpn[i].Item1));
						solvingStack.Push((rpn[i].Item1 as Expression).Execute(interpreter));
					}
					else if (rpn[i].Item1.OriginalToken.Type == TokenType.Literal)
					{
						solvingStack.Push(interpreter.ParseLiteral(rpn[i].Item1.OriginalToken.Content));
					}
					else
					{
						solvingStack.Push(interpreter.ScopeTreeRoot.Lookup(rpn[i].Item1.OriginalToken.Content));
					}
				}
				else
				{
					Function func;
					if (rpn[i].Item1 is Expression)
					{
						func = CheckSignature((rpn[i].Item1 as Expression).Execute(interpreter) as FunctionGroup, solvingStack);
					}
					else if (rpn[i].Item1 is WrappedValue)
					{
						func = CheckSignature((rpn[i].Item1 as WrappedValue).Val as Function, solvingStack);
					}
					else
					{
						func = CheckSignature(interpreter.ScopeTreeRoot.Lookup(rpn[i].Item1.OriginalToken.Content) as FunctionGroup, solvingStack);
					}
					if (func == null) throw new Exception("Function or function group doesn't match the current arguments.");
					else
					{
						var args = solvingStack.Pop(func.Signature.InputSignature.Count);
						args.Reverse();
						var ret = func.Execute(args);
						if (ret is Function)
						{
							rpn[i] = new Tuple<ExpressionItem, KindFixityPair>(new WrappedValue(ret), new KindFixityPair(ValueKind.Function, (ret as Function).Signature.Fixity));
							i--;	//If the returned value is a fuction, we don't treat it as an object and pust it to the  stack,
							//we instead let that function execute.
						}
						else
						{
							solvingStack.Push(ret);
						}
					}
				}
			}
			if (solvingStack.Count > 1) throw new Exception("Expression evaluated to more than one value");
			return solvingStack.Pop();		
		}

		private Function CheckSignature(Function func, Stack<Value> currentStack)
		{
			if (currentStack.Count < func.Signature.InputSignature.Count) return null;
			var args = currentStack.Take(func.Signature.InputSignature.Count).Reverse().ToList();
			return args.Select((x, i) => new []{x.Type, ValueType.GetType("Uncertain")}.Contains(func.Signature.InputSignature[i])).All(x => x) ? func : null;
		}

		private Function CheckSignature(FunctionGroup group, Stack<Value> currentStack)
		{
			var arguments = new List<Value>();

			Function match = null;
			foreach (var func in group.Functions) //Find a fuction that has a matching signature
			{
				match = CheckSignature(func, currentStack);
				if (match != null) break;
			}
			return match;
		}

		private void ToRPN(Interpreter interpreter)
		{
			bool isObject;
			int precedence = 0;
			var str = interpreter.ScopeTreeRoot;
			rpnInfo.Clear();
			operatorStack.Clear();
			infoStack.Clear();
			rpn.Clear();

			for (int i = 0; i < Sequence.Count; ++i)
			{
				isObject = false;
				if (Sequence[i].OriginalToken.Type == TokenType.Literal)
				{
					isObject = true;
					rpnInfo.Add(new Tuple<ValueKind, Fixity>(ValueKind.Object, Fixity.Prefix));
				}
				else if (Sequence[i] is Expression)
				{
					(Sequence[i] as Expression).ToRPN(interpreter);
					var info = (Sequence[i] as Expression).rpnInfo.Last();
					if (info.Item1 == ValueKind.Object)
					{
						isObject = true;
					}
					else
					{
						precedence = (int)info.Item2;
					}
					rpnInfo.Add(info);
				}
				else
				{
					var val = str.Lookup(Sequence[i].OriginalToken.Content);
					if (val is FunctionGroup)
					{
						var fix = (val as FunctionGroup).Fixity;
						precedence = (int)fix;
						rpnInfo.Add(new Tuple<ValueKind, Fixity>(ValueKind.Function, fix));
					}
					else
					{
						isObject = true;
						rpnInfo.Add(new Tuple<ValueKind, Fixity>(ValueKind.Object, Fixity.Prefix));
					}
				}
				if (isObject) rpn.Add(new Tuple<ExpressionItem, KindFixityPair>(Sequence[i], rpnInfo.Last()));
				else
				{
					if (operatorStack.Count == 0 || (int)infoStack.Peek() > precedence)
					{
						operatorStack.Push(Sequence[i]);
						infoStack.Push((Fixity)precedence);
					}
					else
					{
						while (infoStack.Count > 0 && (int)infoStack.Peek() <= precedence)
						{
							rpn.Add(new Tuple<ExpressionItem, KindFixityPair>(operatorStack.Pop(), new KindFixityPair(ValueKind.Function, infoStack.Pop())));
						}
						operatorStack.Push(Sequence[i]);
						infoStack.Push((Fixity)precedence);
					}
				}
			}
			while (operatorStack.Count > 0)
			{
				rpn.Add(new Tuple<ExpressionItem, KindFixityPair>(operatorStack.Pop(), new KindFixityPair(ValueKind.Function, infoStack.Pop())));
			}
		}

		private static Token CheckToken(Token token)
		{
			if (token.Type == TokenType.Group) return token;
			throw new Exception("You must provide a group token to create a new expression");
		}

		public override string ToString()
		{
			return "Expression: " + OriginalToken;
		}

		public void DebugPrint(int tabLevel = 0)
		{
			PrintTabs(tabLevel);
			foreach (var item in Sequence)
			{
				if (item is Expression)
				{
					Console.WriteLine();
					(item as Expression).DebugPrint(tabLevel + 1);
					Console.WriteLine();
				}
				else
				{
					Console.Write(item.OriginalToken.Content + " ");
				}
			}
		}

		private void PrintTabs(int n)
		{
			for (int i = 0; i < n; ++i) Console.Write('\t');
		}

		public int GetNumTokens()
		{
			return Sequence.Aggregate<ExpressionItem, int>(0, (a, i) => a + (i is Expression ? (i as Expression).GetNumTokens() : 1));
		}
	}
}

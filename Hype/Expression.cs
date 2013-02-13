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

		private List<List<LinkedListNode<Value>>> functionList;
		private Stack<LinkedListNode<Value>> functionStack;
		private LinkedList<Value> currentExecution;

		public Expression(Token bracketToken)
			: base(CheckToken(bracketToken))
		{
			Sequence = new List<ExpressionItem>();

		}

		public Value Execute(Interpreter interpreter)
		{
			int numFixities = Enum.GetNames(typeof(Fixity)).Length;
			functionList = new List<List<LinkedListNode<Value>>>(numFixities);
			for (int i = 0; i < numFixities; ++i) functionList.Add(new List<LinkedListNode<Value>>());

			functionStack = new Stack<LinkedListNode<Value>>();
			currentExecution = new LinkedList<Value>();
			Value val;

			for (int i = 0; i < Sequence.Count; ++i)
			{
				val = null;
				var tok = Sequence[i].OriginalToken;
				switch (tok.Type)
				{
					case TokenType.Literal:
						currentExecution.AddLast(interpreter.ParseLiteral(tok.Content));
						break;
					case TokenType.Identifier:
						val = interpreter.CurrentScopeNode.Lookup(tok.Content);
						break;
					case TokenType.Group:
						val = (Sequence[i] as Expression).Execute(interpreter);
						break;
				}
				if (val != null)
				{
					currentExecution.AddLast(val);
					if (val.Type == ValueType.GetType("FunctionGroup"))
					{
						var fg = (IFunctionGroup)val;
						functionList[(int)fg.Fixity].Add(currentExecution.Last);
					}
				}
			}

			for (int j = functionList.Count - 1; j >= 0; --j) for (int k = functionList[j].Count - 1; k >= 0; --k) functionStack.Push(functionList[j][k]);
			while (functionStack.Count > 0)
			{
				var funcNode = functionStack.Pop();
				var func = funcNode.Value as ICurryable;

				Value res = null;
				Side side;

				{
					IInvokable noArgs;
					if ((noArgs = func.MatchesNoArguments) != null)
					{
						res = noArgs.Execute(new List<Value>());
						side = Side.NoArgument;
					}
				}

				if (func.Fixity != Fixity.Prefix && funcNode.Previous != null)
				{
					side = Side.Left;
					try
					{
						res = func.Apply(funcNode.Previous.Value, Side.Left);
					}
					catch (Exception)
					{
						continue;
					}
				}
				else
				{
					if (funcNode.Next == null)
					{
						side = Side.NoArgument;
						res = (Value)func;
					}
					else
					{
						side = Side.Right;
						try
						{
							res = func.Apply(funcNode.Next.Value, Side.Right);
						}
						catch (Exception)
						{
							continue;
						}
					}
				}
				if (side == Side.Left)
				{
					currentExecution.Remove(funcNode.Previous);
				}
				else if (side == Side.Right)
				{
					currentExecution.Remove(funcNode.Next);
				}
				var node = currentExecution.AddAfter(funcNode, res);
				currentExecution.Remove(funcNode);
				if (res != func && res.Type == ValueType.GetType("FunctionGroup")) functionStack.Push(node);
			}

			return currentExecution.First.Value;
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
			return Sequence.Aggregate<ExpressionItem, int>(0, (a, i) => a + (i is Expression ? (i as Expression).GetNumTokens() : 1)) + 2;
		}
	}
}

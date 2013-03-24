using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hype.SL.Global;

namespace Hype
{
	class Expression : ExpressionItem
	{
		public List<ExpressionItem> Sequence;
		public List<ExecutionNode> Nodes;

		private int numFixities;

		public Expression(Token bracketToken)
			: base(CheckToken(bracketToken))
		{
			Sequence = new List<ExpressionItem>();
			numFixities = Enum.GetNames(typeof(Fixity)).Length;
		}

		public void ParseNodes()
		{
			Nodes = new List<ExecutionNode>();
			var current = new List<ExpressionItem>();
			foreach (var item in Sequence)
			{
				if (item.OriginalToken.Type == TokenType.Separator)
				{
					Nodes.Add(new ExecutionNode(current));
					if (Nodes.Count > 1)
					{
						Nodes[Nodes.Count - 2].Next = Nodes.Last();
					}
					current = new List<ExpressionItem>();
				}
				else
				{
					current.Add(item);
				}
			}
			if (current.Count > 0)
			{
				Nodes.Add(new ExecutionNode(current));
				if (Nodes.Count > 1)
				{
					Nodes[Nodes.Count - 2].Next = Nodes.Last();
				}
			}
		}

		public void GenerateLookupCache(Interpreter interpreter)
		{
			if (Nodes.Count == 0) return;

			for (var execNode = Nodes[0]; execNode != null; execNode = execNode.Next)
			{
				GenerateLookupCache(execNode, interpreter);
			}
		}

		public void GenerateLookupCache(ExecutionNode node, Interpreter interpreter)
		{
			var items = node.InnerExpression;

			if (node.Cache == null)
			{
				node.Cache = new List<Reference>();
				for (int i = 0; i < items.Count; ++i) node.Cache.Add(null);
			}

			for (int i = 0; i < items.Count; ++i)
			{
				//Fixed references aren't meant to be modified.
				if (node.Cache[i] != null && node.Cache[i].Fixed) continue;

				var tok = items[i].OriginalToken;
				switch (tok.Type)
				{
					case TokenType.Literal:
						node.Cache[i] = new Reference(interpreter.ParseLiteral(tok.Content));
						break;
					case TokenType.Identifier:
						node.Cache[i] = interpreter.CurrentScopeNode.Lookup(tok.Content);
						break;
					case TokenType.Group:
						var exp = items[i] as Expression;
						if (exp.OriginalToken.Content == "{")
						{
							var codeblock = new CodeBlock(exp);
							node.Cache[i] = new Reference(codeblock);
						}
						else
						{
							exp.GenerateLookupCache(interpreter);
							exp.FixAllReferences();
							if (exp.OriginalToken.Content == "[")
							{
								node.Cache[i] = new ListCache(exp, interpreter);
							}
							else
							{
								node.Cache[i] = new ExpressionCache(exp, interpreter);
							}
						}
						break;
				}

			}
		}

		/// <summary>
		/// Makes all references in the cache fixed.
		/// </summary>
		public void FixAllReferences()
		{
			Nodes.ForEach(
				node =>
					node.Cache.ForEach(
						r =>
						{ if (r != null) r.Fixed = true; }
					)
			);
		}

		public Value Execute(Interpreter interpreter)
		{
			if (Nodes.Count == 0) return Void.Instance;

			Value lastValue = null;

			for (var execNode = Nodes[0]; execNode != null; execNode = execNode.Next)
			{
				if (execNode.Cache == null)
				{
					GenerateLookupCache(execNode, interpreter);
				}

				var functionList = new List<List<LinkedListNode<Value>>>(numFixities);
				for (int i = 0; i < numFixities; ++i) functionList.Add(new List<LinkedListNode<Value>>());

				var functionStack = new Stack<LinkedListNode<Value>>();
				var currentExecution = new LinkedList<Value>();
				var exceptions = new Queue<FunctionCallException>();

				Value val;
				for (int i = 0; i < execNode.Cache.Count; ++i)
				{
					//If the original lookup (on block creation) didn't find the identifier, the cache will be null. If it's 
					//still null after another lookup, set the value to a blank identifier.
					var r = execNode.Cache[i] ?? interpreter.CurrentScopeNode.Lookup(execNode.InnerExpression[i].OriginalToken.Content);
					if (r == null)
					{
						val = new BlankIdentifier(execNode.InnerExpression[i].OriginalToken.Content);
					}
					else
					{
						val = r.RefValue;
					}
					if (val != null)
					{
						if (val.Type <= ValueType.ReturnValue) return val;
						if (val.Type <= ValueType.ProxyValue) val = (val as ProxyValue).Getter();
						currentExecution.AddLast(val);
						if (val is Functional)
						{
							var fg = (IFunctionGroup)val;
							functionList[(int)fg.Fixity].Add(currentExecution.Last);
						}
						if (val is CodeBlock)
						{
							(val as CodeBlock).Expression.GenerateLookupCache(interpreter);
						}
					}
				}

				for (int j = functionList.Count - 1; j >= 0; --j) for (int k = functionList[j].Count - 1; k >= 0; --k) functionStack.Push(functionList[j][k]);

				while (functionStack.Count > 0)
				{
					var funcNode = functionStack.Pop();

					//In some cases, a function yet to be executed gets eaten by another function
					//as an argument. In that case we just skip it
					if (funcNode.List != currentExecution) continue;

					//In some cases, an object with the type Function might not be an actual function
					//but a blank identifier or something else. We skip those cases
					if (!(funcNode.Value is Functional)) continue;

					if (!(funcNode.Value is ICurryable))
					{
						funcNode.Value = new PartialApplication(funcNode.Value as Function);
					}
					var func = funcNode.Value as ICurryable;

					Value res = null;
					Side side;

					if (func.Fixity != Fixity.Prefix && funcNode.Previous != null)
					{
						side = Side.Left;
						try
						{
							res = func.Apply(funcNode.Previous.Value, Side.Left);
							if (res == SignatureMismatchValue.Instance) continue;
#if DEBUG
							interpreter.Log.Add(LogEntry.Applied(funcNode.Previous.Value, func, res));
							if (interpreter.HeavyDebug) interpreter.Log.Add(LogEntry.State(this.Copy<Expression>()));
#endif
						}
						catch (FunctionCallException e)
						{
							exceptions.Enqueue(e);
#if DEBUG
							interpreter.Log.Add(LogEntry.Caught(e));
							if (interpreter.HeavyDebug) interpreter.Log.Add(LogEntry.State(this.Copy<Expression>()));
#endif
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
								if (res == SignatureMismatchValue.Instance) continue;
#if DEBUG
								interpreter.Log.Add(LogEntry.Applied(funcNode.Next.Value, func, res));
								if (interpreter.HeavyDebug) interpreter.Log.Add(LogEntry.State(this.Copy<Expression>()));
#endif
							}
							catch (FunctionCallException e)
							{
								exceptions.Enqueue(e);
#if DEBUG
								interpreter.Log.Add(LogEntry.Caught(e));
								if (interpreter.HeavyDebug) interpreter.Log.Add(LogEntry.State(this.Copy<Expression>()));
#endif
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
					if (res.Type <= ValueType.ReturnValue) return res;
					if (res != func && res.Type.IsSubtypeOf(ValueType.Functional) && currentExecution.Count > 1) functionStack.Push(node);
				}

				if (currentExecution.Count > 1)
				{
					if (exceptions.Count > 0) throw exceptions.Dequeue();
					else throw new MultipleValuesLeft();
				}
				if (currentExecution.First.Value.Type == ValueType.FunctionGroup
					&& ((IFunctionGroup)currentExecution.First.Value).Fixity != Fixity.Prefix) return (Value)(currentExecution.First.Value as ICurryable).PrefixApplication;
				lastValue = currentExecution.First();
			}
			return lastValue;
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

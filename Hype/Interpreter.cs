using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hype.SL.Global;
using System.IO;

namespace Hype
{
	class Interpreter
	{
		public List<LogEntry> Log;
		public bool HeavyDebug = false;

		public Expression RootExpression;

		/// <summary>
		/// The global node
		/// </summary>
		public ScopeTreeNode ScopeTreeRoot;
		public ScopeTreeNode CurrentScopeNode
		{
			get
			{
				return scopeStack.Peek();
			}
		}

		private Stack<ScopeTreeNode> scopeStack;

		public Interpreter(Expression expression)
		{
			scopeStack = new Stack<ScopeTreeNode>();
			ScopeTreeRoot = new ScopeTreeNode();
			RootExpression = expression;
			Log = new List<LogEntry>();
		}

		public void LoadLibrary(Action<Interpreter> loadFunction)
		{
			loadFunction(this);
		}

		public void Run(bool writeLog = false)
		{
			EnterScope(ScopeTreeRoot);
			RootExpression.Execute(this);
#if DEBUG
			if (writeLog)
			{
				using (var writer = new StreamWriter("log.txt"))
				{
					Log.ForEach(e => writer.WriteLine(e));
				}
			}
#endif
		}

		public void EnterScope(ScopeTreeNode node)
		{
			scopeStack.Push(node);
		}

		public void ExitScope()
		{
			scopeStack.Pop();
		}

		public Value ParseLiteral(string literal)
		{
			if (new[] { "true", "false" }.Contains(literal))
			{
				return new Hype.SL.Global.Boolean(bool.Parse(literal));
			}
			return new Number(int.Parse(literal));
		}
	}
}

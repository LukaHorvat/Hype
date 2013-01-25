using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hype
{
	class Interpreter
	{
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
		}

		public void LoadLibrary(Action<Interpreter> loadFunction)
		{
			loadFunction(this);
		}

		public void Run()
		{

		}

		public void EnterScope(ScopeTreeNode node)
		{
			scopeStack.Push(node);
		}

		public void ExitScope()
		{
			scopeStack.Pop();
		}
	}
}

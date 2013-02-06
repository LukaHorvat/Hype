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

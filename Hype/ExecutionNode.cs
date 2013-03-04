using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class ExecutionNode
	{
		public ExecutionNode Next;
		public List<ExpressionItem> InnerExpression;
		public List<LookupCache> Cache;

		public ExecutionNode(List<ExpressionItem> innerExpression, ExecutionNode next)
		{
			Next = next;
			InnerExpression = innerExpression;
		}

		public ExecutionNode(List<ExpressionItem> innerExpression)
			: this(innerExpression, null) { }
	}
}

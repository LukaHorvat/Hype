using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hype
{
	enum ValueKind
	{
		Function,
		Object,
		Nothing
	}

	class ScopeTreeNode
	{
		public Dictionary<string, List<Value>> Values;
		public Dictionary<string, ValueKind> Kinds;
		public List<ScopeTreeNode> Children;
		public ScopeTreeNode Parent;

		public ScopeTreeNode(ScopeTreeNode parent = null)
		{
			Values = new Dictionary<string, List<Value>>();
			Kinds = new Dictionary<string, ValueKind>();
			Children = new List<ScopeTreeNode>();
			Parent = parent;
		}

		public void AddToScope(string key, Value val)
		{
			ScopeTreeNode scope = SearchScope(key) ?? this;
			scope.AddToThisScope(key, val);
		}

		public ValueKind GetKind(string key)
		{
			ScopeTreeNode scope;
			if ((scope = SearchScope(key)) != null)
			{
				return scope.Kinds[key];
			}
			return ValueKind.Nothing;
		}

		private ScopeTreeNode SearchScope(string key)
		{
			if (Values.ContainsKey(key)) return this;
			else if (Parent != null) return Parent.SearchScope(key);
			else return null;
		}

		private void AddToThisScope(string key, Value val)
		{
			if (!Values.ContainsKey(key)) Values[key] = new List<Value>();
			if (!Kinds.ContainsKey(key)) Kinds[key] = ValueKind.Nothing;

			if (val.GetKind() != Kinds[key])
			{
				Values[key].Clear();
				Values[key].Add(val);
			}
			else if (val.GetKind() == ValueKind.Function)
			{
				Values[key].Add(val);
			}
			else if (val.GetKind() == ValueKind.Object)
			{
				Values[key][0] = val;
			}
		}
	}
}

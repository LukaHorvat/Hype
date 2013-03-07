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
		public Dictionary<string, Value> Values;
		public List<ScopeTreeNode> Children;
		public ScopeTreeNode Parent;

		private Dictionary<string, LookupCache> cacheDictionary;
		public ScopeTreeNode(ScopeTreeNode parent = null)
		{
			Values = new Dictionary<string, Value>();
			Children = new List<ScopeTreeNode>();
			cacheDictionary = new Dictionary<string, LookupCache>();
			Parent = parent;
		}

		public void AddToScope(string key, Value val)
		{
			ScopeTreeNode scope = SearchScope(key) ?? this;
			scope.AddToThisScope(key, val);
		}

		public Value LookupNoCache(string key)
		{
			var scope = SearchScope(key);
			if (scope != null)
			{
				return scope.Values[key];
			}
			else return new BlankIdentifier(key);
		}

		public LookupCache Lookup(string key)
		{
			var scope = SearchScope(key);
			if (scope != null)
			{
				return scope.LookupThis(key);
			}
			else
			{
				return null;
			}
		}

		private LookupCache LookupThis(string key)
		{
			if (cacheDictionary.ContainsKey(key)) return cacheDictionary[key];
			var cache = new LookupCache(Values[key]);
			cacheDictionary[key] = cache;
			return cache;
		}

		private ScopeTreeNode SearchScope(string key)
		{
			if (Values.ContainsKey(key)) return this;
			else if (Parent != null) return Parent.SearchScope(key);
			else return null;
		}

		private void AddToThisScope(string key, Value val)
		{
			Value temp;
			if (val.Var.Names.Count == 0) val.Var.Names.Add(key);

			if (val is Function) temp = Values[key] = new PartialApplication(val as Function) { Var = new Variable(key) };
			else temp = Values[key] = val;
			if (cacheDictionary.ContainsKey(key))
			{
				cacheDictionary[key].Cache = temp;
			}
			else
			{
				cacheDictionary[key] = new LookupCache(temp);
			}
		}

		public void GarbageCollect()
		{
			Values.Clear();
			foreach (var cache in cacheDictionary.Values) cache.Cache = null;
		}
	}
}

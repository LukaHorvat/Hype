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
			if (scope == this)
			{
				if (cacheDictionary.ContainsKey(key)) return cacheDictionary[key];
				var cache = new LookupCache(scope.Values[key]);
				cacheDictionary[key] = cache;
				return cache;
			}
			if (scope != null)
			{
				return scope.Lookup(key);
			}
			else return new LookupCache(new BlankIdentifier(key));
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
<<<<<<< HEAD
			if (val.Type <= ValueType.Function) Values[key] = new PartialApplication(val as Function) { Var = new Variable(key) };
			else Values[key] = val;
=======
			if (val is Function) temp = Values[key] = new PartialApplication(val as Function) { Var = new Variable(key) };
			else temp = Values[key] = val;
			if (cacheDictionary.ContainsKey(key))
			{
				cacheDictionary[key].Cache = temp;
			}
>>>>>>> Attempt at optimizations
		}
	}
}

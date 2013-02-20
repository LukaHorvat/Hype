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

        public ScopeTreeNode(ScopeTreeNode parent = null)
        {
            Values = new Dictionary<string, Value>();
            Children = new List<ScopeTreeNode>();
            Parent = parent;
        }

        public void AddToScope(string key, Value val)
        {
            ScopeTreeNode scope = SearchScope(key) ?? this;
            scope.AddToThisScope(key, val);
        }

        public Value Lookup(string key)
        {
            var scope = SearchScope(key);
			if (scope != null)
			{
				scope.Values[key].Var.Name = key;
				return scope.Values[key];
			}
			else return new BlankIdentifier(key);
        }

        private ScopeTreeNode SearchScope(string key)
        {
            if (Values.ContainsKey(key)) return this;
            else if (Parent != null) return Parent.SearchScope(key);
            else return null;
        }

        private void AddToThisScope(string key, Value val)
        {
			if (val.Var.Name == "") val.Var.Name = key;
			if (val is Function) Values[key] = new PartialApplication(val as Function);
			else Values[key] = val;
        }
    }
}

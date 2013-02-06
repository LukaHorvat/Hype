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
        public Dictionary<string, ValueKind> Kinds;
        public List<ScopeTreeNode> Children;
        public ScopeTreeNode Parent;

        public ScopeTreeNode(ScopeTreeNode parent = null)
        {
            Values = new Dictionary<string, Value>();
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
            if (!Kinds.ContainsKey(key)) Kinds[key] = ValueKind.Nothing;

            var kind = val.Kind;
            if (kind != Kinds[key])
            {
				if (kind == ValueKind.Function)
				{
					Kinds[key] = ValueKind.Function;
					if (val is Function)
					{
						Values[key] = new FunctionGroup(val as Function);
					}
					if (val is FunctionGroup) Values[key] = val;
				}
				else
				{
					Values[key] = val;
				}
            }
            else if (kind == ValueKind.Function)
            {
                var fix = (Values[key] as FunctionGroup).Fixity;
                if (val is Function)
                {
                    (Values[key] as FunctionGroup).AddWithOverride(val as Function);
                }
                if (val is FunctionGroup)
                {
                    (Values[key] as FunctionGroup).MergeOrReplace(val as FunctionGroup);
                }
            }
            else if (val.Kind == ValueKind.Object)
            {
                Values[key] = val;
            }
        }
    }
}

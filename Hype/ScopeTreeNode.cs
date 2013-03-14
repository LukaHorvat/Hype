﻿using System;
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

	enum Permanency
	{
		Permanent,
		NonPermanent
	}

	class ScopeTreeNode
	{
		public List<ScopeTreeNode> Children;
		public ScopeTreeNode Parent;

		/// <summary>
		/// If the node isn't expandable it means that any attempt to create a new reference on it will be passed to it's parent.
		/// </summary>
		public bool Expandable = true;

		private Permanency perm;

		private Dictionary<string, Reference> references;

		public ScopeTreeNode(Permanency perm, ScopeTreeNode parent = null)
		{
			Children = new List<ScopeTreeNode>();
			references = new Dictionary<string, Reference>();
			Parent = parent;
			this.perm = perm;
		}

		public void AddToScope(string key, Value val)
		{
			ScopeTreeNode scope = SearchScope(key) ?? this;
			scope.AddToThisScope(key, val);
		}

		public Value LookupNoRef(string key)
		{
			var scope = SearchScope(key);
			if (scope != null)
			{
				return scope.references[key].RefValue;
			}
			else return new BlankIdentifier(key);
		}

		public Reference Lookup(string key)
		{
			if (key.IndexOf('.') != -1)
			{
				var parts = key.Split('.');
				var scope = this;
				int i = 0;
				Reference r = null;
				while (i < parts.Length)
				{
					r = scope.Lookup(parts[i]);
					if (r == null && parts.Length >= i)
					{
						throw new ExpressionException("Tried to access a field of a null reference");
					}
					scope = r.RefValue.ScopeNode;
					++i;
				}
				return r;
			}
			else
			{
				var scope = SearchScope(key);
				if (scope != null)
				{
					return scope.LookupThis(key);
				}
				return null;
			}
		}

		/// <summary>
		/// Only called when this scope surely contains the key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		private Reference LookupThis(string key)
		{
			return references[key];
		}

		private ScopeTreeNode SearchScope(string key)
		{
			if (references.ContainsKey(key)) return this;
			else if (Parent != null) return Parent.SearchScope(key);
			else return null;
		}

		private void AddToThisScope(string key, Value val)
		{
			bool contains = references.ContainsKey(key);
			if (!contains && !Expandable)
			{
				if (Parent == null) throw new ExpressionException("Unexpandable scope has no parent scope and is trying to add a new reference");
				Parent.AddToThisScope(key, val);
				return;
			}

			if (val.Var.Names.Count == 0) val.Var.Names.Add(key);

			if (!contains) references[key] = new Reference(null);
			var refer = references[key];
			if (val is Function) refer.RefValue = new PartialApplication(val as Function) { Var = new Variable(key) };
			else refer.RefValue = val;
		}

		public void GarbageCollect()
		{
			references.Clear();
		}
	}
}

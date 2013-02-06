using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class ValueType
	{
		public List<ValueType> Subtypes = new List<ValueType>();
		public ValueType Supertype;
		public string TypeName;

		protected ValueType(string name)
		{
			TypeName = name;
		}

		static Dictionary<string, ValueType> types = new Dictionary<string,ValueType>(); //To prevent creation of new types when a value is instanced
		private string p;

		public static ValueType GetType(string name)
		{
			return types.ContainsKey(name) ? types[name] : types[name] = new ValueType(name);
		}

		public bool IsSubtypeOf(ValueType type)
		{
			ValueType current;
			for (current = this; current != null; current = current.Supertype)
			{
				if (current == type) return true;
			}
			return false;
		}

		public override string ToString()
		{
			return TypeName;
		}
	}
}

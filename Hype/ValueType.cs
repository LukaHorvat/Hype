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

		//To avoid unnecessary lookups, some build in types are cached (via reflection in the static intializer)
		public static ValueType BlankIdentifier;
		public static ValueType CodeBlock;
		public static ValueType Collection;
		public static ValueType CSharpFunction;
		public static ValueType Function;
		public static ValueType Functional;
		public static ValueType FunctionGroup;
		public static ValueType Identifier;
		public static ValueType List;
		public static ValueType PartialApplication;
		public static ValueType Void;
		public static ValueType String;
		public static ValueType Number;
		public static ValueType Boolean;
		public static ValueType IfFalse;
		public static ValueType Uncertain;
		public static ValueType ProxyValue;

		protected ValueType(string name)
		{
			TypeName = name;
		}

		public static Dictionary<string, ValueType> Types = new Dictionary<string,ValueType>(); //To prevent creation of new types when a value is instanced

		public static ValueType GetType(string name)
		{
			ValueType type;
			if (!Types.TryGetValue(name, out type))
			{
				return Types[name] = new ValueType(name);
			}else
			{
				return type;
			}
		}

		static ValueType()
		{
			var fields = typeof(ValueType).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).Where(f => f.FieldType == typeof(ValueType));
			foreach (var field in fields)
			{
				field.SetValue(null, GetType(field.Name));
			}

			var func = Functional;
			new[] { "FunctionGroup", "Function" }.ToList().ForEach(s => func.AddSubtype(ValueType.GetType(s)));
			String.AddSubtype(Identifier);
			List.AddSubtype(Collection);
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

		public void AddSubtype(ValueType type)
		{
			Subtypes.Add(type);
			type.Supertype = this;
		}

		public static bool operator <=(ValueType a, ValueType b)
		{
			return a.IsSubtypeOf(b);
		}

		public static bool operator >=(ValueType a, ValueType b)
		{
			return b.IsSubtypeOf(a);
		}
	}
}

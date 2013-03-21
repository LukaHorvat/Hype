using Hype.SL.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class List : Value, IEnumerable<Value>
	{
		public List<Value> InnerList;

		public List(int capacity = 0)
			: base(ValueType.List)
		{
			InnerList = new List<Value>(capacity);
			ScopeNode.AddToScope("count", new ProxyWithSetter<Number>(
				() => new Number(InnerList.Count), 
				n => InnerList = InnerList.Take(n.Num).ToList()));

			var at =  new CSharpFunction(
				l => InnerList[(l[0] as Number).Num], 
				Fixity.Prefix, 1);
			at.Signature.InputSignature[0] = ValueType.Number;
			ScopeNode.AddToScope("at", at);
		}

		public override string Show()
		{
			return "[" + InnerShow() + "]";
		}

		protected string InnerShow(bool toString = false)
		{
			var sb = new StringBuilder();
			for (int i = 0; i < InnerList.Count; ++i)
			{
				sb.Append(toString ? InnerList[i].ToString() : InnerList[i].Show());
				if (i < InnerList.Count - 1) sb.Append(", ");
			}
			return sb.ToString();
		}

		public override string ToString()
		{
			return "List: [" + InnerShow(true) + "]";
		}

		public void Add(Value val)
		{
			InnerList.Add(val);
		}

		public IEnumerator<Value> GetEnumerator()
		{
			return InnerList.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return InnerList.GetEnumerator();
		}
	}
}

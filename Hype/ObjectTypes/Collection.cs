using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class Collection : List
	{
		public Collection(int capacity = 0)
			: base(capacity) { Type = ValueType.Collection; }

		public List ToList()
		{
			var l = new List(InnerList.Count);
			InnerList.ForEach(v => l.InnerList.Add(v));
			return l;
		}

		public override string Show()
		{
			return "⌈" + base.Show() + "⌉";
		}

		public override string ToString()
		{
			return "Collection: " + base.Show();
		}
	}
}

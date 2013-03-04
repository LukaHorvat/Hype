using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class ListCache : LookupCache
	{
		private Expression exp;
		private Interpreter interpreter;

		public override Value Cache
		{
			get
			{
				var temp = exp.Execute(interpreter);
				if (temp.Type <= ValueType.Collection) return (temp as Collection).ToList();
				else return new List(1) { temp };
			}
			set
			{
			}
		}

		public ListCache(Expression exp, Interpreter interpreter)
			:base(Void.Instance)
		{
			this.exp = exp;
			this.interpreter = interpreter;
		}
	}
}

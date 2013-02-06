using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	static class ExtensionMethods
	{
		public static bool MemberwiseEquals<T>(this IList<T> self, IList<T> other) where T : class
		{
			if (self.Count != other.Count) return false;
			if (self.Select((t, i) => other[i] == t).Contains(false)) return false;
			return true;
		}

		public static List<T> Pop<T>(this Stack<T> self, int number)
		{
			var list = new List<T>();
			for (int i = 0; i < number; ++i)
			{
				list.Add(self.Pop());
			}
			return list;
		}
	}
}

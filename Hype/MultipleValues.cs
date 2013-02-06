using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class MultipleValues : Value, IList<Value>
	{
		private List<Value> innerList;

		public MultipleValues()
			:base(ValueType.GetType("MultipleValues"))
		{
			innerList = new List<Value>();
		}

		public MultipleValues(List<Value> list)
			:base(ValueType.GetType("MultipleValues"))
		{
			innerList = list;
		}

		public IEnumerator<Value> GetEnumerator()
		{
			return innerList.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return innerList.GetEnumerator();
		}

		public void Add(Value value)
		{
			innerList.Add(value);
		}

		public int IndexOf(Value item)
		{
			return innerList.IndexOf(item);
		}

		public void Insert(int index, Value item)
		{
			innerList.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			innerList.RemoveAt(index);
		}

		public Value this[int index]
		{
			get
			{
				return innerList[index];
			}
			set
			{
				innerList[index] = value;
			}
		}


		public void Clear()
		{
			innerList.Clear();
		}

		public bool Contains(Value item)
		{
			return innerList.Contains(item);
		}

		public void CopyTo(Value[] array, int arrayIndex)
		{
			innerList.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return innerList.Count; }
		}

		public bool IsReadOnly
		{
			get { return (innerList as IList<Value>).IsReadOnly; }
		}

		public bool Remove(Value item)
		{
			return innerList.Remove(item);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	/// <summary>
	/// Used to cache literals and fixed values in CodeBlocks
	/// </summary>
	class FixedReference : Reference
	{
		public override Value RefValue
		{
			get
			{
				return reference.RefValue;
			}
			set { }
		}

		private Reference reference;

		public FixedReference(Reference val)
			: base(val.RefValue)
		{
			reference = val;
		}
	}
}

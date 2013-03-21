using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class Variable
	{
		private string originalName = "";
		public string OriginalName
		{
			get
			{
				return originalName;
			}
			set
			{
				lastName = value;
				originalName = value;
			}
		}
		private string lastName = "";
		public string LastName
		{
			get
			{
				return lastName;
			}
			set
			{
				if (originalName == "") originalName = value;
				lastName = value;
			}
		}

		public Variable(string name)
		{
			OriginalName = name;
		}

		public Variable()
		{
		}

		public override string ToString()
		{
			return "\"" + OriginalName + "\"";
		}
	}
}

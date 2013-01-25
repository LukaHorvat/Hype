using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hype
{
	class Program
	{
		static void Main(string[] args)
		{
			using (StreamReader reader = new StreamReader("samples/tokenization.hy"))
			{
				var parser = new Parser();
				var output = parser.BuildExpressionTree(parser.Tokenize(parser.Split(reader.ReadToEnd())), 0);
				output.DebugPrint();
			}
		}
	}
}

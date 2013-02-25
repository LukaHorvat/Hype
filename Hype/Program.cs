using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hype.SL;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HypeTests")]

namespace Hype
{
	class Program
	{
		static void Main(string[] args)
		{
			using (StreamReader reader = new StreamReader("samples/overloads.hy"))
			{
				var parser = new Parser();
				var output = parser.BuildExpressionTree(parser.Tokenize(parser.Split(reader.ReadToEnd())), 0);

				var interpreter = new Interpreter(output);
				interpreter.LoadLibrary(StandardLibrary.Load);
				interpreter.Run();
			}
		}
	}
}

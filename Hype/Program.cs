using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hype.SL;
using System.Runtime.CompilerServices;
using System.Diagnostics;

[assembly: InternalsVisibleTo("HypeTests")]

namespace Hype
{
	class Program
	{
		static void Main(string[] args)
		{
			using (StreamReader reader = new StreamReader("samples/types.hy"))
			{
				var parser = new Parser();
				var output = parser.BuildExpressionTree(parser.Tokenize(parser.Split(reader.ReadToEnd())), 0);

				var interpreter = new Interpreter(output);
				interpreter.LoadLibrary(StandardLibrary.Load);
				var timer = Stopwatch.StartNew();
				interpreter.Run();
				timer.Stop();
				Console.WriteLine("Time spent: " + timer.ElapsedMilliseconds);
			}
			Console.WriteLine("Done");
			Console.ReadKey();
		}
	}
}

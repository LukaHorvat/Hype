using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Hype.SL;
using Hype.SL.Global;

namespace Hype
{
	class Tests
	{
		public static void Run()
		{
			using (StreamReader reader = new StreamReader("samples/simple.hy"))
			{
				var parser = new Parser();
				var output = parser.BuildExpressionTree(parser.Tokenize(parser.Split(reader.ReadToEnd())), 0);

				var interpreter = new Interpreter(output);
				interpreter.LoadLibrary(StandardLibrary.Load);
				interpreter.Run();

				if ((interpreter.CurrentScopeNode.Lookup("a") as Number).Num != 5) throw new Exception("Wrong result on a test");
			}

			using (StreamReader reader = new StreamReader("samples/tokenization.hy"))
			{
				var parser = new Parser();
				var output = parser.BuildExpressionTree(parser.Tokenize(parser.Split(reader.ReadToEnd())), 0);

				var interpreter = new Interpreter(output);
				interpreter.LoadLibrary(StandardLibrary.Load);
				interpreter.Run();

				if ((interpreter.CurrentScopeNode.Lookup("a") as Number).Num != 25 ||
					(interpreter.CurrentScopeNode.Lookup("b") as Number).Num != 5) throw new Exception("Wrong result on a test");
			}

			using (StreamReader reader = new StreamReader("samples/wat.hy"))
			{
				var parser = new Parser();
				var output = parser.BuildExpressionTree(parser.Tokenize(parser.Split(reader.ReadToEnd())), 0);

				var interpreter = new Interpreter(output);
				interpreter.LoadLibrary(StandardLibrary.Load);
				interpreter.Run();

				if ((interpreter.CurrentScopeNode.Lookup("a") as Number).Num != 4 ||
					(interpreter.CurrentScopeNode.Lookup("b") as Number).Num != 1) throw new Exception("Wrong result on a test");
			}

			using (StreamReader reader = new StreamReader("samples/curry.hy"))
			{
				var parser = new Parser();
				var output = parser.BuildExpressionTree(parser.Tokenize(parser.Split(reader.ReadToEnd())), 0);

				var interpreter = new Interpreter(output);
				interpreter.LoadLibrary(StandardLibrary.Load);
				interpreter.Run();

				if ((interpreter.CurrentScopeNode.Lookup("b") as Number).Num != 5 ||
					(interpreter.CurrentScopeNode.Lookup("c") as Number).Num != 10 ||
					(interpreter.CurrentScopeNode.Lookup("d") as Number).Num != 5) throw new Exception("Wrong result on a test");
			}
		}
	}
}

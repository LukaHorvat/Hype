using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Hype.SL.Global;
using Hype.SL;
using Hype;

namespace HypeTests
{
	[TestClass]
	public class StandardSamples
	{
		public string SamplesPath = "../../../Hype/bin/Debug/samples/";

		[TestMethod]
		public void Simple()
		{
			using (StreamReader reader = new StreamReader(SamplesPath + "simple.hy"))
			{
				var parser = new Parser();
				var output = parser.BuildExpressionTree(parser.Tokenize(parser.Split(reader.ReadToEnd())), 0);

				var interpreter = new Interpreter(output);
				interpreter.LoadLibrary(StandardLibrary.Load);
				interpreter.Run();

				Assert.IsInstanceOfType(interpreter.CurrentScopeNode.Lookup("a"), typeof(Number));
				Assert.AreEqual(5, (interpreter.CurrentScopeNode.Lookup("a") as Number).Num);
			}
		}

		[TestMethod]
		public void Tokenization()
		{
			using (StreamReader reader = new StreamReader(SamplesPath + "tokenization.hy"))
			{
				var parser = new Parser();
				var output = parser.BuildExpressionTree(parser.Tokenize(parser.Split(reader.ReadToEnd())), 0);

				var interpreter = new Interpreter(output);
				interpreter.LoadLibrary(StandardLibrary.Load);
				interpreter.Run();

				Assert.IsInstanceOfType(interpreter.CurrentScopeNode.Lookup("a"), typeof(Number));
				Assert.IsInstanceOfType(interpreter.CurrentScopeNode.Lookup("b"), typeof(Number));
				Assert.AreEqual(25, (interpreter.CurrentScopeNode.Lookup("a") as Number).Num);
				Assert.AreEqual(5, (interpreter.CurrentScopeNode.Lookup("b") as Number).Num);
			}
		}

		[TestMethod]
		public void Wat()
		{
			using (StreamReader reader = new StreamReader(SamplesPath + "wat.hy"))
			{
				var parser = new Parser();
				var output = parser.BuildExpressionTree(parser.Tokenize(parser.Split(reader.ReadToEnd())), 0);

				var interpreter = new Interpreter(output);
				interpreter.LoadLibrary(StandardLibrary.Load);
				interpreter.Run();

				Assert.IsInstanceOfType(interpreter.CurrentScopeNode.Lookup("a"), typeof(Number));
				Assert.IsInstanceOfType(interpreter.CurrentScopeNode.Lookup("b"), typeof(Number));
				Assert.AreEqual(4, (interpreter.CurrentScopeNode.Lookup("a") as Number).Num);
				Assert.AreEqual(1, (interpreter.CurrentScopeNode.Lookup("b") as Number).Num);
			}
		}

		[TestMethod]
		public void Curry()
		{
			using (StreamReader reader = new StreamReader(SamplesPath + "curry.hy"))
			{
				var parser = new Parser();
				var output = parser.BuildExpressionTree(parser.Tokenize(parser.Split(reader.ReadToEnd())), 0);

				var interpreter = new Interpreter(output);
				interpreter.LoadLibrary(StandardLibrary.Load);
				interpreter.Run();

				Assert.IsInstanceOfType(interpreter.CurrentScopeNode.Lookup("b"), typeof(Number));
				Assert.IsInstanceOfType(interpreter.CurrentScopeNode.Lookup("c"), typeof(Number));
				Assert.IsInstanceOfType(interpreter.CurrentScopeNode.Lookup("d"), typeof(Number));
				Assert.AreEqual(5, (interpreter.CurrentScopeNode.Lookup("b") as Number).Num);
				Assert.AreEqual(10, (interpreter.CurrentScopeNode.Lookup("c") as Number).Num);
				Assert.AreEqual(5, (interpreter.CurrentScopeNode.Lookup("d") as Number).Num);
			}
		}
	}
}

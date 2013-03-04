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

		[TestMethod]
		public void Bool()
		{
			using (StreamReader reader = new StreamReader(SamplesPath + "bool.hy"))
			{
				var parser = new Parser();
				var output = parser.BuildExpressionTree(parser.Tokenize(parser.Split(reader.ReadToEnd())), 0);

				var interpreter = new Interpreter(output);
				interpreter.LoadLibrary(StandardLibrary.Load);
				interpreter.Run();

				Assert.IsInstanceOfType(interpreter.CurrentScopeNode.Lookup("a"), typeof(Hype.SL.Global.Boolean));
				Assert.IsInstanceOfType(interpreter.CurrentScopeNode.Lookup("b"), typeof(Number));
				Assert.IsInstanceOfType(interpreter.CurrentScopeNode.Lookup("c"), typeof(Hype.SL.Global.Boolean));
				Assert.IsInstanceOfType(interpreter.CurrentScopeNode.Lookup("d"), typeof(Hype.SL.Global.Boolean));
				Assert.AreEqual(false, (interpreter.CurrentScopeNode.Lookup("a") as Hype.SL.Global.Boolean).Bool);
				Assert.AreEqual(6, (interpreter.CurrentScopeNode.Lookup("b") as Number).Num);
				Assert.AreEqual(true, (interpreter.CurrentScopeNode.Lookup("c") as Hype.SL.Global.Boolean).Bool);
				Assert.AreEqual(true, (interpreter.CurrentScopeNode.Lookup("d") as Hype.SL.Global.Boolean).Bool);
			}
		}

		[TestMethod]
		public void For()
		{
			using (StreamReader reader = new StreamReader(SamplesPath + "for.hy"))
			{
				var parser = new Parser();
				var output = parser.BuildExpressionTree(parser.Tokenize(parser.Split(reader.ReadToEnd())), 0);

				var interpreter = new Interpreter(output);
				interpreter.LoadLibrary(StandardLibrary.Load);
				interpreter.Run();

				Assert.IsInstanceOfType(interpreter.CurrentScopeNode.Lookup("a"), typeof(Number));
				Assert.IsInstanceOfType(interpreter.CurrentScopeNode.Lookup("i"), typeof(Number));
				Assert.AreEqual(2147483647, (interpreter.CurrentScopeNode.Lookup("a") as Number).Num);
				Assert.AreEqual(31, (interpreter.CurrentScopeNode.Lookup("i") as Number).Num);
			}
		}

		[TestMethod]
		public void If()
		{
			using (StreamReader reader = new StreamReader(SamplesPath + "if.hy"))
			{
				var parser = new Parser();
				var output = parser.BuildExpressionTree(parser.Tokenize(parser.Split(reader.ReadToEnd())), 0);

				var interpreter = new Interpreter(output);
				interpreter.LoadLibrary(StandardLibrary.Load);
				interpreter.Run();

				Assert.IsInstanceOfType(interpreter.CurrentScopeNode.Lookup("a"), typeof(Number));
				Assert.IsInstanceOfType(interpreter.CurrentScopeNode.Lookup("b"), typeof(Number));
				Assert.IsInstanceOfType(interpreter.CurrentScopeNode.Lookup("c"), typeof(Number));
				Assert.AreEqual(3, (interpreter.CurrentScopeNode.Lookup("a") as Number).Num);
				Assert.AreEqual(4, (interpreter.CurrentScopeNode.Lookup("b") as Number).Num);
				Assert.AreEqual(3, (interpreter.CurrentScopeNode.Lookup("c") as Number).Num);
			}
		}

		[TestMethod]
		public void Fact()
		{
			using (StreamReader reader = new StreamReader(SamplesPath + "fact.hy"))
			{
				var parser = new Parser();
				var output = parser.BuildExpressionTree(parser.Tokenize(parser.Split(reader.ReadToEnd())), 0);

				var interpreter = new Interpreter(output);
				interpreter.LoadLibrary(StandardLibrary.Load);
				interpreter.Run();

				Assert.IsInstanceOfType(interpreter.CurrentScopeNode.Lookup("acc"), typeof(Number));
				Assert.AreEqual(2004310016, (interpreter.CurrentScopeNode.Lookup("acc") as Number).Num);
			}
		}

		[TestMethod]
		public void Strings()
		{
			using (StreamReader reader = new StreamReader(SamplesPath + "strings.hy"))
			{
				var parser = new Parser();
				var output = parser.BuildExpressionTree(parser.Tokenize(parser.Split(reader.ReadToEnd())), 0);

				var interpreter = new Interpreter(output);
				interpreter.LoadLibrary(StandardLibrary.Load);
				interpreter.Run();

				Assert.IsInstanceOfType(interpreter.CurrentScopeNode.Lookup("a"), typeof(Hype.SL.Global.String));
				Assert.IsInstanceOfType(interpreter.CurrentScopeNode.Lookup("b"), typeof(Hype.SL.Global.String));
				Assert.AreEqual("testing", (interpreter.CurrentScopeNode.Lookup("a") as Hype.SL.Global.String).Str);
				Assert.AreEqual("strings", (interpreter.CurrentScopeNode.Lookup("b") as Hype.SL.Global.String).Str);
			}
		}

		[TestMethod]
		public void Overloads()
		{
			using (StreamReader reader = new StreamReader(SamplesPath + "overloads.hy"))
			{
				var parser = new Parser();
				var output = parser.BuildExpressionTree(parser.Tokenize(parser.Split(reader.ReadToEnd())), 0);

				var interpreter = new Interpreter(output);
				interpreter.LoadLibrary(StandardLibrary.Load);
				interpreter.Run();

				Assert.IsInstanceOfType(interpreter.CurrentScopeNode.Lookup("a"), typeof(Hype.SL.Global.Number));
				Assert.IsInstanceOfType(interpreter.CurrentScopeNode.Lookup("b"), typeof(Hype.SL.Global.String));
				Assert.AreEqual(5, (interpreter.CurrentScopeNode.Lookup("a") as Hype.SL.Global.Number).Num);
				Assert.AreEqual("testing function overloads", (interpreter.CurrentScopeNode.Lookup("b") as Hype.SL.Global.String).Str);
			}
		}
	}
}

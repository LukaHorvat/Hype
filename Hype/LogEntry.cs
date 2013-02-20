using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hype
{
	class LogEntry
	{
		public readonly string Info;
		public Expression ExpressionState;

		private LogEntry(string msg) { Info = msg; }

		public static LogEntry Applied(Value what, IFunctionGroup to, Value result)
		{
			return new LogEntry("Applied [" + what + "] to [" + to + "] that returned [" + result + "]");
		}

		public static LogEntry Caught(Exception exception)
		{
			return new LogEntry("Caught an exception: " + exception.Message);
		}

		public static LogEntry State(Expression exp)
		{
			return new LogEntry("Captured expressions state.") { ExpressionState = exp };
		}

		public override string ToString()
		{
			return Info;
		}
	}
}

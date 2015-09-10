namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
	using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;

	public class LogListener: TraceListener
	{
		public LogListener(ILogger logger, LogKind putsType)
		{
			Logger = logger;
			PutsType = putsType;

			var map = new Dictionary<LogKind, LogPutDelegate>() {
				{ LogKind.Debug, Logger.Debug },
				{ LogKind.Trace, Logger.Trace },
				{ LogKind.Information, Logger.Information },
				{ LogKind.Warning, Logger.Warning },
				{ LogKind.Error, Logger.Error },
				{ LogKind.Fatal, Logger.Fatal },
			};
			Puts = map[PutsType];
		}

		#region property

		protected ILogger Logger { get; private set; }
		public LogKind PutsType { get; private set; }
		LogPutDelegate Puts { get; set; }

		#endregion

		#region function

		protected void Puts_Impl(string s, int skipFrame = 4, int emptySkipFrame = 2)
		{
			var stackTrace = new StackTrace(true).GetFrames();
			var stackFrame = stackTrace.Skip(skipFrame).FirstOrDefault();
			if(stackFrame == null) {
				stackFrame = new StackFrame(emptySkipFrame);
			}
			Puts(s, null, skipFrame, stackFrame.GetFileName(),stackFrame.GetFileLineNumber(), stackFrame.GetMethod().Name);
		}

		#endregion

		#region TargetTypeList

		public override void WriteLine(string s)
		{
			Puts_Impl(s);
		}

		public override void Write(string s)
		{
			Puts_Impl(s);
		}

		#endregion
	}
}

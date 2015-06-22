namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Extension
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

	public static class NonProcessExtension
	{
		public static TimeLogger CreateTimeLogger(this INonProcess nonProcess, LogKind logKind = LogKind.Trace, object detail = null, int frame = 3, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
		{
			Debug.Assert(nonProcess != null);
			Debug.Assert(nonProcess.Logger != null);
			return new TimeLogger(nonProcess.Logger, logKind, detail, frame, callerFile, callerLine, callerMember);
		}
	}
}

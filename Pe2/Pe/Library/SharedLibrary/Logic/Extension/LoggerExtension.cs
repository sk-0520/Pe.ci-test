namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Extension
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	public class LoggerExtension
	{
		#region function

		public static void SafeDebug(this ILogger logger, string message, object detail = null, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "") { if (logger != null) logger.Debug(message, detail, frame, callerFile, callerLine, callerMember);}
		public static void SafeDebug(this ILogger logger, Exception ex, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "") { if (logger != null) logger.Debug(ex, frame, callerFile, callerLine, callerMember); }
		public static void SafeTrace(this ILogger logger, string message, object detail = null, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "") { if (logger != null) logger.Trace(message, detail, frame, callerFile, callerLine, callerMember); }
		public static void SafeTrace(this ILogger logger, Exception ex, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "") { if (logger != null) logger.Trace(ex, frame, callerFile, callerLine, callerMember); }
		public static void SafeInformation(this ILogger logger, string message, object detail = null, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "") { if (logger != null) logger.Information(message, detail, frame, callerFile, callerLine, callerMember); }
		public static void SafeInformation(this ILogger logger, Exception ex, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "") { if (logger != null) logger.Information(ex, frame, callerFile, callerLine, callerMember); }
		public static void SafeWarning(this ILogger logger, string message, object detail = null, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "") { if (logger != null) logger.Warning(message, detail, frame, callerFile, callerLine, callerMember); }
		public static void SafeWarning(this ILogger logger, Exception ex, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "") { if (logger != null) logger.Warning(ex, frame, callerFile, callerLine, callerMember); }
		public static void SafeError(this ILogger logger, string message, object detail = null, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "") { if (logger != null) logger.Error(message, detail, frame, callerFile, callerLine, callerMember); }
		public static void SafeError(this ILogger logger, Exception ex, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "") { if (logger != null) logger.Error(ex, frame, callerFile, callerLine, callerMember); }
		public static void SafeFatal(this ILogger logger, string message, object detail = null, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "") { if (logger != null) logger.Fatal(message, detail, frame, callerFile, callerLine, callerMember); }
		public static void SafeFatal(this ILogger logger, Exception ex, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "") { if (logger != null) logger.Fatal(ex, frame, callerFile, callerLine, callerMember); }

		#endregion
	}
}

﻿namespace ContentTypeTextNet.Library.SharedLibrary.Logic
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
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

	/// <summary>
	/// 開始と終了を記録するログ。
	/// </summary>
	public class TimeLogger: DisposeFinalizeBase
	{
		#region define

		delegate void PutsDelegate(string message, object detail = null, int frame = 1, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "");

		#endregion

		#region static

		[ThreadStatic]
		static Random random = new Random();
		static PutsDelegate GetPuts(ILogger logger, LogKind logKind)
		{
			Debug.Assert(logger != null);

			switch(logKind) {
				case LogKind.Debug: return logger.Debug;
				case LogKind.Trace: return logger.Trace;
				case LogKind.Information: return logger.Information;
				case LogKind.Warning: return logger.Warning;
				case LogKind.Error: return logger.Error;
				case LogKind.Fatal: return logger.Fatal;
				default:
					throw new NotImplementedException();
			}
		}

		#endregion

		#region variable

		PutsDelegate Puts;

		#endregion

		public TimeLogger(ILogger logger, LogKind logKind = LogKind.Trace, object detail = null, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
		{
			CheckUtility.DebugEnforceNotNull(logger);

			InstanceName = GetType().Name;
			InstanceId = random.Next().ToString("x8");

			Logger = logger;
			LogKind = logKind;
			CallerMember = callerMember;

			Detail = detail;

			Puts = GetPuts(Logger, LogKind);

			InstanceStopWatch = Stopwatch.StartNew();
			Puts(MakeMessage("START"), Detail, frame, callerFile, callerLine, callerMember);
		}

		#region property

		protected object Detail { get; private set; }

		protected ILogger Logger { get; private set; }
		protected LogKind LogKind { get; private set; }

		protected string CallerMember { get; private set; }
		protected string InstanceName { get; private set; }
		protected string InstanceId { get; private set; }
		protected Stopwatch InstanceStopWatch { get; private set; }

		#endregion

		#region DisposeFinalizeBase

		protected override void Dispose(bool disposing)
		{
			if(!IsDisposed) {
				InstanceStopWatch.Stop();
				Puts(MakeMessage("STOP"), Detail);

				Puts = null;
			}

			base.Dispose(disposing);
		}

		#endregion

		#region function

		string MakeMessage(string type)
		{
			return string.Format("[{0}-{1}]: {2} {3} > {4}", InstanceName, InstanceId, CallerMember, type, InstanceStopWatch.Elapsed);
		}

		public void Check(object detail = null, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
		{
			Puts(MakeMessage("CHECK"), Detail, frame, callerFile, callerLine, callerMember);
		}
		#endregion
	}
}

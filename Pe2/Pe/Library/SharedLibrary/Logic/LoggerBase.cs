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
using ContentTypeTextNet.Library.SharedLibrary.Model;

	public abstract class LoggerBase: ILogger
	{
		public LoggerBase()
		{
			LoggerConfig = new LoggerConfigModel();
		}

		#region property 

		/// <summary>
		/// ILogger呼び出し時点で対象とするフレーム番号。
		/// </summary>
		protected virtual int BaseFrame { get { return 1; } }

		#endregion

		#region ILogger

		public LoggerConfigModel LoggerConfig { get; set; }

		public virtual void Debug(string message, object detail = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "")
		{
			if (LoggerConfig.EnabledDebug) {
				CallPuts(LogKind.Debug, message, detail, BaseFrame, file, line, member);
			}
		}

		public virtual void Debug(Exception ex, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "")
		{
			if (LoggerConfig.EnabledDebug) {
				CallPuts(LogKind.Debug, default(string), ex, BaseFrame, file, line, member);
			}
		}

		public virtual void Trace(string message, object detail = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "")
		{
			if (LoggerConfig.EnabledTrace) {
			CallPuts(LogKind.Trace, message, detail, BaseFrame, file, line, member);
			}
		}

		public virtual void Trace(Exception ex, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "")
		{
			if (LoggerConfig.EnabledTrace) {
				CallPuts(LogKind.Trace, default(string), ex, BaseFrame, file, line, member);
			}
		}

		public virtual void Information(string message, object detail = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "")
		{
			if (LoggerConfig.EnabledInformation) {
				CallPuts(LogKind.Information, message, detail, BaseFrame, file, line, member);
			}
		}

		public virtual void Information(Exception ex, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "")
		{
			if (LoggerConfig.EnabledInformation) {
				CallPuts(LogKind.Information, default(string), ex, BaseFrame, file, line, member);
			}
		}

		public virtual void Warning(string message, object detail = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "")
		{
			if (LoggerConfig.EnabledWarning) {
				CallPuts(LogKind.Warning, message, detail, BaseFrame, file, line, member);
			}
		}

		public virtual void Warning(Exception ex, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "")
		{
			if (LoggerConfig.EnabledWarning) {
				CallPuts(LogKind.Warning, default(string), ex, BaseFrame, file, line, member);
			}
		}

		public virtual void Error(string message, object detail = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "")
		{
			if (LoggerConfig.EnabledError) {
				CallPuts(LogKind.Error, message, detail, BaseFrame, file, line, member);
			}
		}

		public virtual void Error(Exception ex, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "")
		{
			if (LoggerConfig.EnabledError) {
				CallPuts(LogKind.Error, default(string), ex, BaseFrame, file, line, member);
			}
		}

		public virtual void Fatal(string message, object detail = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "")
		{
			if (LoggerConfig.EnabledFatal) {
				CallPuts(LogKind.Fatal, message, detail, BaseFrame, file, line, member);
			}
		}

		public virtual void Fatal(Exception ex, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "")
		{
			if (LoggerConfig.EnabledFatal) {
				CallPuts(LogKind.Fatal, default(string), ex, BaseFrame, file, line, member);
			}
		}

		#endregion

		#region function

		/// <summary>
		/// 出力担当。
		/// </summary>
		/// <param name="item"></param>
		protected virtual void Puts(LogItemModel item)
		{
			throw new NotImplementedException();
		}

		protected virtual LogItemModel CreateItem(LogKind logKind, string message, object detail, int frame, string file, int line, string member)
		{
			var result = new LogItemModel() {
				DateTime = DateTime.Now,
				LogKind = logKind,
				StackTrace = new StackTrace(frame + 1, true),
				FilePath = file,
				LineNumber = line,
				Member = member,
			};
			if (string.IsNullOrEmpty(message) && detail != null && detail is Exception) {
				var ex = (Exception)detail;
				result.Member = ex.Message;
				result.Detail = ex;
			} else {
				result.Message = message;
				result.Detail = detail;
			}

			return result;
		}

		protected void CallPuts(LogKind logKind, string message, object detail, int frame, string file, int line, string member)
		{
			var logItem = CreateItem(logKind, message, detail, frame + 1, file, line, member);
			Puts(logItem);
		}

		#endregion
	}
}

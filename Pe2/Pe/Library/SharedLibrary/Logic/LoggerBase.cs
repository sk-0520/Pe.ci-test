namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
	using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

		public virtual void Debug(string message, object detail = null, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
		{
			if (LoggerConfig.EnabledDebug) {
				CallPuts(LogKind.Debug, message, detail, BaseFrame, callerFile, callerLine, callerMember, Assembly.GetCallingAssembly());
			}
		}

		public virtual void Debug(Exception ex, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
		{
			if (LoggerConfig.EnabledDebug) {
				CallPuts(LogKind.Debug, default(string), ex, BaseFrame, callerFile, callerLine, callerMember, Assembly.GetCallingAssembly());
			}
		}

		public virtual void Trace(string message, object detail = null, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
		{
			if (LoggerConfig.EnabledTrace) {
				CallPuts(LogKind.Trace, message, detail, BaseFrame, callerFile, callerLine, callerMember, Assembly.GetCallingAssembly());
			}
		}

		public virtual void Trace(Exception ex, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
		{
			if (LoggerConfig.EnabledTrace) {
				CallPuts(LogKind.Trace, default(string), ex, BaseFrame, callerFile, callerLine, callerMember, Assembly.GetCallingAssembly());
			}
		}

		public virtual void Information(string message, object detail = null, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
		{
			if (LoggerConfig.EnabledInformation) {
				CallPuts(LogKind.Information, message, detail, BaseFrame, callerFile, callerLine, callerMember, Assembly.GetCallingAssembly());
			}
		}

		public virtual void Information(Exception ex, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
		{
			if (LoggerConfig.EnabledInformation) {
				CallPuts(LogKind.Information, default(string), ex, BaseFrame, callerFile, callerLine, callerMember, Assembly.GetCallingAssembly());
			}
		}

		public virtual void Warning(string message, object detail = null, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
		{
			if (LoggerConfig.EnabledWarning) {
				CallPuts(LogKind.Warning, message, detail, BaseFrame, callerFile, callerLine, callerMember, Assembly.GetCallingAssembly());
			}
		}

		public virtual void Warning(Exception ex, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
		{
			if (LoggerConfig.EnabledWarning) {
				CallPuts(LogKind.Warning, default(string), ex, BaseFrame, callerFile, callerLine, callerMember, Assembly.GetCallingAssembly());
			}
		}

		public virtual void Error(string message, object detail = null, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
		{
			if (LoggerConfig.EnabledError) {
				CallPuts(LogKind.Error, message, detail, BaseFrame, callerFile, callerLine, callerMember, Assembly.GetCallingAssembly());
			}
		}

		public virtual void Error(Exception ex, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
		{
			if (LoggerConfig.EnabledError) {
				CallPuts(LogKind.Error, default(string), ex, BaseFrame, callerFile, callerLine, callerMember, Assembly.GetCallingAssembly());
			}
		}

		public virtual void Fatal(string message, object detail = null, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
		{
			if (LoggerConfig.EnabledFatal) {
				CallPuts(LogKind.Fatal, message, detail, BaseFrame, callerFile, callerLine, callerMember, Assembly.GetCallingAssembly());
			}
		}

		public virtual void Fatal(Exception ex, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
		{
			if (LoggerConfig.EnabledFatal) {
				CallPuts(LogKind.Fatal, default(string), ex, BaseFrame, callerFile, callerLine, callerMember, Assembly.GetCallingAssembly());
			}
		}

		#endregion

		#region function

		/// <summary>
		/// ファイル出力。
		/// </summary>
		/// <param name="item"></param>
		protected abstract void PutsFile(LogItemModel item);

		/// <summary>
		/// コンソール出力。
		/// </summary>
		/// <param name="item"></param>
		protected abstract void PutsConsole(LogItemModel item);

		/// <summary>
		/// デバッグ出力。
		/// </summary>
		/// <param name="item"></param>
		protected abstract void PutsDebug(LogItemModel item);

		/// <summary>
		/// カスタム出力。
		/// </summary>
		/// <param name="item"></param>
		protected abstract void PutsCustom(LogItemModel item);

		/// <summary>
		/// 出力担当。
		/// </summary>
		/// <param name="item"></param>
		protected void Puts(LogItemModel item)
		{
			var putsList = new[] {
				new Tuple<bool, Action<LogItemModel>>(LoggerConfig.PutsFile, PutsFile),
				new Tuple<bool, Action<LogItemModel>>(LoggerConfig.PutsConsole, PutsConsole),
#if DEBUG
				new Tuple<bool, Action<LogItemModel>>(LoggerConfig.PutsDebug, PutsDebug),
#endif
				new Tuple<bool, Action<LogItemModel>>(LoggerConfig.PutsCustom, PutsCustom),
			};
			foreach(var puts in putsList.Where(p => p.Item1)) {
				puts.Item2(item);
			}
		}

		protected virtual LogItemModel CreateItem(LogKind logKind, string message, object detail, int frame, string callerFile, int callerLine, string callerMember, Assembly callerAssembly)
		{
			var result = new LogItemModel() {
				DateTime = DateTime.Now,
				LogKind = logKind,
				StackTrace = new StackTrace(frame + 1, true),
				CallerFile = callerFile,
				CallerLine = callerLine,
				CallerMember = callerMember,
				CallerAssembly = callerAssembly,
			};
			if (string.IsNullOrEmpty(message) && detail != null && detail is Exception) {
				var ex = (Exception)detail;
				result.CallerMember = ex.Message;
				result.Detail = ex;
			} else {
				result.Message = message;
				result.Detail = detail;
			}

			return result;
		}

		protected void CallPuts(LogKind logKind, string message, object detail, int frame, string callerFile, int callerLine, string callerMember, Assembly callerAssembly)
		{
			var logItem = CreateItem(logKind, message, detail, frame + 1, callerFile, callerLine, callerMember, callerAssembly);
			Puts(logItem);
		}

		#endregion
	}
}

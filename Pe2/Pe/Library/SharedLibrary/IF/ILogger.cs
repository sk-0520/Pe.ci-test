namespace ContentTypeTextNet.Library.SharedLibrary.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	/// <summary>
	/// LogItemModelを用いたログ出力用IF。
	/// </summary>
	public interface ILogger
	{
		#region property

		LoggerConfigModel LoggerConfig { get; set; }

		#endregion

		#region function

		void Debug(string message, object detail = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "");
		void Debug(Exception ex, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "");
		void Trace(string message, object detail = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "");
		void Trace(Exception ex, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "");
		void Information(string message, object detail = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "");
		void Information(Exception ex, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "");
		void Warning(string message, object detail = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "");
		void Warning(Exception ex, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "");
		void Error(string message, object detail = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "");
		void Error(Exception ex, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "");
		void Fatal(string message, object detail = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "");
		void Fatal(Exception ex, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "");

		#endregion
	}
}

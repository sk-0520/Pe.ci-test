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
		/// <summary>
		/// 出力担当。
		/// <para>Debug, Trace, Information, Warning, Error, Fatalと同じ挙動を行う。</para>
		/// <para>というか内部的にこれを呼び出すべき。</para>
		/// </summary>
		/// <param name="item"></param>
		void Puts(LogItemModel item);

		void Debug(string message, object detail = null, int frame = 1, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "");
		void Trace(string message, object detail = null, int frame = 1, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "");
		void Information(string message, object detail = null, int frame = 1, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "");
		void Warning(string message, object detail = null, int frame = 1, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "");
		void Error(string message, object detail = null, int frame = 1, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "");
		void Fatal(string message, object detail = null, int frame = 1, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1, [CallerMemberName] string member = "");
	}
}

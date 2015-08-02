namespace ContentTypeTextNet.Library.SharedLibrary.Define
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Text;
	using System.Threading.Tasks;

	public delegate void LogPutDelegate(string message, object detail = null, int frame = 1, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "");
}

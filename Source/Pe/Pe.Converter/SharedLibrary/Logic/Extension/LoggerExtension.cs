#pragma warning disable IDE0011 // 波かっこを追加します
/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Extension
{
    public static class LoggerExtension
    {
        #region function

        public static void SafeDebug(this ILogger logger, string message, object detail = null, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "") { if(logger != null) logger.Debug(message, detail, frame, callerFile, callerLine, callerMember); }
        public static void SafeDebug(this ILogger logger, Exception ex, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "") { if(logger != null) logger.Debug(ex, frame, callerFile, callerLine, callerMember); }
        public static void SafeTrace(this ILogger logger, string message, object detail = null, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "") { if(logger != null) logger.Trace(message, detail, frame, callerFile, callerLine, callerMember); }
        public static void SafeTrace(this ILogger logger, Exception ex, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "") { if(logger != null) logger.Trace(ex, frame, callerFile, callerLine, callerMember); }
        public static void SafeInformation(this ILogger logger, string message, object detail = null, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "") { if(logger != null) logger.Information(message, detail, frame, callerFile, callerLine, callerMember); }
        public static void SafeInformation(this ILogger logger, Exception ex, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "") { if(logger != null) logger.Information(ex, frame, callerFile, callerLine, callerMember); }
        public static void SafeWarning(this ILogger logger, string message, object detail = null, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "") { if(logger != null) logger.Warning(message, detail, frame, callerFile, callerLine, callerMember); }
        public static void SafeWarning(this ILogger logger, Exception ex, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "") { if(logger != null) logger.Warning(ex, frame, callerFile, callerLine, callerMember); }
        public static void SafeError(this ILogger logger, string message, object detail = null, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "") { if(logger != null) logger.Error(message, detail, frame, callerFile, callerLine, callerMember); }
        public static void SafeError(this ILogger logger, Exception ex, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "") { if(logger != null) logger.Error(ex, frame, callerFile, callerLine, callerMember); }
        public static void SafeFatal(this ILogger logger, string message, object detail = null, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "") { if(logger != null) logger.Fatal(message, detail, frame, callerFile, callerLine, callerMember); }
        public static void SafeFatal(this ILogger logger, Exception ex, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "") { if(logger != null) logger.Fatal(ex, frame, callerFile, callerLine, callerMember); }

        #endregion
    }
}
#pragma warning restore IDE0011 // 波かっこを追加します

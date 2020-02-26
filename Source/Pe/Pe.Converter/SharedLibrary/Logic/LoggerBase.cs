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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
    internal abstract class LoggerBase: DisposeFinalizeBase, ILogger
    {
        public LoggerBase()
        {
            LoggerConfig = new LoggerConfigModel();
        }

        #region property 

        #endregion

        #region function

        /// <summary>
        /// ストリーム出力。
        /// <para>ストリームのフラッシュまでは面倒を見ない</para>
        /// </summary>
        /// <param name="item"></param>
        protected abstract void PutsStream(LogItemModel item);

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
        protected virtual void Puts(LogItemModel item)
        {
            var putsList = new[] {
                new Tuple<bool, Action<LogItemModel>>(LoggerConfig.PutsStream, PutsStream),
                new Tuple<bool, Action<LogItemModel>>(LoggerConfig.PutsConsole, PutsConsole),
#if DEBUG
                new Tuple<bool, Action<LogItemModel>>(LoggerConfig.PutsDebug, PutsDebug),
#endif
                new Tuple<bool, Action<LogItemModel>>(LoggerConfig.PutsCustom, PutsCustom),
            };
            foreach(var puts in putsList.Where(p => p.Item1).ToArray()) {
                puts.Item2(item);
            }
        }

        protected virtual LogItemModel CreateItem(LogKind logKind, string message, object detail, int frame, string callerFile, int callerLine, string callerMember, Assembly callerAssembly, Thread callerThread)
        {
            var result = new LogItemModel() {
                Timestamp = DateTime.Now,
                LogKind = logKind,
                StackTrace = new StackTrace(frame + 1, true),
                CallerFile = callerFile,
                CallerLine = callerLine,
                CallerMember = callerMember,
                CallerAssembly = callerAssembly,
                CallerThread = callerThread,
            };
            if(string.IsNullOrEmpty(message) && detail != null && detail is Exception) {
                var ex = (Exception)detail;
                result.Message = ex.Message;
                result.Detail = ex.ToString();
            } else {
                result.Message = message;
                result.Detail = detail != null ? detail.ToString() : null;
            }

            return result;
        }

        protected void CallPuts(LogKind logKind, string message, object detail, int frame, string callerFile, int callerLine, string callerMember, Assembly callerAssembly, Thread callerThread)
        {
            var logItem = CreateItem(logKind, message, detail, frame + 1, callerFile, callerLine, callerMember, callerAssembly, callerThread);
            Puts(logItem);
        }

        #endregion

        #region ILogger

        public LoggerConfigModel LoggerConfig { get; set; }

        public virtual void Debug(string message, object detail = null, int frame = 1, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
        {
            if(LoggerConfig.EnabledDebug) {
                CallPuts(LogKind.Debug, message, detail, frame, callerFile, callerLine, callerMember, Assembly.GetCallingAssembly(), Thread.CurrentThread);
            }
        }

        public virtual void Debug(Exception ex, int frame = 1, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
        {
            if(LoggerConfig.EnabledDebug) {
                CallPuts(LogKind.Debug, default(string), ex, frame, callerFile, callerLine, callerMember, Assembly.GetCallingAssembly(), Thread.CurrentThread);
            }
        }

        public virtual void Trace(string message, object detail = null, int frame = 1, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
        {
            if(LoggerConfig.EnabledTrace) {
                CallPuts(LogKind.Trace, message, detail, frame, callerFile, callerLine, callerMember, Assembly.GetCallingAssembly(), Thread.CurrentThread);
            }
        }

        public virtual void Trace(Exception ex, int frame = 1, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
        {
            if(LoggerConfig.EnabledTrace) {
                CallPuts(LogKind.Trace, default(string), ex, frame, callerFile, callerLine, callerMember, Assembly.GetCallingAssembly(), Thread.CurrentThread);
            }
        }

        public virtual void Information(string message, object detail = null, int frame = 1, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
        {
            if(LoggerConfig.EnabledInformation) {
                CallPuts(LogKind.Information, message, detail, frame, callerFile, callerLine, callerMember, Assembly.GetCallingAssembly(), Thread.CurrentThread);
            }
        }

        public virtual void Information(Exception ex, int frame = 1, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
        {
            if(LoggerConfig.EnabledInformation) {
                CallPuts(LogKind.Information, default(string), ex, frame, callerFile, callerLine, callerMember, Assembly.GetCallingAssembly(), Thread.CurrentThread);
            }
        }

        public virtual void Warning(string message, object detail = null, int frame = 1, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
        {
            if(LoggerConfig.EnabledWarning) {
                CallPuts(LogKind.Warning, message, detail, frame, callerFile, callerLine, callerMember, Assembly.GetCallingAssembly(), Thread.CurrentThread);
            }
        }

        public virtual void Warning(Exception ex, int frame = 1, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
        {
            if(LoggerConfig.EnabledWarning) {
                CallPuts(LogKind.Warning, default(string), ex, frame, callerFile, callerLine, callerMember, Assembly.GetCallingAssembly(), Thread.CurrentThread);
            }
        }

        public virtual void Error(string message, object detail = null, int frame = 1, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
        {
            if(LoggerConfig.EnabledError) {
                CallPuts(LogKind.Error, message, detail, frame, callerFile, callerLine, callerMember, Assembly.GetCallingAssembly(), Thread.CurrentThread);
            }
        }

        public virtual void Error(Exception ex, int frame = 1, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
        {
            if(LoggerConfig.EnabledError) {
                CallPuts(LogKind.Error, default(string), ex, frame, callerFile, callerLine, callerMember, Assembly.GetCallingAssembly(), Thread.CurrentThread);
            }
        }

        public virtual void Fatal(string message, object detail = null, int frame = 1, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
        {
            if(LoggerConfig.EnabledFatal) {
                CallPuts(LogKind.Fatal, message, detail, frame, callerFile, callerLine, callerMember, Assembly.GetCallingAssembly(), Thread.CurrentThread);
            }
        }

        public virtual void Fatal(Exception ex, int frame = 1, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
        {
            if(LoggerConfig.EnabledFatal) {
                CallPuts(LogKind.Fatal, default(string), ex, frame, callerFile, callerLine, callerMember, Assembly.GetCallingAssembly(), Thread.CurrentThread);
            }
        }

        #endregion
    }
}

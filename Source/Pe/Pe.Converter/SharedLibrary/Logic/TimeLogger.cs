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
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
    /// <summary>
    /// 開始と終了を記録するログ。
    /// </summary>
    internal class TimeLogger: DisposeFinalizeBase
    {
        #region static

        [ThreadStatic]
        static Random random = new Random();
        static LogPutDelegate GetPuts(ILogger logger, LogKind logKind)
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

        LogPutDelegate Puts;

        #endregion

        public TimeLogger(ILogger logger, string id = "", LogKind logKind = LogKind.Debug, object detail = null, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
        {
            CheckUtility.DebugEnforceNotNull(logger);

            InstanceName = GetType().Name;
            InstanceId = !string.IsNullOrWhiteSpace(id)
                ? id.Trim()
                : random.Next().ToString("x8");

            Logger = logger;
            LogKind = logKind;
            CallerMember = callerMember;

            Detail = detail;

            Puts = GetPuts(Logger, LogKind);

            Puts(MakeMessage("START"), Detail, frame, callerFile, callerLine, callerMember);
            InstanceStopWatch = Stopwatch.StartNew();
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

        #region function

        string MakeMessage(string type)
        {
            if(InstanceStopWatch != null) {
                return string.Format("[{0}-{1}]: {2} {3} > {4}", InstanceName, InstanceId, CallerMember, type, InstanceStopWatch.Elapsed);
            } else {
                return string.Format("[{0}-{1}]: {2} {3}", InstanceName, InstanceId, CallerMember, type);
            }
        }

        public void Check(object detail = null, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
        {
            Puts(MakeMessage("CHECK"), Detail, frame, callerFile, callerLine, callerMember);
        }
        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                InstanceStopWatch.Stop();
                Puts(MakeMessage("END"), Detail);

                Puts = null;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}

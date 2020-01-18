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

namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
    public class LogListener: TraceListener
    {
        public LogListener(ILogger logger, LogKind putsType)
        {
            Logger = logger;
            PutsType = putsType;

            var map = new Dictionary<LogKind, LogPutDelegate>() {
                { LogKind.Debug, Logger.Debug },
                { LogKind.Trace, Logger.Trace },
                { LogKind.Information, Logger.Information },
                { LogKind.Warning, Logger.Warning },
                { LogKind.Error, Logger.Error },
                { LogKind.Fatal, Logger.Fatal },
            };
            Puts = map[PutsType];
        }

        #region property

        protected ILogger Logger { get; private set; }
        public LogKind PutsType { get; private set; }
        LogPutDelegate Puts { get; set; }

        #endregion

        #region function

        protected void PutsCore(string s, int skipFrame = 4, int emptySkipFrame = 2)
        {
            var stackTrace = new StackTrace(true).GetFrames();
            var stackFrame = stackTrace.Skip(skipFrame).FirstOrDefault();
            if(stackFrame == null) {
                stackFrame = new StackFrame(emptySkipFrame);
            }
            Puts(s, null, skipFrame, stackFrame.GetFileName(), stackFrame.GetFileLineNumber(), stackFrame.GetMethod().Name);
        }

        #endregion

        #region TargetTypeList

        public override void WriteLine(string s)
        {
            PutsCore(s);
        }

        public override void Write(string s)
        {
            PutsCore(s);
        }

        #endregion
    }
}

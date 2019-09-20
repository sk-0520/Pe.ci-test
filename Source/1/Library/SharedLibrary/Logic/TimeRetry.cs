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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Define;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
    public class TimeRetry<T>: RetryBase<T>
    {
        #region property

        public DelegateRetryExecute<T> ExecuteFunc { get; set; }

        public TimeSpan WaitTime { get; set; }

        #endregion

        #region function

        protected override bool Execute(int waitCurrentCount, ref T result)
        {
            return ExecuteFunc(waitCurrentCount, ref result);
        }

        protected override void Wait(int waitCurrentCount)
        {
            Thread.Sleep(WaitTime);
        }

        #endregion
    }
}

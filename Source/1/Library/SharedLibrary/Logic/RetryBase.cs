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

namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
    /// <summary>
    /// 定型の再試行処理を行う。
    /// </summary>
    public abstract class RetryBase<T>
    {
        #region variable

        T _result;

        #endregion

        #region

        /// <summary>
        /// 待った回数。
        /// </summary>
        public int WaitCurrentCount { get; protected set; }
        /// <summary>
        /// 最大待ち回数。
        /// </summary>
        public int WaitMaxCount { get; set; }
        /// <summary>
        /// 待ち過ぎた。
        /// </summary>
        public bool WaitOver { get { return WaitMaxCount < WaitCurrentCount; } }

        public T Result { get { return this._result; } }

        #endregion

        #region function

        protected abstract bool Execute(int waitCurrentCount, ref T result);
        protected abstract void Wait(int waitCurrentCount);

        public void Run()
        {
            WaitCurrentCount = 0;
            while(WaitCurrentCount <= WaitMaxCount) {
                var result = Execute(WaitCurrentCount++, ref this._result);
                if(result) {
                    return;
                } else {
                    Wait(WaitCurrentCount);
                }
            }
        }

        #endregion
    }
}

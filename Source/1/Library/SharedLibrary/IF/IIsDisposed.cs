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

namespace ContentTypeTextNet.Library.SharedLibrary.IF
{
    public interface IIsDisposed: IDisposable
    {
        /// <summary>
        /// Dispose時に呼び出されるイベント。
        /// <para>本イベントが呼び出されるとき、IsDisposedはまだfalse。</para>
        /// </summary>
        event EventHandler Disposing;

        /// <summary>
        /// 破棄されたか。
        /// </summary>
        bool IsDisposed { get; }
    }
}

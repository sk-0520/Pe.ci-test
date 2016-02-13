/**
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.PeMain.IF
{
    /// <summary>
    /// クリップボード監視状態保持。
    /// </summary>
    public interface IClipboardWatcher
    {
        /// <summary>
        /// クリップボード監視の設定。
        /// </summary>
        /// <param name="watch">真の場合に監視する</param>
        void ClipboardWatchingChange(bool watch);
        /// <summary>
        /// 監視しているか。
        /// </summary>
        bool ClipboardWatching { get; }
        /// <summary>
        /// クリップボード
        /// </summary>
        bool ClipboardEnabledApplicationCopy { get; }
        /// <summary>
        /// 転送にクリップボードを使用する。
        /// </summary>
        bool UsingClipboard { get; set; }
    }
}

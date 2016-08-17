/*
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

namespace ContentTypeTextNet.Pe.Library.PeData.Define
{
    /// <summary>
    /// クリップボードの型情報。
    /// </summary>
    [Flags]
    public enum ClipboardType
    {
        /// <summary>
        /// 無し。
        /// <para>比較時にのみ使用する感じ。</para>
        /// </summary>
        None = 0x00,
        /// <summary>
        /// プレーンテキスト。
        /// </summary>
        Text = 0x01,
        /// <summary>
        /// 書式付文字列。
        /// </summary>
        Rtf = 0x02,
        /// <summary>
        /// HTML。
        /// </summary>
        Html = 0x04,
        /// <summary>
        /// 画像。
        /// </summary>
        Image = 0x08,
        /// <summary>
        /// ファイル。
        /// </summary>
        Files = 0x10,
        /// <summary>
        /// 全て。
        /// </summary>
        All = Text | Rtf | Html | Image | Files,
    }
}

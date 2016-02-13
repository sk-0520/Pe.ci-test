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
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.PeMain.Define;

namespace ContentTypeTextNet.Pe.PeMain.Data.Temporary
{
    /// <summary>
    /// 圧縮データ。
    /// </summary>
    internal class ArchiveParameter
    {
        #region property

        /// <summary>
        /// 基準からのパス
        /// </summary>
        public string RelativePath { get; set; }
        /// <summary>
        /// 圧縮操作で速度または圧縮サイズのどちらを重視するかどうかを示す値を指定します。
        /// </summary>
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Optimal;
        /// <summary>
        /// アーカイブ種別。
        /// </summary>
        public ArchiveType ArchiveType { get; /*set;*/ } = ArchiveType.Zip;
        /// <summary>
        /// RelativePath がディレクトリの場合にサブディレクトリも検索対象とするか。
        /// </summary>
        public SearchOption SearchOption { get; set; }
        /// <summary>
        /// RelativePath がディレクトリの場合に検索対象とするファイル名のワイルドカード。
        /// </summary>
        public string SearchPattern { get; set; } = "*";

        #endregion

        public string GetFullPath(string baseDirectoryPath)
        {
            return Path.Combine(baseDirectoryPath, RelativePath);
        }

    }
}

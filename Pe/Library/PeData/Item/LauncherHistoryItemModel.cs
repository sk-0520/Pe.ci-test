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
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
    /// <summary>
    /// ランチャーアイテム履歴データ。
    /// </summary>
    [Serializable]
    public class LauncherHistoryItemModel: HistoryItemModel, IDeepClone
    {
        public LauncherHistoryItemModel()
            : base()
        {
            Options = new CollectionModel<string>();
            WorkDirectoryPaths = new CollectionModel<string>();
        }

        /// <summary>
        /// 実行回数。
        /// </summary>
        [DataMember]
        public uint ExecuteCount { get; set; }
        /// <summary>
        /// 最終実行日。
        /// </summary>
        [DataMember]
        public DateTime ExecuteTimestamp { get; set; }

        /// <summary>
        /// オプション。
        /// </summary>
        [DataMember, XmlArray("Options"), XmlArrayItem("Item")]
        public CollectionModel<string> Options { get; set; }

        /// <summary>
        /// 作業ディレクトリ。
        /// </summary>
        [DataMember, XmlArray("WorkDirectoryPaths"), XmlArrayItem("Item")]
        public CollectionModel<string> WorkDirectoryPaths { get; set; }

        #region IDeepClone

        public override IDeepClone DeepClone()
        {
            var result = new LauncherHistoryItemModel() {
                ExecuteCount = this.ExecuteCount,
                CreateTimestamp = this.CreateTimestamp,
                UpdateTimestamp = this.UpdateTimestamp,
                UpdateCount = this.UpdateCount,
            };

            result.WorkDirectoryPaths = new CollectionModel<string>(WorkDirectoryPaths);
            result.Options = new CollectionModel<string>(Options);

            return result;
        }

        #endregion
    }
}

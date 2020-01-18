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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.IF;

namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
    /// <summary>
    /// ランチャーアイテム。
    /// </summary>
    [DataContract, Serializable]
    public class LauncherItemModel: GuidModelBase
    {
        public LauncherItemModel()
            : base()
        { }

        #region property

        /// <summary>
        /// ランチャー種別。
        /// </summary>
        [DataMember, IsDeepClone]
        public LauncherKind LauncherKind { get; set; }
        /// <summary>
        /// 実行時に使用される値。
        /// </summary>
        [DataMember, IsDeepClone]
        public string Command { get; set; }
        /// <summary>
        /// 実行時に渡されるオプション。
        /// </summary>
        [DataMember, IsDeepClone]
        public string Option { get; set; }
        /// <summary>
        /// 実行時の作業ディレクトリ。
        /// </summary>
        [DataMember, IsDeepClone]
        public string WorkDirectoryPath { get; set; }
        /// <summary>
        /// 表示アイコンパス。
        /// </summary>
        [DataMember, IsDeepClone]
        public IconItemModel Icon { get; set; } = new IconItemModel();

        /// <summary>
        /// 実行履歴
        /// </summary>
        [DataMember, IsDeepClone]
        public LauncherHistoryItemModel History { get; set; } = new LauncherHistoryItemModel();

        /// <summary>
        /// コメント
        /// </summary>
        [DataMember, IsDeepClone]
        public string Comment { get; set; }
        /// <summary>
        /// タグ
        /// </summary>
        [DataMember, IsDeepClone]
        public TagItemModel Tag { get; set; } = new TagItemModel();
        /// <summary>
        /// 標準入出力。
        /// </summary>
        [DataMember, IsDeepClone]
        public LauncherStdStreamItemModel StdStream { get; set; } = new LauncherStdStreamItemModel();
        /// <summary>
        /// 管理者として実行。
        /// </summary>
        [DataMember, IsDeepClone]
        public bool Administrator { get; set; }
        /// <summary>
        /// 環境変数。
        /// </summary>
        [DataMember, IsDeepClone]
        public EnvironmentVariablesItemModel EnvironmentVariables { get; set; } = new EnvironmentVariablesItemModel();

        /// <summary>
        /// コマンド入力時の列挙対象か。
        /// <para>完全一致時は設定値に関わらず表示される。</para>
        /// </summary>
        [DataMember, IsDeepClone]
        public bool IsCommandAutocomplete { get; set; }

        #endregion

        #region IName

        /// <summary>
        /// アイテム名称。
        /// </summary>
        [DataMember, XmlAttribute, IsDeepClone]
        public string Name { get; set; }

        #endregion

    }
}

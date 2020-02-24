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
using System.Windows.Media;
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
    /// ランチャーグループ。
    /// </summary>
    [DataContract, Serializable]
    public class LauncherGroupItemModel: GuidModelBase
    {
        public LauncherGroupItemModel()
            : base()
        {
            LauncherItems = new CollectionModel<Guid>();
        }

        #region property

        /// <summary>
        /// グループ種別。
        /// </summary>
        [DataMember, IsDeepClone]
        public GroupKind GroupKind { get; set; }

        /// <summary>
        /// ランチャーアイテム。
        /// </summary>
        [DataMember, XmlArrayItem("Item"), IsDeepClone]
        public CollectionModel<Guid> LauncherItems { get; set; }

        #endregion

        #region IName

        /// <summary>
        /// グループ名称。
        /// </summary>
        [DataMember, XmlAttribute, IsDeepClone]
        public string Name { get; set; }

        /// <summary>
        /// グループアイコン。
        /// </summary>
        [DataMember, IsDeepClone]
        public LauncherGroupIconType GroupIconType { get; set; }

        /// <summary>
        /// グループアイコンの色。
        /// </summary>
        [DataMember, IsDeepClone]
        public Color GroupIconColor { get; set; }

        #endregion

        #region IDeepClone

        //public override void DeepCloneTo(IDeepClone target)
        //{
        //    base.DeepCloneTo(target);

        //    var obj = (LauncherGroupItemModel)target;

        //    obj.Name = Name;
        //    obj.GroupKind = GroupKind;
        //    obj.GroupIconType = GroupIconType;
        //    obj.GroupIconColor = GroupIconColor;
        //    obj.LauncherItems.InitializeRange(LauncherItems);
        //}


        #endregion
    }
}

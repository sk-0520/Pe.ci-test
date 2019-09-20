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
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.Library.PeData.Setting;

namespace ContentTypeTextNet.Pe.PeMain.Data
{
    public class LauncherToolbarDataModel: ItemModelBase
    {
        //private LauncherToolbarDataModel()
        //	: this(new ToolbarItemModel(), new LauncherItemSettingModel(), new LauncherItemCollectionModel(), new LauncherGroupItemCollectionModel())
        //{ }

        public LauncherToolbarDataModel(ToolbarItemModel toolbar, LauncherItemSettingModel launcherItemSetting, LauncherItemCollectionModel item, LauncherGroupItemCollectionModel group)
            : base()
        {
            Toolbar = toolbar;
            LauncherItemSetting = launcherItemSetting;
            LauncherItems = item;
            GroupItems = group;
        }

        //public LauncherToolbarDataModel(LauncherItemSettingModel launcherItemSetting, LauncherItemCollectionModel item, LauncherGroupItemCollectionModel group)
        //	: this(null, launcherItemSetting, item, group)
        //{ }

        #region property

        /// <summary>
        /// ツールバー設定。
        /// </summary>
        public ToolbarItemModel Toolbar { get; private set; }
        /// <summary>
        /// ランチャーアイテム。
        /// </summary>
        public LauncherItemCollectionModel LauncherItems { get; private set; }
        /// <summary>
        /// ランチャー設定。
        /// </summary>
        public LauncherItemSettingModel LauncherItemSetting { get; private set; }
        /// <summary>
        /// グループアイテム。
        /// </summary>
        public LauncherGroupItemCollectionModel GroupItems { get; private set; }

        #endregion
    }
}

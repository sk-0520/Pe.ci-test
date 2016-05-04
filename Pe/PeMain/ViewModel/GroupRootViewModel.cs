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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.Define;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
    public class GroupRootViewModel: GroupViewModelBase<LauncherGroupItemModel>
    {
        #region variable
        CollectionModel<GroupItemViewMode> _nodes;
        #endregion

        public GroupRootViewModel(LauncherGroupItemModel model, LauncherItemCollectionModel items, IAppNonProcess appNonProcess)
            : base(model, appNonProcess)
        {
            Items = items;
        }

        #region proeprty

        LauncherItemCollectionModel Items { get; set; }

        public CollectionModel<GroupItemViewMode> Nodes
        {
            get
            {
                if(this._nodes == null) {
                    var list = new List<GroupItemViewMode>(Model.LauncherItems.Count);
                    foreach(var s in Model.LauncherItems) {
                        if(Items.Contains(s)) {
                            var item = Items[s];
                            list.Add(new GroupItemViewMode(item, AppNonProcess));
                        }
                    }

                    this._nodes = new CollectionModel<GroupItemViewMode>(list);
                }
                return this._nodes;
            }
        }

        public Color GroupIconColor
        {
            get { return Model.GroupIconColor; }
            set { SetModelValue(value); }
        }

        public LauncherGroupIconType GroupIconType
        {
            get { return Model.GroupIconType; }
            set { SetModelValue(value); }
        }

        #endregion

        #region IToolbarNode

        public override ToolbarNodeKind ToolbarNodeKind { get { return ToolbarNodeKind.Group; } }

        public override string Name
        {
            get { return Model.Name; }
            set
            {
                if(SetModelValue(value)) {
                    CallOnPropertyChangeDisplayItem();
                }
            }
        }

        public override bool CanEdit { get { return true; } }

        #endregion
    }
}

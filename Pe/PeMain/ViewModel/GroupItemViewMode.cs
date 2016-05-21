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
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.SharedLibrary.Define;
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
    public class GroupItemViewMode: GroupViewModelBase<LauncherItemModel>
    {
        public GroupItemViewMode(LauncherItemModel model, IAppNonProcess appNonProcess)
            : base(model, appNonProcess)
        { }

        #region property

        public CollectionModel<GroupItemViewMode> Nodes
        {
            get { return null; }
        }

        public override Color GroupIconColor
        {
            get { return Colors.Transparent; }
            set { throw new NotSupportedException(); }
        }

        public override LauncherGroupIconType GroupIconType
        {
            get { return LauncherGroupIconType.Folder; }
            set { throw new NotSupportedException(); }
        }

        #endregion

        #region IToolbarNode

        public override ToolbarNodeKind ToolbarNodeKind { get { return ToolbarNodeKind.Item; } }

        public override BitmapSource Image
        {
            get
            {
                return AppUtility.LoadLauncherItemIcon(IconScale.Small, Model, AppNonProcess.LauncherIconCaching, AppNonProcess);
            }
        }

        public override string Name
        {
            get { return Model.Name; }
            set { throw new NotSupportedException(); }
        }

        public override bool CanEdit { get { return false; } }


        #endregion

        #region GroupViewModelBase

        protected override void CallOnPropertyChangeDisplayItem()
        {
            base.CallOnPropertyChangeDisplayItem();
            OnPropertyChanged(nameof(Image));
        }

        #endregion
    }
}

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
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using System.Windows;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using System.Windows.Controls;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using System.Windows.Shapes;
using ContentTypeTextNet.Library.SharedLibrary.Logic;

namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
    public class LauncherGroupItemViewModel: SingleModelWrapperViewModelBase<LauncherGroupItemModel>
    {
        #region variable

        bool _isChecked;

        #endregion

        public LauncherGroupItemViewModel(LauncherGroupItemModel model)
            : base(model)
        { }

        #region property

        BitmapSource GroupIconImage { get; set; }

        public bool IsChecked
        {
            get { return this._isChecked; }
            set { SetVariableValue(ref this._isChecked, value); }
        }

        public ImageSource GroupIcon
        {
            get
            {
                if(GroupIconImage == null) {
                    GroupIconImage = LauncherGroupUtility.CreateGroupIconImage(Model.GroupIconType, Model.GroupIconColor);
                }

                return GroupIconImage;
            }
        }

        #endregion

        #region function
        #endregion

        #region SingleModelWrapperViewModelBase

        public override string DisplayText { get { return DisplayTextUtility.GetDisplayName(Model); } }

        #endregion
    }
}

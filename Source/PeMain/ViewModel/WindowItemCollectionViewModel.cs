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
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
    public class WindowItemCollectionViewModel: SingleModelWrapperViewModelBase<WindowItemCollectionModel>, IMenuItem
    {
        public WindowItemCollectionViewModel(WindowItemCollectionModel model)
            : base(model)
        { }

        #region command

        public ICommand WindowMenuSelectedCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        AppUtility.ChangeWindowFromWindowList(Model);
                    }
                );

                return result;
            }
        }
        #endregion

        #region IMenuItem

        public string MenuText
        {
            get
            {
                return Model.Name;
            }
        }

        public FrameworkElement MenuIcon
        {
            get
            {
                return null;
            }
        }

        #endregion
    }
}

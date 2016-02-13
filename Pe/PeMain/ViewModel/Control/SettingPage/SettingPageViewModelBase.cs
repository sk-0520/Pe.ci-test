/**
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
namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using ContentTypeTextNet.Library.SharedLibrary.IF;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
    using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
    using ContentTypeTextNet.Pe.PeMain.Data;
    using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
    using ContentTypeTextNet.Pe.PeMain.IF;

    public abstract class SettingPageViewModelBase<TView>: ViewModelBase, IHasAppNonProcess, IHasView<TView>
        where TView : UserControl
    {
        public SettingPageViewModelBase(TView view, IAppNonProcess appNonProcess, SettingNotifyData settingNotifiyItem)
        {
            View = view;
            AppNonProcess = appNonProcess;
            SettingNotifiyItem = settingNotifiyItem;
        }

        #region property

        protected SettingNotifyData SettingNotifiyItem { get; private set; }

        #endregion

        #region IHasAppNonProcess

        public IAppNonProcess AppNonProcess { get; private set; }

        #endregion

        #region IHasView

        public TView View { get; private set; }

        public bool HasView
        {
            get
            {
                return HasViewUtility.GetHasView(this);
            }
        }

        #endregion
    }
}

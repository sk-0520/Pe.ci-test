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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.Define;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
    public abstract class GroupViewModelBase<TModel>: SingleModelWrapperViewModelBase<TModel>, IHasAppNonProcess, IToolbarNode, IRefreshFromViewModel
        where TModel : IModel, ITId<Guid>, IName
    {
        #region variable

        bool _isSelected;
        bool _isColorOpen;

        #endregion

        public GroupViewModelBase(TModel model, IAppNonProcess appNonProcess)
            : base(model)
        {
            AppNonProcess = appNonProcess;
        }

        #region property

        public Guid Id { get { return Model.Id; } }

        public abstract Color GroupIconColor { get; set; }
        public abstract LauncherGroupIconType GroupIconType { get; set; }

        public bool IsColorOpen
        {
            get { return this._isColorOpen; }
            set { SetVariableValue(ref this._isColorOpen, value); }
        }

        #endregion

        #region IToolbarNode

        public abstract ToolbarNodeKind ToolbarNodeKind { get; }
        public bool IsExpanded { get { return true; } }
        public bool IsSelected
        {
            get { return this._isSelected; }
            set { SetVariableValue(ref this._isSelected, value); }
        }
        public abstract string Name { get; set; }
        public abstract bool CanEdit { get; }
        public virtual BitmapSource Image { get { return null; } }

        #endregion

        #region IHasAppNonProcess

        public IAppNonProcess AppNonProcess { get; private set; }

        #endregion

        #region SingleModelWrapperViewModelBase

        public override string DisplayText { get { return DisplayTextUtility.GetDisplayName(Model); } }

        #endregion

        #region IRefreshFromViewModel

        public void Refresh()
        {
            CallOnPropertyChangeDisplayItem();
        }

        #endregion
    }
}

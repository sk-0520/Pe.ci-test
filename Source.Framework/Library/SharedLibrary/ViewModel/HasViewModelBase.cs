/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

namespace ContentTypeTextNet.Library.SharedLibrary.ViewModel
{
    public abstract class HasViewModelBase<TView>: ViewModelBase, IHasView<TView>
        where TView : UIElement
    {
        public HasViewModelBase(TView view)
        {
            View = view;
            InitializeView();
        }

        #region function

        protected virtual void InitializeView()
        { }

        protected virtual void UninitializeView()
        { }

        #endregion

        #region IHavingView

        public TView View { get; private set; }
        public bool HasView { get { return HasViewUtility.GetHasView(this); } }

        #endregion

        #region ViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                View = null;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}

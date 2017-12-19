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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.Library.SharedLibrary.ViewModel
{
    public abstract class HasViewSingleModelWrapperViewModelBase<TModel, TView>: SingleModelWrapperViewModelBase<TModel>, IHasView<TView>
        where TModel : IModel
        where TView : UIElement
    {
        #region variable

        //protected EventHandlerDisposer _closedEvent = null; 

        #endregion

        public HasViewSingleModelWrapperViewModelBase(TModel model, TView view)
            : base(model)
        {
            View = view;
            if(HasView) {
                InitializeView();
            }
        }

        #region property

        public TView View { get; private set; }
        public bool HasView { get { return HasViewUtility.GetHasView(this); } }
        public bool IsClosed { get; private set; }

        #endregion

        #region function

        protected virtual void InitializeView()
        {
            Debug.Assert(View != null);
            IsClosed = false;
            var vm = this as IHasView<Window>;
            if(vm != null && HasView) {
                vm.View.Closed += View_Closed;
            }
        }

        void UninitializeViewCore()
        {
            Debug.Assert(HasView);

            UninitializeView();
        }

        protected virtual void UninitializeView()
        {
            Debug.Assert(HasView);
        }

        #endregion

        #region SingleModelWrapperViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                View = null;
            }

            base.Dispose(disposing);
        }

        #endregion

        void View_Closed(object sender, EventArgs e)
        {
            Debug.Assert(HasView);
            IsClosed = true;
            var vm = (IHasView<Window>)this;
            vm.View.Closed -= View_Closed;
            UninitializeViewCore();
        }
    }
}

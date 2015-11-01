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
namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ContentTypeTextNet.Library.SharedLibrary.IF;
    using ContentTypeTextNet.Library.SharedLibrary.Model;
    using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
    using ContentTypeTextNet.Pe.PeMain.IF;

    public abstract class HavingViewSingleModelWrapperBodyViewModelBase<TIndexItemModelBase, TIndexBodyItemModel>: SingleModelWrapperViewModelBase<TIndexItemModelBase>, IHavingAppSender, IHavingAppNonProcess
        where TIndexItemModelBase: class, IModel
        where TIndexBodyItemModel: IModel, IIsDisposed
    {
        #region variable

        TIndexBodyItemModel _bodyModel;

        #endregion

        public HavingViewSingleModelWrapperBodyViewModelBase(TIndexItemModelBase model, IAppSender appSender, IAppNonProcess appNonProcess)
            : base(model)
        {
            WeakModel = new WeakReference(Model);

            AppSender = appSender;
            AppNonProcess = appNonProcess;
        }

        #region property

        /// <summary>
        /// Model自体は上位のインデックス統括で管理しているため緩くてOK。
        /// </summary>
        WeakReference WeakModel { get; set; }

        #endregion

        #region SingleModelWrapperViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(this._bodyModel != null) {
                    this._bodyModel.Disposing -= Body_Disposing;
                    this._bodyModel = default(TIndexBodyItemModel);
                }
            }
            base.Dispose(disposing);
        }

        public override TIndexItemModelBase Model
        {
            get
            {
                if(base.Model != null) {
                    base.Model = (TIndexItemModelBase)WeakModel.Target;
                }

                return base.Model;
            }

            protected set
            {
                base.Model = value;
            }
        }

        #endregion

        #region IHavingAppSender

        public IAppSender AppSender { get; private set; }

        #endregion

        #region  IHavingAppNonProcess

        public IAppNonProcess AppNonProcess { get; private set; }

        #endregion

        private void Body_Disposing(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}

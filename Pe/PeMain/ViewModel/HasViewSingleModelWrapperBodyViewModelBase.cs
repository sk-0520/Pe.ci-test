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
    using ContentTypeTextNet.Pe.Library.PeData.Define;
    using ContentTypeTextNet.Pe.Library.PeData.Item;
    using Define;
    using ContentTypeTextNet.Pe.PeMain.IF;

    public abstract class HasViewSingleModelWrapperBodyViewModelBase<TIndexItemModelBase, TIndexBodyItemModel>: SingleModelWrapperViewModelBase<TIndexItemModelBase>, IHasAppSender, IHasAppNonProcess
        where TIndexItemModelBase: IndexItemModelBase
        where TIndexBodyItemModel: IndexBodyItemModelBase
    {
        #region variable

        TIndexBodyItemModel _bodyModel;

        #endregion

        public HasViewSingleModelWrapperBodyViewModelBase(TIndexItemModelBase model, IAppSender appSender, IAppNonProcess appNonProcess)
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

        protected abstract IndexKind IndexKind { get; }

        protected bool IsLoadedBodyModel
        {
            get { return this._bodyModel != null; }
        }

        protected TIndexBodyItemModel BodyModel
        {
            get
            {
                if(!IsLoadedBodyModel) {
                    this._bodyModel = LoadBodyItem();
                    CorrectionBodyModel(this._bodyModel);
                    this._bodyModel.Disposing += Body_Disposing;
                    if(IsDisposed) {
                        // 再度読み込まれた
                        IsDisposed = false;
                    }
                }

                return this._bodyModel;
            }
        }

        #endregion

        #region function

        /// <summary>
        /// 本体部読み込み時に補正を行う。
        /// </summary>
        protected virtual void CorrectionBodyModel(TIndexBodyItemModel bodyModel)
        { }

        protected virtual TIndexBodyItemModel LoadBodyItem()
        {
            var rawBody = AppSender.SendLoadIndexBody(IndexKind, Model.Id);
            var body = (TIndexBodyItemModel)rawBody;
            return body;
        }

        public virtual void SaveBody(Timing timing)
        {
            if(!IsLoadedBodyModel) {
                // 読み込んでない。
                return;
            }
            
            BodyModel.History.Update();
            AppNonProcess.Logger.Information("save body:" + Model.Name, BodyModel);
            AppSender.SendSaveIndexBody(BodyModel, Model.Id, timing);

            ResetChangeFlag();
        }


        #endregion

        #region SingleModelWrapperViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(this._bodyModel != null) {
                    this._bodyModel.Disposing -= Body_Disposing;
                    this._bodyModel = null;
                }
            }

            base.Dispose(disposing);
        }

        public override TIndexItemModelBase Model
        {
            get
            {
                if(base.Model == null) {
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

        #region  IHasAppNonProcess

        public IAppNonProcess AppNonProcess { get; private set; }

        #endregion

        protected void Body_Disposing(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}

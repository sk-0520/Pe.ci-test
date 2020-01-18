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
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.Library.SharedLibrary.ViewModel
{
    /// <summary>
    /// 単一モデルを取り込むVM。
    /// </summary>
    /// <typeparam name="TModel">モデル。</typeparam>
    public abstract class SingleModelWrapperViewModelBase<TModel>: ViewModelBase
        where TModel : IModel
    {
        /// <summary>
        /// モデルを取り込んで生成。
        /// </summary>
        /// <param name="model">取り込むモデル</param>
        public SingleModelWrapperViewModelBase(TModel model)
            : base()
        {
            Model = model;
            InitializeModel();
        }

        #region property

        /// <summary>
        /// モデル。
        /// </summary>
        public virtual TModel Model { get; protected set; }

        #endregion

        #region function

        /// <summary>
        /// モデル初期化。
        /// </summary>
        protected virtual void InitializeModel()
        { }

        /// <summary>
        /// モデル変更用ヘルパ。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">変更データ。</param>
        /// <param name="targetMemberName">対象オブジェクトのメンバ名。</param>
        /// <param name="notifyPropertyName"></param>
        /// <param name="propertyCheck"></param>
        /// <returns>変更があった場合は真を返す。</returns>
        protected bool SetModelValue<T>(T value, [CallerMemberName] string targetMemberName = "", [CallerMemberName] string notifyPropertyName = "", PropertyCheck propertyCheck = PropertyCheck.None)
        {
            return SetPropertyValue(Model, value, targetMemberName, notifyPropertyName, propertyCheck);
        }

        #endregion

        #region ViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Model = default(TModel);
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}

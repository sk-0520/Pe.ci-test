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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;

namespace ContentTypeTextNet.Library.SharedLibrary.Data
{
    /// <summary>
    /// モデルとビューモデルのペア。
    /// </summary>
    /// <typeparam name="TModel">モデルの型。</typeparam>
    /// <typeparam name="TViewModel">モデルに対応するビューモデルの型。</typeparam>
    public struct MVMPair<TModel, TViewModel>
        where TModel : ModelBase
        where TViewModel : ViewModelBase
    {
        #region variable

        TModel _model;
        TViewModel _viewModel;

        #endregion

        public MVMPair(TModel model, TViewModel viewModel)
        {
            this._model = model;
            this._viewModel = viewModel;
        }

        #region property

        /// <summary>
        /// モデル。
        /// </summary>
        public TModel Model { get { return this._model; } }
        /// <summary>
        /// ビューモデル。
        /// </summary>
        public TViewModel ViewModel { get { return this._viewModel; } }

        #endregion

        #region object

        public override string ToString()
        {
            return $"{nameof(Model)}=>{Model},{nameof(ViewModel)}=>{ViewModel}";
        }

        #endregion
    }

    /// <summary>
    /// <see cref="MVMPair{TModel, TViewModel}"/>生成のヘルパ。
    /// </summary>
    public static class MVMPair
    {
        public static MVMPair<TModel, TViewModel> Create<TModel, TViewModel>(TModel model, TViewModel viewModel)
            where TModel : ModelBase
            where TViewModel : ViewModelBase
        {
            return new MVMPair<TModel, TViewModel>(model, viewModel);
        }

        public static MVMPair<TModel, TViewModel> Create<TModel, TViewModel>(TViewModel viewModel)
            where TModel : ModelBase
            where TViewModel : SingleModelWrapperViewModelBase<TModel>
        {
            return new MVMPair<TModel, TViewModel>(viewModel.Model, viewModel);
        }

    }
}

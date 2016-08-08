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
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Item;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityCore
{
    /// <summary>
    /// ボディデータ初期化処理既定。
    /// </summary>
    /// <typeparam name="TModel">設定データ。</typeparam>
    internal abstract class InitializeBodyBase<TModel>: InitializeBase<TModel>
        where TModel : IndexBodyItemModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">モデル。</param>
        /// <param name="isCreate">new で生成したか。</param>
        /// <param name="nonProcess"></param>
        public InitializeBodyBase(TModel model, bool isCreate, INonProcess nonProcess)
            :base(model, model.PreviousVersion, nonProcess)
        {
            IsCreate = isCreate;
        }

        #region property

        /// <summary>
        /// new で生成したデータか。
        /// <para>前回バージョンのデータ構造を追加したため初回の null 判定が出来ないためこれで代用。</para>
        /// </summary>
        public bool IsCreate { get; }

        #endregion

        #region function

        protected virtual void Correction_FirstCore()
        { }

        #endregion

        #region InitializeBase

        protected sealed override void Correction_First()
        {
            if(!IsCreate) {
                NonProcess.Logger.Trace($"Correction skip: {typeof(TModel).Name} - first");
                return;
            }

            Correction_FirstCore();
        }

        #endregion
    }
}

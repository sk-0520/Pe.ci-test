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

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityCore
{
    /// <summary>
    /// 設定データ初期化処理既定。
    /// </summary>
    /// <typeparam name="TModel">設定データ。</typeparam>
    internal abstract class InitializeBase<TModel>
        where TModel: ModelBase
    {
        public InitializeBase(TModel model, Version previousVersion, INonProcess nonProcess)
        {
            Model = model;
            PreviousVersion = previousVersion;
            NonProcess = nonProcess;
        }

        #region property

        /// <summary>
        /// データモデル。
        /// </summary>
        protected TModel Model { get; }
        /// <summary>
        /// 前回バージョン。
        /// </summary>
        protected Version PreviousVersion { get; }
        /// <summary>
        /// 
        /// </summary>
        protected INonProcess NonProcess { get; }

        #endregion

        /// <summary>
        /// 補正処理実施。
        /// </summary>
        public void Correction()
        {
            V_First();
            V_Upper();
            V_Last();
        }

        /// <summary>
        /// 最終補正処理。
        /// </summary>
        protected abstract void V_LastCore();
        void V_Last()
        {
            V_LastCore();
        }

        /// <summary>
        /// 初回データ補正。
        /// <para>前回バージョンがない場合にのみ実施される。</para>
        /// </summary>
        protected abstract void V_FirstCore();
        void V_First()
        {
            if(PreviousVersion != null) {
                return;
            }

            V_FirstCore();
        }

        void V_Upper()
        {
            V_0_70_0();
        }

        /// <summary>
        /// 設定データが 0.70.0.40764 以下のバージョン補正。
        /// </summary>
        protected virtual void V_0_70_0Core()
        { }
        void V_0_70_0()
        {
            if(new Version(0, 70, 0, 40764) < PreviousVersion) {
                return;
            }

            V_0_70_0Core();
        }
    }
}

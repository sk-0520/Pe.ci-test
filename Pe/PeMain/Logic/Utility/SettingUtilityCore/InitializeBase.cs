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
        where TModel : ModelBase
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

        #region function

        protected static bool IsIllegalPlusNumber(double number)
        {
            return double.IsNaN(number) || number <= 0;
        }

        protected static bool IsIllegalPlusNumber(int number)
        {
            return number <= 0;
        }

        protected static bool IsIllegalString(string s)
        {
            return s == null;
        }

        /// <summary>
        /// 補正処理実施。
        /// </summary>
        public void Correction()
        {
            Correction_FirstCore();
            Correction_Versions();
            Correction_LastCore();
        }

        /// <summary>
        /// 最終補正処理。
        /// </summary>
        protected abstract void Correction_Last();
        void Correction_LastCore()
        {
            Correction_Last();
        }

        /// <summary>
        /// 初回データ補正。
        /// <para>前回バージョンがない場合にのみ実施される。</para>
        /// </summary>
        protected abstract void Correction_First();
        void Correction_FirstCore()
        {
            if(PreviousVersion != null) {
                return;
            }

            NonProcess.Logger.Trace($"Correction: {typeof(TModel).Name} - first");

            Correction_First();
        }

        void Correction_Versions()
        {
            CheckAndCorrection(0, 65, 0, 43015, Correction_0_65_0);
            CheckAndCorrection(0, 69, 0, 38641, Correction_0_69_0);
            CheckAndCorrection(0, 70, 0, 40764, Correction_0_70_0);
            CheckAndCorrection(0, 71, 0, 27279, Correction_0_71_0);
            CheckAndCorrection(0, 77, 0, 340, Correction_0_77_0);
            CheckAndCorrection(0, 78, 0, 27501, Correction_0_78_0);
        }

        /// <summary>
        /// 指定バージョンは補正対象バージョンかチェックし、補正対象であれば補正処理を呼び出す。
        /// </summary>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        /// <param name="build"></param>
        /// <param name="revision"></param>
        /// <param name="action">補正処理。</param>
        void CheckAndCorrection(int major, int minor, int build, int revision, Action action)
        {
            var targetVersion = new Version(major, minor, build, revision);
            if(targetVersion < PreviousVersion) {
                return;
            }

            NonProcess.Logger.Trace($"Correction: {typeof(TModel).Name} - {targetVersion}");

            action();
        }

        /// <summary>
        /// 設定データが 0.65.0 以下のバージョン補正。
        /// </summary>
        protected virtual void Correction_0_65_0()
        { }

        /// <summary>
        /// 設定データが 0.69.0 以下のバージョン補正。
        /// </summary>
        protected virtual void Correction_0_69_0()
        { }

        /// <summary>
        /// 設定データが 0.70.0 以下のバージョン補正。
        /// </summary>
        protected virtual void Correction_0_70_0()
        { }


        /// <summary>
        /// 設定データが 0.71.0 以下のバージョン補正。
        /// </summary>
        protected virtual void Correction_0_71_0()
        { }

        /// <summary>
        /// 設定データが 0.77.0 以下のバージョン補正。
        /// </summary>
        protected virtual void Correction_0_77_0()
        { }

        /// <summary>
        /// 設定データが 0.78.0 以下のバージョン補正。
        /// </summary>
        protected virtual void Correction_0_78_0()
        { }

        #endregion
    }
}

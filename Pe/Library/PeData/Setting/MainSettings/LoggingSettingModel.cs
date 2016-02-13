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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Pe.Library.PeData.IF;
using ContentTypeTextNet.Pe.Library.PeData.Item;

namespace ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings
{
    [Serializable]
    public class LoggingSettingModel: SettingModelBase, IWindowStatus, IDeepClone
    {
        public LoggingSettingModel()
            : base()
        { }

        #region property

        /// <summary>
        /// ログ追加時にUIを表示るか。
        /// </summary>
        [DataMember, IsDeepClone]
        public bool AddShow { get; set; }
        /// <summary>
        /// デバッグ時に表示する
        /// </summary>
        [DataMember, IsDeepClone]
        public bool ShowTriggerDebug { get; set; }
        /// <summary>
        /// トレース時に表示する
        /// </summary>
        [DataMember, IsDeepClone]
        public bool ShowTriggerTrace { get; set; }
        /// <summary>
        /// 情報時に表示する
        /// </summary>
        [DataMember, IsDeepClone]
        public bool ShowTriggerInformation { get; set; }
        /// <summary>
        /// 警告時に表示する
        /// </summary>
        [DataMember, IsDeepClone]
        public bool ShowTriggerWarning { get; set; }
        /// <summary>
        /// エラー時に表示する
        /// </summary>
        [DataMember, IsDeepClone]
        public bool ShowTriggerError { get; set; }
        /// <summary>
        /// 致命時に表示する
        /// </summary>
        [DataMember, IsDeepClone]
        public bool ShowTriggerFatal { get; set; }

        /// <summary>
        /// 詳細部分を折り返し表示するか。
        /// </summary>
        [DataMember, IsDeepClone]
        public bool DetailWordWrap { get; set; }

        #endregion

        #region IWindowStatus

        [DataMember, IsDeepClone]
        [PixelKind(Px.Logical)]
        public double WindowTop { get; set; }
        [DataMember, IsDeepClone]
        [PixelKind(Px.Logical)]
        public double WindowLeft { get; set; }
        [DataMember, IsDeepClone]
        [PixelKind(Px.Logical)]
        public double WindowWidth { get; set; }
        [DataMember, IsDeepClone]
        [PixelKind(Px.Logical)]
        public double WindowHeight { get; set; }
        [DataMember, IsDeepClone]
        [PixelKind(Px.Logical)]
        public WindowState WindowState { get; set; }

        #region ITopMost

        [DataMember, IsDeepClone]
        public bool IsTopmost { get; set; }

        #endregion

        #region IVisible

        [DataMember, IsDeepClone]
        public bool IsVisible { get; set; }

        #endregion

        #endregion

        #region IDeepClone

        public IDeepClone DeepClone()
        {
            return DeepCloneUtility.Copy(this);
        }

        #endregion
    }
}

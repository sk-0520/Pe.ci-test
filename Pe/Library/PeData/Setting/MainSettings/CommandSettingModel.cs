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
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.IF;
using ContentTypeTextNet.Pe.Library.PeData.Item;

namespace ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings
{
    [Serializable]
    public class CommandSettingModel: SettingModelBase, IDeepClone
    {
        public CommandSettingModel()
            : base()
        {
            ShowHotkey = new HotKeyModel();
            Font = new FontModel();
        }

        #region property

        /// <summary>
        /// アイコンサイズ。
        /// </summary>
        [DataMember]
        public IconScale IconScale { get; set; }
        /// <summary>
        /// 非表示になるまでの時間。
        /// </summary>
        [DataMember]
        public TimeSpan HideTime { get; set; }

        /// <summary>
        /// 呼び出しホットキー。
        /// </summary>
        [DataMember]
        public HotKeyModel ShowHotkey { get; set; }

        /// <summary>
        /// タグを検索対象にする。
        /// </summary>
        [DataMember]
        public bool FindTag { get; set; }
        /// <summary>
        /// ファイル検索を有効にする。
        /// </summary>
        [DataMember]
        public bool FindFile { get; set; }

        [DataMember]
        [PixelKind(Px.Logical)]
        public double WindowWidth { get; set; }

        [DataMember]
        public FontModel Font { get; set; }

        #endregion

        #region IDeepClone

        public void DeepCloneTo(IDeepClone target)
        {
            var obj = (CommandSettingModel)target;

            obj.IconScale = IconScale;
            obj.HideTime = HideTime;
            //ShowHotkey.DeepCloneTo(obj.ShowHotkey);
            obj.ShowHotkey = (HotKeyModel)ShowHotkey.DeepClone();
            obj.FindTag = FindTag;
            obj.FindFile = FindFile;
            obj.WindowWidth = WindowWidth;
            //Font.DeepCloneTo(obj.Font);
            obj.Font = (FontModel)Font.DeepClone();
        }

        public IDeepClone DeepClone()
        {
            var result = new CommandSettingModel();

            DeepCloneTo(result);

            return result;
        }

        #endregion
    }
}

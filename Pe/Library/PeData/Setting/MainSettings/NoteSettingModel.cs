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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.IF;

namespace ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings
{
    public class NoteSettingModel: SettingModelBase, IColorPair, IDeepClone
    {
        public NoteSettingModel()
            : base()
        {
            CreateHotKey = new HotKeyModel();
            HideHotKey = new HotKeyModel();
            CompactHotKey = new HotKeyModel();
            ShowFrontHotKey = new HotKeyModel();

            Font = new FontModel();
        }

        #region property

        /// <summary>
        /// 新規作成時のホットキー
        /// </summary>
        [DataMember]
        public HotKeyModel CreateHotKey { get; set; }
        [DataMember]
        public HotKeyModel HideHotKey { get; set; }
        [DataMember]
        public HotKeyModel CompactHotKey { get; set; }
        [DataMember]
        public HotKeyModel ShowFrontHotKey { get; set; }

        [DataMember]
        public FontModel Font { get; set; }

        [DataMember]
        public NoteTitle NoteTitle { get; set; }

        #endregion

        #region IColorPair

        [DataMember]
        public Color ForeColor { get; set; }
        [DataMember]
        public Color BackColor { get; set; }

        #endregion

        #region IDeepClone

        public void DeepCloneTo(IDeepClone target)
        {
            var obj = (NoteSettingModel)target;

            CreateHotKey.DeepCloneTo(obj.CreateHotKey);
            HideHotKey.DeepCloneTo(obj.HideHotKey);
            CompactHotKey.DeepCloneTo(obj.CompactHotKey);
            ShowFrontHotKey.DeepCloneTo(obj.ShowFrontHotKey);
            obj.ForeColor = ForeColor;
            obj.BackColor = BackColor;
            //Font.DeepCloneTo(obj.Font);
            obj.Font = (FontModel)Font.DeepClone();
            obj.NoteTitle = NoteTitle;
        }

        public IDeepClone DeepClone()
        {
            var result = new NoteSettingModel();

            DeepCloneTo(result);

            return result;
        }

        #endregion
    }
}

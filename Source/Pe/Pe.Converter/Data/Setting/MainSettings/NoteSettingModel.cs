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
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.IF;

namespace ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings
{
    [DataContract, Serializable]
    public class NoteSettingModel: SettingModelBase, IColorPair, ITopMost
    {
        public NoteSettingModel()
            : base()
        { }

        #region property

        /// <summary>
        /// 新規作成時のホットキー
        /// </summary>
        [DataMember, IsDeepClone]
        public HotKeyModel CreateHotKey { get; set; } = new HotKeyModel();
        [DataMember, IsDeepClone]
        public HotKeyModel HideHotKey { get; set; } = new HotKeyModel();
        [DataMember, IsDeepClone]
        public HotKeyModel CompactHotKey { get; set; } = new HotKeyModel();
        [DataMember, IsDeepClone]
        public HotKeyModel ShowFrontHotKey { get; set; } = new HotKeyModel();

        [DataMember, IsDeepClone]
        public FontModel Font { get; set; } = new FontModel();

        [DataMember, IsDeepClone]
        public NoteTitle NoteTitle { get; set; }

        [DataMember, IsDeepClone]
        public bool AutoLineFeed { get; set; }

        [DataMember, IsDeepClone]
        public NoteKind NoteKind { get; set; }

        #endregion

        #region IColorPair

        [DataMember, IsDeepClone]
        public Color ForeColor { get; set; }
        [DataMember, IsDeepClone]
        public Color BackColor { get; set; }

        #endregion

        #region ITopMost

        [DataMember, IsDeepClone]
        public bool IsTopmost { get; set; }

        #endregion

        #region IDeepClone

        //public void DeepCloneTo(IDeepClone target)
        //{
        //    var obj = (NoteSettingModel)target;

        //    //CreateHotKey.DeepCloneTo(obj.CreateHotKey);
        //    //HideHotKey.DeepCloneTo(obj.HideHotKey);
        //    //CompactHotKey.DeepCloneTo(obj.CompactHotKey);
        //    //ShowFrontHotKey.DeepCloneTo(obj.ShowFrontHotKey);
        //    obj.CreateHotKey = (HotKeyModel)CreateHotKey.DeepClone();
        //    obj.HideHotKey = (HotKeyModel)HideHotKey.DeepClone();
        //    obj.CompactHotKey = (HotKeyModel)CompactHotKey.DeepClone();
        //    obj.ShowFrontHotKey = (HotKeyModel)ShowFrontHotKey.DeepClone();
        //    obj.ForeColor = ForeColor;
        //    obj.BackColor = BackColor;
        //    //Font.DeepCloneTo(obj.Font);
        //    obj.Font = (FontModel)Font.DeepClone();
        //    obj.NoteTitle = NoteTitle;
        //    obj.IsTopmost = IsTopmost;
        //    obj.AutoLineFeed = AutoLineFeed;
        //    obj.NoteKind = NoteKind;
        //}

        #endregion
    }
}

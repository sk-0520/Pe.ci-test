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
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.IF;

namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
    /// <summary>
    /// ツールバーデータ。
    /// </summary>
    [Serializable]
    public class ToolbarItemModel: ModelBase, IVisible, ITopMost, ITId<string>, IDeepClone
    {
        public ToolbarItemModel()
            : base()
        {
            FloatToolbar = new FloatToolbarItemModel();
            Font = new FontModel();
        }

        #region property

        /// <summary>
        /// ツールバー位置。
        /// </summary>
        [DataMember]
        public DockType DockType { get; set; }

        /// <summary>
        /// フロート状態。
        /// </summary>
        [DataMember]
        public FloatToolbarItemModel FloatToolbar { get; set; }
        /// <summary>
        /// アイコンサイズ。
        /// </summary>
        [DataMember]
        public IconScale IconScale { get; set; }
        /// <summary>
        /// 自動的に隠すか。
        /// </summary>
        [DataMember]
        public bool AutoHide { get; set; }
        /// <summary>
        /// 自動的に隠すまでの時間。
        /// </summary>
        [DataMember]
        public TimeSpan HideWaitTime { get; set; }
        /// <summary>
        /// 自動的に隠す際のアニメーション時間。
        /// </summary>
        [DataMember]
        public TimeSpan HideAnimateTime { get; set; }
        /// <summary>
        /// 初期値として使用するグループID。
        /// </summary>
        [DataMember]
        public Guid DefaultGroupId { get; set; }

        /// <summary>
        /// テキスト表示を行うか。
        /// </summary>
        [DataMember]
        public bool TextVisible { get; set; }
        /// <summary>
        /// テキストの表示幅。
        /// </summary>
        [DataMember]
        public double TextWidth { get; set; }
        /// <summary>
        /// フォント情報。
        /// </summary>
        [DataMember]
        public FontModel Font { get; set; }

        /// <summary>
        /// メニューボタンの位置を補正する。
        /// <para>右側にツールバー設置時に左側にメニューボタンを配置する。</para>
        /// </summary>
        [DataMember]
        public bool MenuPositionCorrection { get; set; }

        /// <summary>
        /// ボタン位置。
        /// </summary>
        [DataMember]
        public ToolbarButtonPosition ButtonPosition { get; set; }

        /// <summary>
        /// メニューボタンを表示するか。
        /// </summary>
        [DataMember]
        public bool VisibleMenuButton { get; set; }

        #endregion

        #region IVisible

        /// <summary>
        /// 表示。
        /// </summary>
        [DataMember]
        public bool IsVisible { get; set; }

        #endregion

        #region ITopMost

        /// <summary>
        /// 最前面表示。
        /// </summary>
        [DataMember]
        public bool IsTopmost { get; set; }

        #endregion

        #region IId

        /// <summary>
        /// ツールバーの所属ディスプレイ名。
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        public bool IsSafeId(string s)
        {
            return !string.IsNullOrEmpty(s);
        }

        public string ToSafeId(string s)
        {
            if(string.IsNullOrEmpty(s)) {
                return "id";
            }

            return s;
        }

        #endregion

        #region IDeepClone

        public void DeepCloneTo(IDeepClone target)
        {
            var obj = (ToolbarItemModel)target;

            obj.DockType = DockType;
            //FloatToolbar.DeepCloneTo(obj.FloatToolbar);
            obj.FloatToolbar = (FloatToolbarItemModel)FloatToolbar.DeepClone();
            obj.IconScale = IconScale;
            obj.AutoHide = AutoHide;
            obj.HideWaitTime = HideWaitTime;
            obj.HideAnimateTime = HideAnimateTime;
            obj.DefaultGroupId = DefaultGroupId;
            obj.TextVisible = TextVisible;
            obj.TextWidth = TextWidth;
            obj.IsVisible = IsVisible;
            obj.IsTopmost = IsTopmost;
            obj.MenuPositionCorrection = MenuPositionCorrection;
            obj.ButtonPosition = ButtonPosition;
            obj.Id = Id;
            //Font.DeepCloneTo(obj.Font);
            obj.Font = (FontModel)Font.DeepClone();
        }

        public IDeepClone DeepClone()
        {
            var result = new ToolbarItemModel();

            DeepCloneTo(result);

            return result;
        }

        #endregion
    }
}

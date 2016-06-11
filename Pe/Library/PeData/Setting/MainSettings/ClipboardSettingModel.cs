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
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.IF;
using ContentTypeTextNet.Pe.Library.PeData.Item;

namespace ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings
{
    [Serializable]
    public class ClipboardSettingModel: SettingModelBase, IWindowStatus, IDeepClone
    {
        public ClipboardSettingModel()
            : base()
        {
            ToggleHotKey = new HotKeyModel();
            Font = new FontModel();
            LimitSize = new ClipboardLimitSizeItemModel();
        }

        #region property

        /// <summary>
        /// クリップボード監視の変更を検知するか。
        /// </summary>
        [DataMember]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// アプリケーション内でのコピー操作も監視対象とするか。
        /// </summary>
        [DataMember]
        public bool IsEnabledApplicationCopy { get; set; }

        /// <summary>
        /// 表示非表示切り替えキー。
        /// </summary>
        [DataMember]
        public HotKeyModel ToggleHotKey { get; set; }

        /// <summary>
        /// 取り込み対象。
        /// </summary>
        [DataMember]
        public ClipboardType CaptureType { get; set; }
        [DataMember]
        public ClipboardLimitSizeItemModel LimitSize { get; set; }

        /// <summary>
        /// 履歴数。
        /// </summary>
        [DataMember]
        public int SaveCount { get; set; }

        /// <summary>
        /// 待機時間。
        /// </summary>
        [DataMember]
        public TimeSpan WaitTime { get; set; }

        /// <summary>
        /// 重複判定。
        /// </summary>
        [DataMember]
        public int DuplicationCount { get; set; }

        /// <summary>
        /// 転送にクリップボードを使用する。
        /// </summary>
        [DataMember]
        public bool UsingClipboard { get; set; }

        /// <summary>
        /// リスト部の幅。
        /// </summary>
        [DataMember]
        public double ItemsListWidth { get; set; }

        /// <summary>
        /// フォント。
        /// </summary>
        [DataMember]
        public FontModel Font { get; set; }

        /// <summary>
        /// 重複アイテムを上へ移動するか。
        /// </summary>
        [DataMember]
        public bool DuplicationMoveHead { get; set; }

        /// <summary>
        /// アイテムダブルクリック時の処理。
        /// </summary>
        [DataMember]
        public IndexItemsDoubleClickBehavior DoubleClickBehavior { get; set; }

        #endregion

        #region IWindowStatus

        [DataMember]
        [PixelKind(Px.Logical)]
        public double WindowTop { get; set; }
        [DataMember]
        [PixelKind(Px.Logical)]
        public double WindowLeft { get; set; }
        [DataMember]
        [PixelKind(Px.Logical)]
        public double WindowWidth { get; set; }
        [DataMember]
        [PixelKind(Px.Logical)]
        public double WindowHeight { get; set; }
        [DataMember]
        [PixelKind(Px.Logical)]
        public WindowState WindowState { get; set; }

        #region ITopMost

        [DataMember]
        public bool IsTopmost { get; set; }

        #endregion

        #region IVisible

        [DataMember]
        public bool IsVisible { get; set; }

        #endregion

        #endregion

        #region IDeepClone

        public void DeepCloneTo(IDeepClone target)
        {
            var obj = (ClipboardSettingModel)target;

            obj.IsEnabled = IsEnabled;
            obj.IsEnabledApplicationCopy = IsEnabledApplicationCopy;
            //ToggleHotKey.DeepCloneTo(obj.ToggleHotKey);
            obj.ToggleHotKey = (HotKeyModel)ToggleHotKey.DeepClone();
            obj.CaptureType = CaptureType;
            obj.SaveCount = SaveCount;
            obj.WaitTime = WaitTime;
            obj.DuplicationCount = DuplicationCount;
            obj.UsingClipboard = UsingClipboard;
            obj.ItemsListWidth = ItemsListWidth;
            obj.WindowTop = WindowTop;
            obj.WindowLeft = WindowLeft;
            obj.WindowWidth = WindowWidth;
            obj.WindowHeight = WindowHeight;
            obj.WindowState = WindowState;
            obj.IsTopmost = IsTopmost;
            obj.IsVisible = IsVisible;
            obj.DuplicationMoveHead = DuplicationMoveHead;
            //Font.DeepCloneTo(obj.Font);
            obj.Font = (FontModel)Font.DeepClone();
            LimitSize.DeepCloneTo(obj.LimitSize);
            obj.DoubleClickBehavior = DoubleClickBehavior;
        }

        public IDeepClone DeepClone()
        {
            var result = new ClipboardSettingModel();

            DeepCloneTo(result);

            return result;
        }

        #endregion
    }
}

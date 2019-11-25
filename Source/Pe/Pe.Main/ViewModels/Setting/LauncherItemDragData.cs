using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class LauncherItemDragData
    {
        public LauncherItemDragData(LauncherItemWithIconViewModel<CommonLauncherItemViewModel> item, bool fromAllItems)
        {
            Item = item;
            FromAllItems = fromAllItems;
        }

        #region property

        /// <summary>
        /// 運んでるデータ。
        /// </summary>
        public LauncherItemWithIconViewModel<CommonLauncherItemViewModel> Item { get; }
        /// <summary>
        /// ランチャーアイテム一覧から運んでいるか。
        /// </summary>
        public bool FromAllItems { get; }

        #endregion
    }
}

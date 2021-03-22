namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class LauncherItemDragData
    {
        public LauncherItemDragData(LauncherItemSettingEditorViewModel item, bool fromAllItems)
        {
            Item = item;
            FromAllItems = fromAllItems;
        }

        #region property

        /// <summary>
        /// 運んでるデータ。
        /// </summary>
        public LauncherItemSettingEditorViewModel Item { get; }
        /// <summary>
        /// ランチャーアイテム一覧から運んでいるか。
        /// </summary>
        public bool FromAllItems { get; }

        #endregion
    }
}

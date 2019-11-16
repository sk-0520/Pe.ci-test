using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    /// <summary>
    /// キーボード処理の種別。
    /// <para>修飾キー単体での起動は行わない方針。</para>
    /// </summary>
    public enum KeyActionKind
    {
        /// <summary>
        /// キー置き換え。
        /// <para>マッピングデータは起点となる一つのみ。</para>
        /// <para>アクション内容は置き換えキー。</para>
        /// </summary>
        Replace,
        /// <summary>
        /// キー無効化。
        /// <para>無効化するが二重入力で活性とする。</para>
        /// <para>マッピングデータは起点となる一つのみ。</para>
        /// </summary>
        Disable,
        /// <summary>
        /// コマンド型ランチャーの起動。
        /// </summary>
        Command,
        /// <summary>
        /// ランチャーアイテムの起動。
        /// <para>アクション内容は起動する方法。</para>
        /// <para>アクションオプションは起動するアイテム。</para>
        /// </summary>
        LauncherItem,
        /// <summary>
        /// ランチャーツールバーの処理。
        /// <para>アクション内容は処理内容。</para>
        /// <para>オプション内容はまちまち。</para>
        /// </summary>
        LauncherToolbar,
        /// <summary>
        /// ノートの処理。
        /// <para>アクション内容は処理内容。</para>
        /// </summary>
        Note,
    }

    /// <summary>
    /// <see cref="KeyActionKind.LauncherItem"/>で処理する内容。
    /// </summary>
    public enum KeyActionContentLauncherItem
    {
        /// <summary>
        /// 実行。
        /// </summary>
        Execute,
        /// <summary>
        /// 指定して実行。
        /// </summary>
        ExtendsExecute,
    }

    public enum KeyActionContentLauncherToolbar
    {
        /// <summary>
        /// 自動的に隠すツールバーを隠す。
        /// </summary>
        AutoHiddenToHide,
    }

    /// <summary>
    /// <see cref="KeyActionKind.Note"/>で処理する内容。
    /// </summary>
    public enum KeyActionContentNote
    {
        /// <summary>
        /// 新規ノートの作成。
        /// </summary>
        Create,
        /// <summary>
        /// 表示中ノートの最前面移動。
        /// </summary>
        ZOrderTop,
        /// <summary>
        /// 表示中ノートの最後面移動。
        /// </summary>
        ZOrderBottom,
    }

    /// <summary>
    /// 修飾キーの左右位置。
    /// </summary>
    public enum ModifierKey
    {
        /// <summary>
        /// なし
        /// </summary>
        None,
        /// <summary>
        /// いずれか。
        /// </summary>
        Any,
        /// <summary>
        /// 左。
        /// </summary>
        Left,
        /// <summary>
        /// 右。
        /// </summary>
        Right,
        /// <summary>
        /// 両方。
        /// </summary>
        All,
    }

    public interface IReadOnlyKeyActionCommonData
    {
        #region property

        Guid KeyActionId { get; }
        KeyActionKind KeyActionKind { get; }

        #endregion
    }

    public class KeyActionCommonData : DataBase, IReadOnlyKeyActionCommonData
    {
        #region IReadOnlyKeyActionCommonData

        public Guid KeyActionId { get; set; }
        public KeyActionKind KeyActionKind { get; set; }

        #endregion
    }

    public interface IReadOnlyKeyActionReplaceData : IReadOnlyKeyActionCommonData
    {
        #region property

        /// <summary>
        /// 置き換えキー。
        /// NOTE: IReadOnlyKeyMappingData にするか悩んだけどいいだろもう
        /// </summary>
        Key ReplaceKey { get; }

        #endregion
    }

    public class KeyActionReplaceData : KeyActionCommonData, IReadOnlyKeyActionReplaceData
    {
        #region IReadOnlyKeyActionReplaceData

        /// <summary>
        /// <see cref="IReadOnlyKeyActionReplaceData.ReplaceKey"/>
        /// </summary>
        public Key ReplaceKey { get; set; }

        #endregion
    }

    public interface IReadOnlyKeyActionDisableData : IReadOnlyKeyActionCommonData
    {
        #region property

        /// <summary>
        /// 完全に無視するか。
        /// </summary>
        bool StopEnable { get; }

        #endregion
    }

    public class KeyActionDisableData : KeyActionCommonData, IReadOnlyKeyActionDisableData
    {
        #region IReadOnlyKeyActionDisableData

        public bool StopEnable {get;set;}

        #endregion
    }

    public interface IReadOnlyKeyActionPressedData : IReadOnlyKeyActionCommonData
    { }

    public class KeyActionPressedData : KeyActionCommonData, IReadOnlyKeyActionPressedData
    { }

    public interface IReadOnlyKeyActionLauncherItemData : IReadOnlyKeyActionPressedData
    {
        #region property
        #endregion
    }

    public class KeyActionLauncherItemData : KeyActionPressedData, IReadOnlyKeyActionLauncherItemData
    {
        #region IReadOnlyKeyActionLauncherItemData
        #endregion
    }

    public interface IReadOnlyKeyActionLauncherToolbarData : IReadOnlyKeyActionPressedData
    {
        #region property
        #endregion
    }

    public class KeyActionLauncherToolbarData : KeyActionPressedData, IReadOnlyKeyActionLauncherToolbarData
    {
        #region IReadOnlyKeyActionLauncherToolbarData
        #endregion
    }


    public interface IReadOnlyKeyMappingData
    {
        #region property

        Key Key { get; }
        ModifierKey Shift { get; }
        ModifierKey Control { get; }
        ModifierKey Alt { get; }
        ModifierKey Super { get; }

        #endregion
    }

    public class KeyMappingData : IReadOnlyKeyMappingData
    {
        #region IReadOnlyKeyMappingData

        public Key Key { get; set; } = Key.None;
        public ModifierKey Shift { get; set; } = ModifierKey.None;
        public ModifierKey Control { get; set; } = ModifierKey.None;
        public ModifierKey Alt { get; set; } = ModifierKey.None;
        public ModifierKey Super { get; set; } = ModifierKey.None;

        #endregion
    }

    public class KeyActionData: DataBase
    {
        #region function

        public Guid KeyActionId { get; set; }
        public KeyActionKind KeyActionKind { get; set; }
        public string KeyActionContent { get; set; } = string.Empty;
        public string KeyActionOption { get; set; } = string.Empty;

        public Key Key { get; set; }
        public ModifierKey Shift { get; set; }
        public ModifierKey Contrl { get; set; }
        public ModifierKey Alt { get; set; }
        public ModifierKey Super { get; set; }

        #endregion
    }

}

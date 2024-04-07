using System.Collections.Generic;

namespace ContentTypeTextNet.Pe.Bridge.Models.Data
{
    /// <summary>
    /// コマンドアイテム種別。
    /// </summary>
    public enum CommandItemKind
    {
        #region property

        #region Pe
        /// <summary>
        /// コマンドはランチャーアイテム。
        /// </summary>
        /// <remarks>
        /// <para>Pe 専用。</para>
        /// </remarks>
        LauncherItem,
        /// <summary>
        /// ランチャーアイテムの名前に一致。
        /// </summary>
        /// <remarks>
        /// <para>Pe 専用。</para>
        /// </remarks>
        LauncherItemName,
        /// <summary>
        /// ランチャーアイテムのコードに一致。
        /// </summary>
        /// <remarks>
        /// <para>Pe 専用。</para>
        /// </remarks>
        LauncherItemCode,
        /// <summary>
        /// ランチャーアイテムのタグに一致。
        /// </summary>
        /// <remarks>
        /// <para>Pe 専用。</para>
        /// </remarks>
        LauncherItemTag,

        /// <summary>
        /// アプリケーション固有処理。
        /// </summary>
        /// <remarks>
        /// <para>Pe 専用。</para>
        /// </remarks>
        ApplicationCommand,

        #endregion

        /// <summary>
        /// プラグイン処理により生成。
        /// </summary>
        Plugin,

        #endregion
    }

    /// <summary>
    /// コマンド実行時の状態パラメータ。
    /// </summary>
    public interface ICommandExecuteParameter
    {
        #region property

        /// <summary>
        /// コマンドランチャーの所在地。
        /// </summary>
        IScreen Screen { get; }
        /// <summary>
        /// 拡張機能(コマンドアイテム依存)を用いるか。
        /// </summary>
        bool IsExtend { get; }

        #endregion
    }

    /// <summary>
    /// コマンド型ランチャーで表示するアイテム。
    /// </summary>
    public interface ICommandItem
    {
        #region property

        /// <summary>
        /// このアイテムを選択可能な入力文字列。
        /// </summary>
        /// <remarks>
        /// <para>この文字列を入力すれば確実にリストアップされることを保証する</para>
        /// </remarks>
        public string FullMatchValue { get; }

        /// <summary>
        /// 小さく表示する種別文言。
        /// </summary>
        CommandItemKind Kind { get; }

        /// <summary>
        /// メイン表示文字列。
        /// </summary>
        IReadOnlyList<HitValue> HeaderValues { get; }
        /// <summary>
        /// 追記文言。
        /// </summary>
        IReadOnlyList<HitValue> DescriptionValues { get; }
        /// <summary>
        /// 拡張機能としての追記文言。
        /// </summary>
        IReadOnlyList<HitValue> ExtendDescriptionValues { get; }

        /// <summary>
        /// 表示優先度。
        /// </summary>
        /// <remarks>
        /// <para><see cref="ScoreKind"/>と<see cref="IHitValuesCreator"/>からもろもろ算出する。</para>
        /// </remarks>
        int Score { get; }

        #endregion

        #region function

        /// <summary>
        /// アイコン取得。
        /// </summary>
        /// <remarks>
        /// <para>UIスレッド上で実行を保証。</para>
        /// </remarks>
        /// <param name="iconScale"></param>
        /// <returns>アイコンとなるデータ。</returns>
        object GetIcon(in IconScale iconScale);

        /// <summary>
        /// コマンドアイテムの実行。
        /// </summary>
        /// <param name="parameter">実行パラメータ。</param>
        void Execute(ICommandExecuteParameter parameter);

        /// <summary>
        /// 対象が自身と同等であるかを調べる。
        /// </summary>
        /// <param name="commandItem"></param>
        /// <returns></returns>
        bool IsEquals(ICommandItem? commandItem);

        #endregion
    }
}

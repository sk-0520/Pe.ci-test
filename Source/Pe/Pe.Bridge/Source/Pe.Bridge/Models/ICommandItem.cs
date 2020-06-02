using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ContentTypeTextNet.Pe.Bridge.Models.Data
{
    public enum CommandItemKind
    {
        #region property

        #region Pe
        /// <summary>
        /// コマンドはランチャーアイテム。
        /// </summary>
        LauncherItem,
        /// <summary>
        /// ランチャーアイテムの名前に一致。
        /// </summary>
        LauncherItemName,
        /// <summary>
        /// ランチャーアイテムのコードに一致。
        /// </summary>
        LauncherItemCode,
        /// <summary>
        /// ランチャーアイテムのタグに一致。
        /// </summary>
        LauncherItemTag,

        /// <summary>
        /// アプリケーション固有処理。
        /// </summary>
        ApplicationCommand,

        #endregion

        /// <summary>
        /// プラグイン処理により生成。
        /// </summary>
        Plugin,

        #endregion
    }

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
        /// <para>この文字列を入力すれば確実にリストアップされることを保証する</para>
        /// </summary>
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


        int Score { get; }

        #endregion

        #region function

        /// <summary>
        /// アイコン取得。
        /// <para>UIスレッド上で実行を保証。</para>
        /// </summary>
        /// <param name="iconBox"></param>
        /// <returns>アイコンとなるデータ。</returns>
        object GetIcon(IconBox iconBox);

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

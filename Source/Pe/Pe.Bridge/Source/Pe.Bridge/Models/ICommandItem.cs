using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ContentTypeTextNet.Pe.Bridge.Models.Data
{
    public static class CommandItemKind
    {
        #region property

        /// <summary>
        /// コマンドはランチャーアイテム。
        /// </summary>
        public static string LauncherItem { get; } = "ITEM";
        /// <summary>
        /// ランチャーアイテムの名前に一致。
        /// </summary>
        public static string LauncherItemName { get; } = "NAME";
        /// <summary>
        /// ランチャーアイテムのコードに一致。
        /// </summary>
        public static string LauncherItemCode { get; } = "CODE";
        /// <summary>
        /// ランチャーアイテムのタグに一致。
        /// </summary>
        public static string LauncherItemTag { get; } = "TAG";
        /// <summary>
        /// プラグイン処理により生成。
        /// </summary>
        public static string Plugin { get; } = "PLUGIN";

        #endregion
    }
    public interface ICommandItem
    {
        #region property

        /// <summary>
        /// メイン表示文字列。
        /// </summary>
        string Header { get; }
        /// <summary>
        /// 追記文言。
        /// </summary>
        string Description { get; }
        /// <summary>
        /// 小さく表示する種別文言。
        /// </summary>
        string Kind { get; }

        IReadOnlyList<Range> HeaderMatches { get; }
        IReadOnlyList<Range> DescriptionMatches { get; }

        double Score { get; }

        #endregion

        #region function

        object GetIcon(IconBox iconBox);

        /// <summary>
        /// コマンドアイテムの実行。
        /// </summary>
        /// <param name="screen">コマンドランチャーの所在地。</param>
        /// <param name="isExtend">拡張機能(コマンドアイテム依存)を用いるか。</param>
        void Execute(IScreen screen, bool isExtend);

        #endregion
    }
}

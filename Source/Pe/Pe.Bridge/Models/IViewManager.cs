using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    /// <summary>
    /// ウィンドウ種別。
    /// </summary>
    public enum ViewKind
    {
        None,
        /// <summary>
        /// なにかのウィンドウハンドル。
        /// </summary>
        Unknown,
        /// <summary>
        /// デスクトップ。
        /// </summary>
        Desktop,
        /// <summary>
        /// Windows Explorer。
        /// </summary>
        Explorer,
        /// <summary>
        /// [Pe] Pe の何か。
        /// </summary>
        Application,
        /// <summary>
        /// [Pe] ランチャーツールバー。
        /// </summary>
        LauncherToolbar,
        /// <summary>
        /// [Pe] ノート。
        /// </summary>
        Note,
        /// <summary>
        /// [Pe] コマンド。
        /// </summary>
        Command,
        /// <summary>
        /// [Pe] 指定して実行。
        /// </summary>
        ExtendsExecute,
        /// <summary>
        /// [Pe] ランチャーアイテム変更。
        /// </summary>
        LauncherItemCustomize,
        /// <summary>
        /// [Pe] 通知。
        /// </summary>
        NotifyLog,
        /// <summary>
        /// [Pe] リリース。
        /// </summary>
        ReleaseNote,
        /// <summary>
        /// [Pe] 標準入出力。
        /// </summary>
        StandardInputOutput,
        /// <summary>
        /// [Pe] プラグイン。
        /// </summary>
        Plugin,
        /// <summary>
        /// [Pe] ウィジェット。
        /// </summary>
        Widget,
    }

    /// <summary>
    /// View 周りの処理。
    /// <para>NOTE: これ未実装。</para>
    /// <para>Pe から提供される。</para>
    /// </summary>
    public interface IViewManager
    {
        #region function

        /// <summary>
        /// 指定したウィンドウハンドルの種別を判定。
        /// </summary>
        /// <param name="hWnd">ウィンドウハンドル。</param>
        /// <returns>ウィンドウ種別。</returns>
        public ViewKind GetViewKind(IntPtr hWnd);

        /// <summary>
        /// Pe に所属するウィンドウ種別か。
        /// <para></para>
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public bool IsApplicationWindow(ViewKind kind);

        #endregion
    }
}

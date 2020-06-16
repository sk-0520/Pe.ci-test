using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Addon
{
    /// <summary>
    /// バックグラウンド処理種別。
    /// </summary>
    public enum BackgroundKind
    {
        /// <summary>
        /// バックグラウンド専用処理。
        /// </summary>
        Running,
        /// <summary>
        /// キーボードフック。
        /// <para>キーボードの押下は取得できるが取り消しは不可。</para>
        /// </summary>
        KeyboardHook,
        /// <summary>
        /// マウスフック。
        /// <para>マウスの押下・移動は取得できるが取り消しは不可。</para>
        /// </summary>
        MouseHook,
    }

    /// <summary>
    /// バックグラウンドで適当に何かする処理。
    /// <para><see cref="IBackground"/>毎の優先度は存在しない。</para>
    /// </summary>
    public interface IBackground
    {
        #region property
        #endregion

        #region function

        /// <summary>
        /// サポートしているバックグラウンド処理。
        /// <para>必要な種別を定義しておかないと必要な Hook* 関数が呼ばれない。</para>
        /// </summary>
        /// <param name="backgroundKind"></param>
        /// <returns></returns>
        bool IsSupported(BackgroundKind backgroundKind);

        /// <summary>
        /// バックグランド処理開始時点で呼び出される。
        /// <para>非同期で呼び出される。</para>
        /// </summary>
        /// <param name="backgroundAddonRunStartupContext"></param>
        void RunStartup(IBackgroundAddonRunStartupContext backgroundAddonRunStartupContext);
        /// <summary>
        /// バックグラウンド処理終了時点で呼び出される。
        /// <para>非同期で呼び出される。</para>
        /// </summary>
        /// <param name="backgroundAddonRunShutdownContext"></param>
        void RunShutdown(IBackgroundAddonRunShutdownContext backgroundAddonRunShutdownContext);


        /// <summary>
        /// キーが押下された。
        /// <para>Pe による無効化・差し替えは無視される。</para>
        /// <para>非同期で呼び出される。</para>
        /// </summary>
        void HookKeyDown(IBackgroundAddonKeyboardContext backgroundAddonKeyboardContext);
        /// <summary>
        /// キーが離された。
        /// <para>Pe による無効化・差し替えは無視される。</para>
        /// <para>非同期で呼び出される。</para>
        /// </summary>
        void HookKeyUp(IBackgroundAddonKeyboardContext backgroundAddonKeyboardContext);


        /// <summary>
        /// マウスが移動した。
        /// <para>Pe による無効化・差し替えは無視される。</para>
        /// <para>非同期で呼び出される。</para>
        /// </summary>
        void HookMouseMove(IBackgroundAddonMouseMoveContext backgroundAddonMouseMoveContext);
        /// <summary>
        /// マウスのボタンが押された。
        /// <para>Pe による無効化・差し替えは無視される。</para>
        /// <para>非同期で呼び出される。</para>
        /// </summary>
        /// <param name="mouseButton"></param>
        /// <param name="mouseButtonState"></param>
        void HookMouseDown(IBackgroundAddonMouseButtonContext backgroundAddonMouseButtonContext);
        /// <summary>
        /// マウスのボタンが離された。
        /// <para>Pe による無効化・差し替えは無視される。</para>
        /// <para>非同期で呼び出される。</para>
        /// </summary>
        /// <param name="mouseButton"></param>
        /// <param name="mouseButtonState"></param>
        void HookMouseUp(IBackgroundAddonMouseButtonContext backgroundAddonMouseButtonContext);

        #endregion
    }
}

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
        /// </summary>
        /// <remarks>
        /// <para>キーボードの押下は取得できるが取り消しは不可。</para>
        /// </remarks>
        KeyboardHook,
        /// <summary>
        /// マウスフック。
        /// </summary>
        /// <remarks>
        /// <para>マウスの押下・移動は取得できるが取り消しは不可。</para>
        /// </remarks>
        MouseHook,
    }

    /// <summary>
    /// なんだこれ。。。
    /// </summary>
    public enum RunExecuteKind
    {

    }

    /// <summary>
    /// バックグラウンドで適当に何かする処理。
    /// </summary>
    /// <remarks>
    /// <para><see cref="IBackground"/>毎の優先度は存在しない。</para>
    /// </remarks>
    public interface IBackground
    {
        #region property
        #endregion

        #region function

        /// <summary>
        /// サポートしているバックグラウンド処理。
        /// </summary>
        /// <remarks>
        /// <para>必要な種別を定義しておかないと必要な Hook* 関数が呼ばれない。</para>
        /// </remarks>
        /// <param name="backgroundKind"></param>
        /// <returns></returns>
        bool IsSupported(BackgroundKind backgroundKind);

        /// <summary>
        /// バックグランド処理開始時点で呼び出される。
        /// </summary>
        /// <remarks>
        /// <para>非同期で呼び出される。</para>
        /// </remarks>
        /// <param name="backgroundAddonRunStartupContext"></param>
        void RunStartup(IBackgroundAddonRunStartupContext backgroundAddonRunStartupContext);
        /// <summary>
        /// バックグランド処理を一時停止・再実行する際に呼び出される
        /// </summary>
        /// <param name="backgroundAddonRunPauseContext"></param>
        void RunPause(IBackgroundAddonRunPauseContext backgroundAddonRunPauseContext);

        /// <summary>
        /// 何かしらが実行された際に呼び出される。
        /// </summary>
        /// <remarks>
        /// <para>非同期で呼び出される。</para>
        /// </remarks>
        /// <param name="backgroundAddonRunExecuteContext"></param>
        void RunExecute(IBackgroundAddonRunExecuteContext backgroundAddonRunExecuteContext);

        /// <summary>
        /// バックグラウンド処理終了時点で呼び出される。
        /// </summary>
        /// <param name="backgroundAddonRunShutdownContext"></param>
        void RunShutdown(IBackgroundAddonRunShutdownContext backgroundAddonRunShutdownContext);


        /// <summary>
        /// キーが押下された。
        /// </summary>
        /// <remarks>
        /// <para>Pe による無効化・差し替えは無視される。</para>
        /// <para>非同期で呼び出される。</para>
        /// </remarks>
        void HookKeyDown(IBackgroundAddonKeyboardContext backgroundAddonKeyboardContext);
        /// <summary>
        /// キーが離された。
        /// </summary>
        /// <remarks>
        /// <para>Pe による無効化・差し替えは無視される。</para>
        /// <para>非同期で呼び出される。</para>
        /// </remarks>
        void HookKeyUp(IBackgroundAddonKeyboardContext backgroundAddonKeyboardContext);


        /// <summary>
        /// マウスが移動した。
        /// </summary>
        /// <remarks>
        /// <para>Pe による無効化・差し替えは無視される。</para>
        /// <para>非同期で呼び出される。</para>
        /// </remarks>
        void HookMouseMove(IBackgroundAddonMouseMoveContext backgroundAddonMouseMoveContext);
        /// <summary>
        /// マウスのボタンが押された。
        /// </summary>
        /// <remarks>
        /// <para>Pe による無効化・差し替えは無視される。</para>
        /// <para>非同期で呼び出される。</para>
        /// </remarks>
        /// <param name="mouseButton"></param>
        /// <param name="mouseButtonState"></param>
        void HookMouseDown(IBackgroundAddonMouseButtonContext backgroundAddonMouseButtonContext);
        /// <summary>
        /// マウスのボタンが離された。
        /// </summary>
        /// <remarks>
        /// <para>Pe による無効化・差し替えは無視される。</para>
        /// <para>非同期で呼び出される。</para>
        /// </remarks>
        /// <param name="mouseButton"></param>
        /// <param name="mouseButtonState"></param>
        void HookMouseUp(IBackgroundAddonMouseButtonContext backgroundAddonMouseButtonContext);

        #endregion
    }
}

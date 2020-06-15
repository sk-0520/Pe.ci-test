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
    public enum BackgroundKinds
    {
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
        /// <summary>
        /// DB アクセス時処理割り込み。
        /// <para>SQL変更・パラメータ変更はできるけど Pe バージョンに依存しまくるし、型の恩恵もないし結果に対してどうこうも出来ない。。</para>
        /// <para>Pe の実行完了後に動作する。</para>
        /// </summary>
        DatabaseAccessHook,
    }

    /// <summary>
    /// バックグラウンドで適当に何かする処理。
    /// <para><see cref="IBackground"/>毎の優先度は存在しない。</para>
    /// </summary>
    public interface IBackground
    {
        #region property

        /// <summary>
        /// サポートしているバックグラウンド処理。
        /// <para>必要な種別を定義しておかないと必要な Hook* 関数が呼ばれない。</para>
        /// </summary>
        public BackgroundKinds SupportedKinds { get; }

        #endregion

        #region function

        /// <summary>
        /// キーが押下された。
        /// <para>非同期で呼び出される。</para>
        /// </summary>
        void HookKeyDown(IBackgroundAddonKeyboardContext backgroundAddonKeyboardContext);
        /// <summary>
        /// キーが離された。
        /// <para>非同期で呼び出される。</para>
        /// </summary>
        void HookKeyUp(IBackgroundAddonKeyboardContext backgroundAddonKeyboardContext);


        /// <summary>
        /// マウスが移動した。
        /// <para>非同期で呼び出される。</para>
        /// </summary>
        void HookMouseMouve(IBackgroundAddonMouseMoveContext backgroundAddonMouseMoveContext);
        /// <summary>
        /// マウスのボタンが押された。
        /// <para>非同期で呼び出される。</para>
        /// </summary>
        /// <param name="mouseButton"></param>
        /// <param name="mouseButtonState"></param>
        void HookMouseDown(IBackgroundAddonMouseButtonContext backgroundAddonMouseButtonContext);
        /// <summary>
        /// マウスのボタンが離された。
        /// <para>非同期で呼び出される。</para>
        /// </summary>
        /// <param name="mouseButton"></param>
        /// <param name="mouseButtonState"></param>
        void HookMouseUp(IBackgroundAddonMouseButtonContext backgroundAddonMouseButtonContext);

        /// <summary>
        /// DBアクセス時にSQLを書き換える。
        /// </summary>
        /// <param name="backgroundAddonDatabaseStatementContext"></param>
        // <returns>書き換え後のSQL文。</returns>
        string HookDatabaseStatement(IBackgroundAddonDatabaseStatementContext backgroundAddonDatabaseStatementContext);

        /// <summary>
        /// DBアクセス時にパラメータを書き換える。
        /// </summary>
        /// <param name="backgroundAddonDatabaseParameterContext"></param>
        /// <returns>書き換え後のパラメータ。入力と同じ(参照)であれば書き換えがなかったとみなされる。</returns>
        object? HookDatabaseParameter(IBackgroundAddonDatabaseParameterContext backgroundAddonDatabaseParameterContext);


        #endregion
    }
}

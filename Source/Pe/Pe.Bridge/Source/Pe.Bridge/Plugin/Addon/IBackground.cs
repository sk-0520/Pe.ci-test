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
        /// <param name="key"></param>
        void HookKeyDown(Key key);
        /// <summary>
        /// キーが離された。
        /// <para>非同期で呼び出される。</para>
        /// </summary>
        /// <param name="key"></param>
        void HookKeyUp(Key key);


        /// <summary>
        /// マウスが移動した。
        /// <para>非同期で呼び出される。</para>
        /// </summary>
        /// <param name="location">マウスカーソルの物理座標。</param>
        void HookMouseMouve([PixelKind(Px.Device)] Point location);
        /// <summary>
        /// マウスのボタンが押された。
        /// <para>非同期で呼び出される。</para>
        /// </summary>
        /// <param name="mouseButton"></param>
        /// <param name="mouseButtonState"></param>
        void HookMouseDown(MouseButton mouseButton, MouseButtonState mouseButtonState);
        /// <summary>
        /// マウスのボタンが離された。
        /// <para>非同期で呼び出される。</para>
        /// </summary>
        /// <param name="mouseButton"></param>
        /// <param name="mouseButtonState"></param>
        void HookMouseUp(MouseButton mouseButton, MouseButtonState mouseButtonState);

        /// <summary>
        /// DBアクセス時にSQLを書き換える。
        /// </summary>
        /// <param name="statement">SQL文。他プラグインによる変更の可能性あり。</param>
        /// <param name="parameter">パラメータ。</param>
        /// <returns>書き換え後のSQL文。</returns>
        string HookDatabaseStatement(string statement, object? parameter);
        /// <summary>
        /// DBアクセス時にパラメータを書き換える。
        /// </summary>
        /// <param name="statement">SQL文。他プラグインによる変更の可能性あり。</param>
        /// <param name="parameter">パラメータ。直接こいつを書き換えるのではなく<paramref name="objectCloner"/>を経由もしくは自力で複製して書き換え処理を行うこと。他プラグインによる変更の可能性あり。</param>
        /// <param name="objectCloner"></param>
        /// <returns>書き換え後のパラメータ。<paramref name="parameter"/>と同じであれば書き換えがなかったとみなされる。</returns>
        object? HookDatabaseParameter(string statement, object? parameter, Func<object, object> objectCloner);


        #endregion
    }
}

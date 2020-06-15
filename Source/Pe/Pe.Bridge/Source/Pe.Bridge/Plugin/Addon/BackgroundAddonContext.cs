using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Addon
{
    public interface IBackgroundAddonKeyboardContext
    {
        #region property

        Key Key { get; }
        bool IsDown { get; }

        DateTime TimestampUtc { get; }

        #endregion
    }

    public interface IBackgroundAddonMouseMoveContext
    {
        #region proeprty

        /// <summary>
        /// マウスカーソルの物理座標。
        /// </summary>
        [PixelKind(Px.Device)]
        Point Location { get; }
        DateTime TimestampUtc { get; }

        #endregion
    }

    public interface IBackgroundAddonMouseButtonContext
    {
        #region proeprty

        MouseButton Button { get; }
        MouseButtonState State { get; }
        DateTime TimestampUtc { get; }

        #endregion
    }

    public interface IBackgroundAddonDatabaseStatementContext
    {
        #region proeprty

        /// <summary>
        /// SQL文。他プラグインによる変更の可能性あり。
        /// </summary>
        string Statement { get; }
        /// <summary>
        /// パラメータ。
        /// </summary>
        object? Parameter { get; }
        DateTime TimestampUtc { get; }

        #endregion
    }

    public interface IBackgroundAddonDatabaseParameterContext
    {
        #region proeprty

        /// <summary>
        /// SQL文。他プラグインによる変更の可能性あり。
        /// </summary>
        string Statement { get; }
        /// <summary>
        /// パラメータ。
        /// <para>直接こいつを書き換えるのではなく<see cref="ObjectCloner"/>を経由もしくは自力で複製して書き換え処理を行うこと。他プラグインによる変更の可能性あり。</para>
        /// </summary>
        object? Parameter { get; }
        /// <summary>
        /// パラメータ複製装置。
        /// </summary>
        Func<object, object> ObjectCloner { get; }
        DateTime TimestampUtc { get; }

        #endregion
    }
}

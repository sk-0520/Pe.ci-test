using System;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    internal enum WebViewWidgetResizeDirection
    {
        /// <summary>
        /// 上。
        /// </summary>
        North,
        /// <summary>
        /// 下。
        /// </summary>
        South,
        /// <summary>
        /// 右。
        /// </summary>
        East,
        /// <summary>
        /// 左。
        /// </summary>
        West,
        /// <summary>
        /// 右上。
        /// </summary>
        NorthEast,
        /// <summary>
        /// 左上。
        /// </summary>
        NorthWest,
        /// <summary>
        /// 左下。
        /// </summary>
        SouthWest,
        /// <summary>
        /// 右下。
        /// </summary>
        SouthEast,
    }

    internal class WebViewWidgetResizeEventArgs: EventArgs
    {
        public WebViewWidgetResizeEventArgs(WebViewWidgetResizeDirection direction)
        {
            Direction = direction;
        }

        #region property

        public WebViewWidgetResizeDirection Direction { get; }

        #endregion
    }

    internal sealed class WebViewWidgetCallbacks
    {
        #region event

        public event EventHandler? MoveStarted;
        public event EventHandler<WebViewWidgetResizeEventArgs>? ResizeStarted;
        public event EventHandler? Injected;

        #endregion
        public WebViewWidgetCallbacks(IPluginIdentifiers pluginIdentifiers, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            PluginIdentifiers = pluginIdentifiers;
        }

        #region property
        IPluginIdentifiers PluginIdentifiers { get; }
        ILogger Logger { get; }

        #endregion

        #region function

        void OnMoveStarted()
        {
            MoveStarted?.Invoke(this, EventArgs.Empty);
        }

        public void MoveStart()
        {
            OnMoveStarted();
        }

        void OnResizeStart(string direction)
        {
            var resizeStarted = ResizeStarted;
            if(resizeStarted != null) {
                var resizeDirection = direction switch {
                    "n" => WebViewWidgetResizeDirection.North,
                    "s" => WebViewWidgetResizeDirection.South,
                    "e" => WebViewWidgetResizeDirection.East,
                    "w" => WebViewWidgetResizeDirection.West,
                    "ne" => WebViewWidgetResizeDirection.NorthEast,
                    "nw" => WebViewWidgetResizeDirection.NorthWest,
                    "sw" => WebViewWidgetResizeDirection.SouthWest,
                    _ => WebViewWidgetResizeDirection.SouthEast,
                };

                resizeStarted(this, new WebViewWidgetResizeEventArgs(resizeDirection));
            }
        }

        public void ResizeStart(string direction)
        {
            Logger.LogDebug("{0}", direction);
            OnResizeStart(direction);
        }

        void OnInjected()
        {
            Injected?.Invoke(this, EventArgs.Empty);
        }

        public void CompleteInjection()
        {
            OnInjected();
        }

        #endregion
    }
}

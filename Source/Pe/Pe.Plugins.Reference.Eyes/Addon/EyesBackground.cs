using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Eyes.Addon
{
    public class BackgroundMouseMoveEventArgs: EventArgs
    {
        public BackgroundMouseMoveEventArgs(Point location, DateTime timestamp)
        {
            Location = location;
            Timestamp = timestamp;
        }

        #region property

        public Point Location { get; }
        public DateTime Timestamp { get; }

        #endregion
    }

    public class BackgroundMouseButtonEventArgs: EventArgs
    {
        public BackgroundMouseButtonEventArgs(MouseButton button, MouseButtonState state,DateTime timestamp)
        {
            Button = button;
            State = state;
            Timestamp = timestamp;
        }

        #region property

        public MouseButton Button { get; }
        public MouseButtonState State { get; }
        public DateTime Timestamp { get; }

        #endregion
    }

    public class BackgroundKeyEventArgs: EventArgs
    {
        public BackgroundKeyEventArgs(Key key, DateTime timestamp)
        {
            Key = key;
            Timestamp = timestamp;
        }

        #region property

        public Key Key { get; }
        public DateTime Timestamp { get; }

        #endregion
    }

    internal class EyesBackground: IBackground
    {
        #region event

        public event EventHandler<BackgroundMouseMoveEventArgs>? MouseMoved;
        public event EventHandler<BackgroundMouseButtonEventArgs>? MouseDown;
        public event EventHandler<BackgroundMouseButtonEventArgs>? MouseUp;
        public event EventHandler<BackgroundKeyEventArgs>? KeyDown;
        public event EventHandler<BackgroundKeyEventArgs>? KeyUp;

        #endregion

        public EyesBackground(IAddonParameter parameter, IPluginInformation pluginInformation)
        {
            LoggerFactory = parameter.LoggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            AddonExecutor = parameter.AddonExecutor;
            DispatcherWrapper = parameter.DispatcherWrapper;
            SkeletonImplements = parameter.SkeletonImplements;
            PluginInformation = pluginInformation;
        }

        #region proeprty

        private ILoggerFactory LoggerFactory { get; }
        private ILogger Logger { get; }
        private IAddonExecutor AddonExecutor { get; }
        private IDispatcherWrapper DispatcherWrapper { get; }
        private ISkeletonImplements SkeletonImplements { get; }
        private IPluginInformation PluginInformation { get; }

        #endregion

        #region IBackground

        public bool IsSupported(BackgroundKind backgroundKind)
        {
            return backgroundKind switch
            {
                BackgroundKind.Running => false,
                BackgroundKind.KeyboardHook => true,
                BackgroundKind.MouseHook => true,
                _ => throw new NotImplementedException(),
            };
        }

        public void RunStartup(IBackgroundAddonRunStartupContext backgroundAddonRunStartupContext)
        {
            throw new NotSupportedException();
        }

        public void RunPause(IBackgroundAddonRunPauseContext backgroundAddonRunPauseContext)
        {
            throw new NotSupportedException();
        }

        public void RunExecute(IBackgroundAddonRunExecuteContext backgroundAddonRunExecuteContext)
        {
            throw new NotSupportedException();
        }

        public void RunShutdown(IBackgroundAddonRunShutdownContext backgroundAddonRunShutdownContext)
        {
            throw new NotSupportedException();
        }



        public void HookKeyDown(IBackgroundAddonKeyboardContext backgroundAddonKeyboardContext)
        {
            KeyDown?.Invoke(this, new BackgroundKeyEventArgs(backgroundAddonKeyboardContext.Key, backgroundAddonKeyboardContext.Timestamp));
        }

        public void HookKeyUp(IBackgroundAddonKeyboardContext backgroundAddonKeyboardContext)
        {
            KeyUp?.Invoke(this, new BackgroundKeyEventArgs(backgroundAddonKeyboardContext.Key, backgroundAddonKeyboardContext.Timestamp));
        }


        public void HookMouseMove(IBackgroundAddonMouseMoveContext backgroundAddonMouseMoveContext)
        {
            MouseMoved?.Invoke(this, new BackgroundMouseMoveEventArgs(backgroundAddonMouseMoveContext.Location, backgroundAddonMouseMoveContext.Timestamp));
        }

        public void HookMouseDown(IBackgroundAddonMouseButtonContext backgroundAddonMouseButtonContext)
        {
            MouseDown?.Invoke(this, new BackgroundMouseButtonEventArgs(backgroundAddonMouseButtonContext.Button, backgroundAddonMouseButtonContext.State, backgroundAddonMouseButtonContext.Timestamp));
        }

        public void HookMouseUp(IBackgroundAddonMouseButtonContext backgroundAddonMouseButtonContext)
        {
            MouseUp?.Invoke(this, new BackgroundMouseButtonEventArgs(backgroundAddonMouseButtonContext.Button, backgroundAddonMouseButtonContext.State, backgroundAddonMouseButtonContext.Timestamp));
        }


        #endregion
    }
}

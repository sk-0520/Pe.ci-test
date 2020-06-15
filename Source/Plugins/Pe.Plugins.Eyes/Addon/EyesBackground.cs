using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Eyes.Addon
{
    internal class EyesBackground: IBackground
    {
        public EyesBackground(IAddonParameter parameter, IPluginInformations pluginInformations)
        {
            LoggerFactory = parameter.LoggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            AddonExecutor = parameter.AddonExecutor;
            DispatcherWrapper = parameter.DispatcherWrapper;
            SkeletonImplements = parameter.SkeletonImplements;
            PluginInformations = pluginInformations;
        }

        #region proeprty

        ILoggerFactory LoggerFactory { get; }
        ILogger Logger { get; }
        IAddonExecutor AddonExecutor { get; }
        IDispatcherWrapper DispatcherWrapper { get; }
        ISkeletonImplements SkeletonImplements { get; }
        IPluginInformations PluginInformations { get; }

        #endregion

        #region IBackground

        public bool IsSupported(BackgroundKind backgroundKind)
        {
            return backgroundKind switch
            {
                BackgroundKind.KeyboardHook => true,
                BackgroundKind.MouseHook => true,
                BackgroundKind.DatabaseAccessHook => false,
                _ => throw new NotImplementedException(),
            };
        }

        public void HookKeyDown(IBackgroundAddonKeyboardContext backgroundAddonKeyboardContext)
        {
            Logger.LogInformation("down {0}", backgroundAddonKeyboardContext.Key);
        }

        public void HookKeyUp(IBackgroundAddonKeyboardContext backgroundAddonKeyboardContext)
        {
            Logger.LogInformation("up {0}", backgroundAddonKeyboardContext.Key);
        }


        public void HookMouseMove(IBackgroundAddonMouseMoveContext backgroundAddonMouseMoveContext)
        {
            //throw new NotImplementedException();
            Logger.LogInformation("move {0}", backgroundAddonMouseMoveContext.Location);
        }

        public void HookMouseDown(IBackgroundAddonMouseButtonContext backgroundAddonMouseButtonContext)
        {
            //throw new NotImplementedException();
        }

        public void HookMouseUp(IBackgroundAddonMouseButtonContext backgroundAddonMouseButtonContext)
        {
            //throw new NotImplementedException();
        }


        public string HookDatabaseStatement(IBackgroundAddonDatabaseStatementContext backgroundAddonDatabaseStatementContext)
        {
            throw new NotImplementedException();
        }

        public object? HookDatabaseParameter(IBackgroundAddonDatabaseParameterContext backgroundAddonDatabaseParameterContext)
        {
            throw new NotImplementedException();
        }




        #endregion
    }
}

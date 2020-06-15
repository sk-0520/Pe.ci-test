using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    internal class BackgroundAddonProxy: AddonsProxyBase<IBackground>
    {
        public BackgroundAddonProxy(IReadOnlyList<IAddon> addons, PluginContextFactory pluginContextFactory, IUserAgentFactory userAgentFactory, IPlatformTheme platformTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(addons, pluginContextFactory, userAgentFactory, platformTheme, dispatcherWrapper, loggerFactory)
        {
        }

        #region function

        /// <inheritdoc cref="IBackground.IsSupported(BackgroundKind)"/>
        public bool IsSupported(BackgroundKind backgroundKind)
        {
            return FunctionUnits.Any(i => i.IsSupported(backgroundKind));
        }

        /// <inheritdoc cref="IBackground.HookKeyDown(IBackgroundAddonKeyboardContext)"/>
        public void HookKeyDown(IBackgroundAddonKeyboardContext backgroundAddonKeyboardContext)
        {
            Debug.Assert(backgroundAddonKeyboardContext.GetType() == typeof(BackgroundAddonProxyKeyboardContext));

            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IBackground.HookKeyUp(IBackgroundAddonKeyboardContext)"/>
        public void HookKeyUp(IBackgroundAddonKeyboardContext backgroundAddonKeyboardContext)
        {
            Debug.Assert(backgroundAddonKeyboardContext.GetType() == typeof(BackgroundAddonProxyKeyboardContext));

            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IBackground.HookMouseMove(IBackgroundAddonMouseMoveContext)"/>
        public void HookMouseMove(IBackgroundAddonMouseMoveContext backgroundAddonMouseMoveContext)
        {
            Debug.Assert(backgroundAddonMouseMoveContext.GetType() == typeof(BackgroundAddonProxyMouseMoveContext));
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IBackground.HookMouseDown(IBackgroundAddonMouseButtonContext)"/>
        public void HookMouseDown(IBackgroundAddonMouseButtonContext backgroundAddonMouseButtonContext)
        {
            Debug.Assert(backgroundAddonMouseButtonContext == null);
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IBackground.HookMouseUp(IBackgroundAddonMouseButtonContext)"/>
        public void HookMouseUp(IBackgroundAddonMouseButtonContext backgroundAddonMouseButtonContext)
        {
            Debug.Assert(backgroundAddonMouseButtonContext == null);
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IBackground"/>
        public string HookDatabaseStatement(IBackgroundAddonDatabaseStatementContext backgroundAddonDatabaseStatementContext)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IBackground"/>
        public object? HookDatabaseParameter(IBackgroundAddonDatabaseParameterContext backgroundAddonDatabaseParameterContext)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region AddonWrapperBase

        protected override AddonKind AddonKind => AddonKind.Background;

        protected override IBackground BuildFunctionUnit(IAddon loadedAddon)
        {
            return loadedAddon.BuildBackground(CreateParameter(loadedAddon));
        }


        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    internal class BackgroundAddonProxy: AddonsProxyBase<IBackground>, IBackground
    {
        public BackgroundAddonProxy(IReadOnlyList<IAddon> addons, PluginContextFactory pluginContextFactory, BackgroundAddonContextFactory backgroundAddonContextFactory, IUserAgentFactory userAgentFactory, IPlatformTheme platformTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(addons, pluginContextFactory, userAgentFactory, platformTheme, dispatcherWrapper, loggerFactory)
        {
            BackgroundAddonContextFactory = backgroundAddonContextFactory;
        }

        #region property

        BackgroundAddonContextFactory BackgroundAddonContextFactory { get; }

        IDictionary<BackgroundKind, bool> SupportedCache { get; } = new Dictionary<BackgroundKind, bool>();

        #endregion

        #region function

        /// <inheritdoc cref="IBackground.IsSupported(BackgroundKind)"/>
        public bool IsSupported(BackgroundKind backgroundKind)
        {
            if(SupportedCache.TryGetValue(backgroundKind, out var value)) {
                return value;
            }
            var result = FunctionUnits.Any(i => i.IsSupported(backgroundKind));
            return SupportedCache[backgroundKind] = result;
        }

        /// <inheritdoc cref="IBackground.HookKeyDown(IBackgroundAddonKeyboardContext)"/>
        public void HookKeyDown(BackgroundAddonProxyKeyboardContext backgroundAddonKeyboardContext)
        {
            Task.Run(() => {
                var functionUnits = FunctionUnits.Where(i => i.IsSupported(BackgroundKind.KeyboardHook));
                foreach(var functionUnit in functionUnits) {
                    var addon = GetAddon(functionUnit);
                    var context = BackgroundAddonContextFactory.CreateKeyboardContext(addon.PluginInformations, backgroundAddonKeyboardContext.KeyboardHookEventArgs);
                    functionUnit.HookKeyDown(context);
                }
            });
        }
        void IBackground.HookKeyDown(IBackgroundAddonKeyboardContext backgroundAddonKeyboardContext)
        {
            Debug.Assert(backgroundAddonKeyboardContext.GetType() == typeof(BackgroundAddonProxyKeyboardContext));
            HookKeyDown((BackgroundAddonProxyKeyboardContext)backgroundAddonKeyboardContext);
        }

        /// <inheritdoc cref="IBackground.HookKeyUp(IBackgroundAddonKeyboardContext)"/>
        public void HookKeyUp(BackgroundAddonProxyKeyboardContext backgroundAddonKeyboardContext)
        {
            Debug.Assert(backgroundAddonKeyboardContext.GetType() == typeof(BackgroundAddonProxyKeyboardContext));
            Task.Run(() => {
                var functionUnits = FunctionUnits.Where(i => i.IsSupported(BackgroundKind.KeyboardHook));
                foreach(var functionUnit in functionUnits) {
                    var addon = GetAddon(functionUnit);
                    var context = BackgroundAddonContextFactory.CreateKeyboardContext(addon.PluginInformations, backgroundAddonKeyboardContext.KeyboardHookEventArgs);
                    functionUnit.HookKeyUp(context);
                }
            });
        }
        void IBackground.HookKeyUp(IBackgroundAddonKeyboardContext backgroundAddonKeyboardContext)
        {
            Debug.Assert(backgroundAddonKeyboardContext.GetType() == typeof(BackgroundAddonProxyKeyboardContext));
            HookKeyUp((BackgroundAddonProxyKeyboardContext)backgroundAddonKeyboardContext);
        }

        /// <inheritdoc cref="IBackground.HookMouseMove(IBackgroundAddonMouseMoveContext)"/>
        public void HookMouseMove(BackgroundAddonProxyMouseMoveContext backgroundAddonMouseMoveContext)
        {
            Debug.Assert(backgroundAddonMouseMoveContext.GetType() == typeof(BackgroundAddonProxyMouseMoveContext));
            Task.Run(() => {
                var functionUnits = FunctionUnits.Where(i => i.IsSupported(BackgroundKind.MouseHook));
                foreach(var functionUnit in functionUnits) {
                    var addon = GetAddon(functionUnit);
                    var context = BackgroundAddonContextFactory.CreateMouseMoveContex(addon.PluginInformations, backgroundAddonMouseMoveContext.MouseHookEventArgs);
                    functionUnit.HookMouseMove(context);
                }
            });
        }
        void IBackground.HookMouseMove(IBackgroundAddonMouseMoveContext backgroundAddonMouseMoveContext)
        {
            Debug.Assert(backgroundAddonMouseMoveContext.GetType() == typeof(BackgroundAddonProxyMouseMoveContext));
            HookMouseMove((BackgroundAddonProxyMouseMoveContext)backgroundAddonMouseMoveContext);
        }

        /// <inheritdoc cref="IBackground.HookMouseDown(IBackgroundAddonMouseButtonContext)"/>
        public void HookMouseDown(BackgroundAddonProxyMouseButtonContext backgroundAddonMouseButtonContext)
        {
            Task.Run(() => {
                var functionUnits = FunctionUnits.Where(i => i.IsSupported(BackgroundKind.MouseHook));
                foreach(var functionUnit in functionUnits) {
                    var addon = GetAddon(functionUnit);
                    var context = BackgroundAddonContextFactory.CreateMouseButtonContex(addon.PluginInformations, backgroundAddonMouseButtonContext.MouseHookEventArgs);
                    functionUnit.HookMouseDown(context);
                }
            });
        }
        void IBackground.HookMouseDown(IBackgroundAddonMouseButtonContext backgroundAddonMouseButtonContext)
        {
            Debug.Assert(backgroundAddonMouseButtonContext.GetType() == typeof(BackgroundAddonProxyMouseButtonContext));
            HookMouseDown((BackgroundAddonProxyMouseButtonContext)backgroundAddonMouseButtonContext);
        }

        /// <inheritdoc cref="IBackground.HookMouseUp(IBackgroundAddonMouseButtonContext)"/>
        public void HookMouseUp(BackgroundAddonProxyMouseButtonContext backgroundAddonMouseButtonContext)
        {
            Task.Run(() => {
                var functionUnits = FunctionUnits.Where(i => i.IsSupported(BackgroundKind.MouseHook));
                foreach(var functionUnit in functionUnits) {
                    var addon = GetAddon(functionUnit);
                    var context = BackgroundAddonContextFactory.CreateMouseButtonContex(addon.PluginInformations, backgroundAddonMouseButtonContext.MouseHookEventArgs);
                    functionUnit.HookMouseUp(context);
                }
            });
        }
        void IBackground.HookMouseUp(IBackgroundAddonMouseButtonContext backgroundAddonMouseButtonContext)
        {
            Debug.Assert(backgroundAddonMouseButtonContext.GetType() == typeof(BackgroundAddonProxyMouseButtonContext));
            HookMouseUp((BackgroundAddonProxyMouseButtonContext)backgroundAddonMouseButtonContext);
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

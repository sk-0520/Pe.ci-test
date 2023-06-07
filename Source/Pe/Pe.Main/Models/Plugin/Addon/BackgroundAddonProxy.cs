using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    internal class BackgroundAddonProxy: AddonsProxyBase<IBackground>, IBackground
    {
        public BackgroundAddonProxy(IReadOnlyList<IAddon> addons, PluginContextFactory pluginContextFactory, BackgroundAddonContextFactory backgroundAddonContextFactory, IHttpUserAgentFactory userAgentFactory, IViewManager viewManager, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(addons, pluginContextFactory, userAgentFactory, viewManager, platformTheme, imageLoader, mediaConverter, policy, dispatcherWrapper, loggerFactory)
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

        /// <inheritdoc cref="IBackground.RunStartup(IBackgroundAddonRunStartupContext)"/>
        public void RunStartup(BackgroundAddonProxyRunStartupContext backgroundAddonRunStartupContext)
        {
            var functionUnits = FunctionUnits.Where(i => i.IsSupported(BackgroundKind.Running));
            foreach(var functionUnit in functionUnits) {
                var addon = GetAddon(functionUnit);
                var context = BackgroundAddonContextFactory.CreateRunStartupContext(addon.PluginInformation);
                functionUnit.RunStartup(context);
            }
        }

        void IBackground.RunStartup(IBackgroundAddonRunStartupContext backgroundAddonRunStartupContext)
        {
            Debug.Assert(backgroundAddonRunStartupContext.GetType() == typeof(BackgroundAddonProxyRunStartupContext));
            RunStartup((BackgroundAddonProxyRunStartupContext)backgroundAddonRunStartupContext);
        }

        /// <inheritdoc cref="IBackground.RunPause(IBackgroundAddonRunPauseContext)"/>
        public void RunPause(BackgroundAddonProxyRunPauseContext backgroundAddonRunPauseContext)
        {
            var functionUnits = FunctionUnits.Where(i => i.IsSupported(BackgroundKind.Running));
            foreach(var functionUnit in functionUnits) {
                var addon = GetAddon(functionUnit);
                var context = BackgroundAddonContextFactory.CreateRunPauseContext(addon.PluginInformation, backgroundAddonRunPauseContext.IsPausing);
                functionUnit.RunPause(context);
            }
        }

        void IBackground.RunPause(IBackgroundAddonRunPauseContext backgroundAddonRunPauseContext)
        {
            Debug.Assert(backgroundAddonRunPauseContext.GetType() == typeof(BackgroundAddonProxyRunPauseContext));
            RunPause((BackgroundAddonProxyRunPauseContext)backgroundAddonRunPauseContext);
        }

        /// <inheritdoc cref="IBackground.RunExecute(IBackgroundAddonRunExecuteContext)"/>
        public void RunExecute(BackgroundAddonProxyRunExecuteContext backgroundAddonRunExecuteContext)
        {
            Task.Run(() => {
                var functionUnits = FunctionUnits.Where(i => i.IsSupported(BackgroundKind.Running));
                foreach(var functionUnit in functionUnits) {
                    var addon = GetAddon(functionUnit);
                    var context = BackgroundAddonContextFactory.CreateRunExecuteContext(addon.PluginInformation, backgroundAddonRunExecuteContext.RunExecuteKind, backgroundAddonRunExecuteContext.Parameter, backgroundAddonRunExecuteContext.Timestamp);
                    functionUnit.RunExecute(context);
                }
            });
        }
        void IBackground.RunExecute(IBackgroundAddonRunExecuteContext backgroundAddonRunExecuteContext)
        {
            Debug.Assert(backgroundAddonRunExecuteContext.GetType() == typeof(BackgroundAddonProxyRunExecuteContext));
            RunExecute((BackgroundAddonProxyRunExecuteContext)backgroundAddonRunExecuteContext);
        }


        /// <inheritdoc cref="IBackground.RunShutdown(IBackgroundAddonRunShutdownContext)"/>
        public void RunShutdown(BackgroundAddonProxyRunShutdownContext backgroundAddonRunShutdownContext)
        {
            var functionUnits = FunctionUnits.Where(i => i.IsSupported(BackgroundKind.Running));
            foreach(var functionUnit in functionUnits) {
                var addon = GetAddon(functionUnit);
                var context = BackgroundAddonContextFactory.CreateRunShutdownContext(addon.PluginInformation);
                functionUnit.RunShutdown(context);
            }
        }
        void IBackground.RunShutdown(IBackgroundAddonRunShutdownContext backgroundAddonRunShutdownContext)
        {
            Debug.Assert(backgroundAddonRunShutdownContext.GetType() == typeof(BackgroundAddonProxyRunShutdownContext));
            RunShutdown((BackgroundAddonProxyRunShutdownContext)backgroundAddonRunShutdownContext);
        }

        /// <inheritdoc cref="IBackground.HookKeyDown(IBackgroundAddonKeyboardContext)"/>
        public void HookKeyDown(BackgroundAddonProxyKeyboardContext backgroundAddonKeyboardContext)
        {
            Task.Run(() => {
                var functionUnits = FunctionUnits.Where(i => i.IsSupported(BackgroundKind.KeyboardHook));
                foreach(var functionUnit in functionUnits) {
                    var addon = GetAddon(functionUnit);
                    var context = BackgroundAddonContextFactory.CreateKeyboardContext(addon.PluginInformation, backgroundAddonKeyboardContext.KeyboardHookEventArgs);
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
                    var context = BackgroundAddonContextFactory.CreateKeyboardContext(addon.PluginInformation, backgroundAddonKeyboardContext.KeyboardHookEventArgs);
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
                    var context = BackgroundAddonContextFactory.CreateMouseMoveContext(addon.PluginInformation, backgroundAddonMouseMoveContext.MouseHookEventArgs);
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
                    var context = BackgroundAddonContextFactory.CreateMouseButtonContext(addon.PluginInformation, backgroundAddonMouseButtonContext.MouseHookEventArgs);
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
                    var context = BackgroundAddonContextFactory.CreateMouseButtonContext(addon.PluginInformation, backgroundAddonMouseButtonContext.MouseHookEventArgs);
                    functionUnit.HookMouseUp(context);
                }
            });
        }
        void IBackground.HookMouseUp(IBackgroundAddonMouseButtonContext backgroundAddonMouseButtonContext)
        {
            Debug.Assert(backgroundAddonMouseButtonContext.GetType() == typeof(BackgroundAddonProxyMouseButtonContext));
            HookMouseUp((BackgroundAddonProxyMouseButtonContext)backgroundAddonMouseButtonContext);
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

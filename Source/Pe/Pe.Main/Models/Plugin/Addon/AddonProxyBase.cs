using System;
using System.Collections.Generic;
using System.Diagnostics;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    public abstract class CommonAddonProxyBase<TFunctionUnit>: DisposerBase
        where TFunctionUnit : notnull
    {
        protected CommonAddonProxyBase(PluginContextFactory pluginContextFactory, IHttpUserAgentFactory userAgentFactory, IViewManager viewManager, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());

            PluginContextFactory = pluginContextFactory;

            UserAgentFactory = userAgentFactory;

            ViewManager = viewManager;
            PlatformTheme = platformTheme;
            ImageLoader = imageLoader;
            MediaConverter = mediaConverter;
            Policy = policy;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        /// <inheritdoc cref="ILoggerFactory"/>
        protected ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        protected ILogger Logger { get; }
        /// <inheritdoc cref="PluginContextFactory"/>
        protected PluginContextFactory PluginContextFactory { get; }
        /// <inheritdoc cref="IHttpUserAgentFactory"/>
        protected IHttpUserAgentFactory UserAgentFactory { get; }

        protected IViewManager ViewManager { get; }
        /// <inheritdoc cref="IPlatformTheme"/>
        protected IPlatformTheme PlatformTheme { get; }
        /// <inheritdoc cref="IImageLoader"/>
        protected IImageLoader ImageLoader { get; }
        /// <inheritdoc cref="IMediaConverter"/>
        protected IMediaConverter MediaConverter { get; }
        /// <inheritdoc cref="inheritdoc"/>
        protected IPolicy Policy { get; }
        /// <inheritdoc cref="IDispatcherWrapper"/>
        protected IDispatcherWrapper DispatcherWrapper { get; }

        /// <summary>
        /// 担当する処理単位。
        /// </summary>
        protected abstract AddonKind AddonKind { get; }

        #endregion

        #region function

        /// <summary>
        /// <see cref="AddonParameter"/> を普通に作成する。
        /// </summary>
        /// <returns></returns>
        protected virtual AddonParameter CreateParameter(IPlugin plugin) => new AddonParameter(new SkeletonImplements(), plugin.PluginInformation, UserAgentFactory, ViewManager, PlatformTheme, ImageLoader, MediaConverter, Policy, DispatcherWrapper, LoggerFactory);

        protected abstract TFunctionUnit BuildFunctionUnit(IAddon loadedAddon);

        #endregion

    }

    /// <summary>
    /// 特定に機能単位による単独アドオン機能のラッパー。
    /// </summary>
    /// <typeparam name="TFunctionUnit"></typeparam>
    public abstract class AddonProxyBase<TFunctionUnit>: CommonAddonProxyBase<TFunctionUnit>
        where TFunctionUnit : class
    {
        #region variable

        TFunctionUnit? _functionUnit;

        #endregion

        protected AddonProxyBase(IAddon addon, PluginContextFactory pluginContextFactory, IHttpUserAgentFactory userAgentFactory, IViewManager viewManager, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(pluginContextFactory, userAgentFactory, viewManager, platformTheme, imageLoader, mediaConverter, policy, dispatcherWrapper, loggerFactory)
        {
            Addon = addon;
        }

        #region property

        /// <summary>
        /// 対象アドオン。
        /// </summary>
        public IAddon Addon { get; }

        protected TFunctionUnit FunctionUnit
        {
            get
            {
                if(this._functionUnit == null) {
                    Debug.Assert(Addon.IsSupported(AddonKind));

                    if(!Addon.IsLoaded(Bridge.Plugin.PluginKind.Addon)) {
                        using(var reader = PluginContextFactory.BarrierRead()) {
                            using var loadContext = PluginContextFactory.CreateLoadContext(Addon.PluginInformation, reader);
                            Addon.Load(Bridge.Plugin.PluginKind.Addon, loadContext);
                        }
                    }

                    this._functionUnit = BuildFunctionUnit(Addon);
                }

                return this._functionUnit;
            }
        }

        #endregion

        #region function
        #endregion
    }

    /// <summary>
    /// 特定に機能単位による複数アドオン機能のラッパー。
    /// </summary>
    /// <typeparam name="TFunctionUnit"></typeparam>
    public abstract class AddonsProxyBase<TFunctionUnit>: CommonAddonProxyBase<TFunctionUnit>
        where TFunctionUnit : notnull
    {
        #region variable

        IReadOnlyList<TFunctionUnit>? _functionUnits;
        IReadOnlyDictionary<TFunctionUnit, IAddon>? _functionAddonMap;

        #endregion

        protected AddonsProxyBase(IReadOnlyList<IAddon> addons, PluginContextFactory pluginContextFactory, IHttpUserAgentFactory userAgentFactory, IViewManager viewManager, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(pluginContextFactory, userAgentFactory, viewManager, platformTheme, imageLoader, mediaConverter, policy, dispatcherWrapper, loggerFactory)
        {
            Addons = addons;
        }

        #region property


        /// <summary>
        /// 対象アドオン一覧。
        /// </summary>
        protected IReadOnlyList<IAddon> Addons { get; }

        /// <summary>
        /// 処理単位一覧。
        /// </summary>
        /// <remarks>
        /// <para>初回参照時に読み込まれる。</para>
        /// </remarks>
        protected IReadOnlyList<TFunctionUnit> FunctionUnits
        {
            get
            {
                if(this._functionUnits == null) {
                    var result = LoadFunctionUnits();
                    this._functionUnits = result.units;
                    this._functionAddonMap = result.map;
                }

                return this._functionUnits;
            }
        }

        #endregion

        #region function

        protected (IReadOnlyList<TFunctionUnit> units, IReadOnlyDictionary<TFunctionUnit, IAddon> map) LoadFunctionUnits()
        {
            var map = new Dictionary<TFunctionUnit, IAddon>();
            var list = new List<TFunctionUnit>(Addons.Count);
            foreach(var addon in Addons) {
                Debug.Assert(addon.IsSupported(AddonKind));

                if(!addon.IsLoaded(Bridge.Plugin.PluginKind.Addon)) {
                    using(var reader = PluginContextFactory.BarrierRead()) {
                        using var loadContext = PluginContextFactory.CreateLoadContext(addon.PluginInformation, reader);
                        addon.Load(Bridge.Plugin.PluginKind.Addon, loadContext);
                    }
                }
                var functionUnit = BuildFunctionUnit(addon);
                list.Add(functionUnit);
                map.Add(functionUnit, addon);
            }
            return (list, map);
        }

        public IAddon GetAddon(TFunctionUnit functionUnit)
        {
            if(this._functionAddonMap == null) {
                throw new InvalidOperationException(nameof(FunctionUnits));
            }

            return this._functionAddonMap[functionUnit];
        }

        #endregion
    }
}

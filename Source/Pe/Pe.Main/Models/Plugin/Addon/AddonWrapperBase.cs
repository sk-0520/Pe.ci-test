using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    /// <summary>
    /// 特定に機能単位によるアドオン機能のラッパー。
    /// </summary>
    /// <typeparam name="TFunctionUnit"></typeparam>
    public abstract class AddonWrapperBase<TFunctionUnit>: DisposerBase
    {
        #region variable

        IReadOnlyList<TFunctionUnit>? _functionUnits;

        #endregion

        protected AddonWrapperBase(IReadOnlyList<IAddon> addons, IDatabaseBarrierPack databaseBarrierPack, IDatabaseLazyWriterPack databaseLazyWriterPack, IDatabaseStatementLoader databaseStatementLoader, EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager, IPlatformTheme platformTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            DatabaseBarrierPack = databaseBarrierPack;
            DatabaseLazyWriterPack =databaseLazyWriterPack;
            DatabaseStatementLoader = databaseStatementLoader;
            EnvironmentParameters = environmentParameters;
            UserAgentManager = userAgentManager;
            PlatformTheme = platformTheme;
            DispatcherWrapper = dispatcherWrapper;
            Addons = addons;
        }

        #region property

        protected ILoggerFactory LoggerFactory { get; }
        protected ILogger Logger { get; }
       protected IDatabaseBarrierPack DatabaseBarrierPack { get; }
        protected IDatabaseLazyWriterPack DatabaseLazyWriterPack { get; }
        protected IDatabaseStatementLoader DatabaseStatementLoader { get; }
        protected EnvironmentParameters EnvironmentParameters { get; }
        protected IUserAgentManager UserAgentManager { get; }
        protected IPlatformTheme PlatformTheme { get; }
        protected IDispatcherWrapper DispatcherWrapper { get; }
        /// <summary>
        /// 対象アドオン一覧。
        /// </summary>
        protected IReadOnlyList<IAddon> Addons { get; }

        /// <summary>
        /// 担当する処理単位。
        /// </summary>
        protected abstract AddonKind AddonKind { get; }

        /// <summary>
        /// 処理単位一覧。
        /// <para>初回参照時に読み込まれる。</para>
        /// </summary>
        public IReadOnlyList<TFunctionUnit> FunctionUnits
        {
            get => this._functionUnits ??= LoadFunctionUnits();
        }

        #endregion

        #region function

        /// <summary>
        /// <see cref="AddonParameter"/> を普通に作成する。
        /// </summary>
        /// <returns></returns>
        protected virtual AddonParameter CreateParameter() => new AddonParameter(PlatformTheme, DispatcherWrapper, LoggerFactory);

        protected abstract TFunctionUnit BuildFunctionUnit(IAddon loadedAddon);

        protected IReadOnlyList<TFunctionUnit> LoadFunctionUnits()
        {
            var list = new List<TFunctionUnit>(Addons.Count);
            foreach(var addon in Addons) {
                Debug.Assert(addon.IsSupported(AddonKind));

                if(!addon.IsLoaded(Bridge.Plugin.PluginKind.Addon)) {
                    var pluginContextFactory = new PluginContextFactory(DatabaseBarrierPack, DatabaseLazyWriterPack, DatabaseStatementLoader, EnvironmentParameters, UserAgentManager, LoggerFactory);
                    using(var reader = DatabaseBarrierPack.WaitRead()) {
                        var loadContext = pluginContextFactory.CreateLoadContex(addon.PluginInformations, reader);
                        addon.Load(Bridge.Plugin.PluginKind.Addon, loadContext);
                    }
                }
                var functionUnit = BuildFunctionUnit(addon);
                list.Add(functionUnit);
            }
            return list;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    /// <inheritdoc cref="IAddonExecutor" />
    internal class AddonExecutor: IAddonExecutor
    {
        public AddonExecutor(IPluginInformations pluginInformations, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());

            PluginInformations = pluginInformations;

            Logger.LogTrace("ほぼほぼ未完成処理");
        }

        #region property

        /// <inheritdoc cref="ILoggerFactory"/>
        private ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        private ILogger Logger { get; }
        private IPluginInformations PluginInformations { get; }

        #endregion

        #region IAddonExecutor

        /// <inheritdoc cref="IAddonExecutor.Execute(string)" />
        public void Execute(string path)
        {
            var systemExecutor = new SystemExecutor();
            try {
                systemExecutor.ExecuteFile(path);
            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
            }
        }

        /// <inheritdoc cref="IAddonExecutor.Execute(string, IEnumerable{string})" />
        public void Execute(string path, IEnumerable<string> options)
        {
            Logger.LogWarning("未完成処理");
            Execute(path);
        }

        /// <inheritdoc cref="IAddonExecutor.Execute(string, IEnumerable{string}, string)" />
        public void Execute(string path, IEnumerable<string> options, string workDirectoryPath)
        {
            Logger.LogWarning("未完成処理");
            Execute(path);
        }

        /// <inheritdoc cref="IAddonExecutor.Execute(string, IEnumerable{string}, string, ShowMode)" />
        public void Execute(string path, IEnumerable<string> options, string workDirectoryPath, ShowMode showMode)
        {
            Logger.LogWarning("未完成処理");
            Execute(path);
        }

        /// <inheritdoc cref="IAddonExecutor.ExtendsExecute(string)" />
        public void ExtendsExecute(string path)
        {
            Logger.LogWarning("未完成処理");
            Execute(path);
        }

        /// <inheritdoc cref="IAddonExecutor.ExtendsExecute(string, IEnumerable{string})" />
        public void ExtendsExecute(string path, IEnumerable<string> options)
        {
            Logger.LogWarning("未完成処理");
            Execute(path);
        }

        /// <inheritdoc cref="IAddonExecutor.ExtendsExecute(string, IEnumerable{string}, string)" />
        public void ExtendsExecute(string path, IEnumerable<string> options, string workDirectoryPath)
        {
            Logger.LogWarning("未完成処理");
            Execute(path);
        }

        /// <inheritdoc cref="IAddonExecutor.ExtendsExecute(string, IEnumerable{string}, string, ShowMode)" />
        public void ExtendsExecute(string path, IEnumerable<string> options, string workDirectoryPath, ShowMode showMode)
        {
            Logger.LogWarning("未完成処理");
            Execute(path);
        }
        #endregion
    }
}

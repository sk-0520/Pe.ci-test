using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    /// <inheritdoc cref="ILauncherItemAddonFileStorage"/>
    public class LauncherItemAddonFileStorage: PluginFileStorageBase, ILauncherItemAddonFileStorage
    {
        public LauncherItemAddonFileStorage(DirectoryInfo directoryInfo)
            : base(directoryInfo)
        { }

        #region function

        private string ToDirectoryName(LauncherItemId launcherItemId) => launcherItemId.ToString();

        #endregion

        #region ILauncherItemAddonFileStorage

        /// <inheritdoc cref="ILauncherItemAddonFileStorage.Exists(LauncherItemId, string)"/>
        public bool Exists(LauncherItemId launcherItemId, string name)
        {
            return Exists(ToDirectoryName(launcherItemId), name);
        }

        /// <inheritdoc cref="ILauncherItemAddonFileStorage.Rename(LauncherItemId, string, string, bool)"/>
        public void Rename(LauncherItemId launcherItemId, string sourceName, string destinationName, bool overwrite)
        {
            Rename(ToDirectoryName(launcherItemId), sourceName, destinationName, overwrite);
        }

        /// <inheritdoc cref="ILauncherItemAddonFileStorage.Copy(LauncherItemId, string, string, bool)"/>
        public void Copy(LauncherItemId launcherItemId, string sourceName, string destinationName, bool overwrite)
        {
            Copy(ToDirectoryName(launcherItemId), sourceName, destinationName, overwrite);
        }

        /// <inheritdoc cref="ILauncherItemAddonFileStorage.Delete(LauncherItemId, string)"/>
        public void Delete(LauncherItemId launcherItemId, string name)
        {
            Delete(ToDirectoryName(launcherItemId), name);
        }

        /// <inheritdoc cref="ILauncherItemAddonFileStorage.Open(LauncherItemId, string, FileMode)"/>
        public Stream Open(LauncherItemId launcherItemId, string name, FileMode fileMode)
        {
            return Open(ToDirectoryName(launcherItemId), name, fileMode);
        }

        #endregion
    }

    /// <inheritdoc cref="ILauncherItemAddonPersistenceStorage"/>
    public sealed class LauncherItemAddonPersistenceStorage: PluginPersistenceStorageBase, ILauncherItemAddonPersistenceStorage
    {
        /// <inheritdoc cref="PluginPersistenceStorageBase.PluginPersistenceStorageBase(IPluginIdentifiers, IPluginVersions, IDatabaseContexts, IDatabaseStatementLoader, bool, ILoggerFactory)"/>
        public LauncherItemAddonPersistenceStorage(IPluginIdentifiers pluginIdentifiers, IPluginVersions pluginVersions, IDatabaseContexts databaseContexts, IDatabaseStatementLoader databaseStatementLoader, bool isReadOnly, ILoggerFactory loggerFactory)
            : base(pluginIdentifiers, pluginVersions, databaseContexts, databaseStatementLoader, isReadOnly, loggerFactory)
        { }

        /// <inheritdoc cref="PluginPersistenceStorageBase.PluginPersistenceStorageBase(IPluginIdentifiers, IPluginVersions, IDatabaseBarrier, IDatabaseStatementLoader, bool, ILoggerFactory)"/>
        public LauncherItemAddonPersistenceStorage(IPluginIdentifiers pluginIdentifiers, IPluginVersions pluginVersions, IDatabaseBarrier databaseBarrier, IDatabaseStatementLoader databaseStatementLoader, bool isReadOnly, ILoggerFactory loggerFactory)
            : base(pluginIdentifiers, pluginVersions, databaseBarrier, databaseStatementLoader, isReadOnly, loggerFactory)
        { }

        /// <inheritdoc cref="PluginPersistenceStorageBase.PluginPersistenceStorageBase(IPluginIdentifiers, IPluginVersions, IDatabaseBarrier, IDatabaseDelayWriter, IDatabaseStatementLoader, ILoggerFactory)"/>
        public LauncherItemAddonPersistenceStorage(IPluginIdentifiers pluginIdentifiers, IPluginVersions pluginVersions, IDatabaseBarrier databaseBarrier, IDatabaseDelayWriter databaseDelayWriter, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(pluginIdentifiers, pluginVersions, databaseBarrier, databaseDelayWriter, databaseStatementLoader, loggerFactory)
        { }

        #region ILauncherItemAddonPersistenceStorage

        public IEnumerable<string> GetKeys(LauncherItemId launcherItemId)
        {
            return GetKeysImpl((d) => {
                var pluginLauncherItemSettingsEntityDao = new PluginLauncherItemSettingsEntityDao(d.DatabaseContexts.Context, d.DatabaseStatementLoader, d.DatabaseContexts.Implementation, d.LoggerFactory);
                return pluginLauncherItemSettingsEntityDao.SelectPluginLauncherItemSettingKeys(PluginId, launcherItemId);
            });
        }

        public bool Exists(LauncherItemId launcherItemId, string key)
        {
            return ExistsImpl((launcherItemId, key), (p, d) => {
                var pluginLauncherItemSettingsEntityDao = new PluginLauncherItemSettingsEntityDao(d.DatabaseContexts.Context, d.DatabaseStatementLoader, d.DatabaseContexts.Implementation, d.LoggerFactory);
                return pluginLauncherItemSettingsEntityDao.SelectExistsPluginLauncherItemSetting(PluginId, launcherItemId, NormalizeKey(key));
            });
        }

        public bool TryGet<TValue>(LauncherItemId launcherItemId, string key, [MaybeNullWhen(returnValue: false)] out TValue value)
        {
            return TryGetImpl((launcherItemId, key), (p, d) => {
                var pluginLauncherItemSettingsEntityDao = new PluginLauncherItemSettingsEntityDao(d.DatabaseContexts.Context, d.DatabaseStatementLoader, d.DatabaseContexts.Implementation, d.LoggerFactory);
                return pluginLauncherItemSettingsEntityDao.SelectPluginLauncherItemValue(PluginId, p.launcherItemId, NormalizeKey(p.key));
            }, out value);
        }

        public bool Set<TValue>(LauncherItemId launcherItemId, string key, TValue value, PluginPersistenceFormat format)
            where TValue : notnull
        {
            return SetImpl(value, format, (launcherItemId, key), (p, d, v) => {
                var pluginLauncherItemSettingsEntityDao = new PluginLauncherItemSettingsEntityDao(d.DatabaseContexts.Context, d.DatabaseStatementLoader, d.DatabaseContexts.Implementation, d.LoggerFactory);
                var normalizedKey = NormalizeKey(p.key);
                if(pluginLauncherItemSettingsEntityDao.SelectExistsPluginLauncherItemSetting(PluginId, p.launcherItemId, normalizedKey)) {
                    pluginLauncherItemSettingsEntityDao.UpdatePluginLauncherItemSetting(PluginId, p.launcherItemId, normalizedKey, v, DatabaseCommonStatus.CreatePluginAccount(PluginIdentifiers, PluginVersions));
                } else {
                    pluginLauncherItemSettingsEntityDao.InsertPluginLauncherItemSetting(PluginId, p.launcherItemId, normalizedKey, v, DatabaseCommonStatus.CreatePluginAccount(PluginIdentifiers, PluginVersions));
                }
            });
        }
        public bool Set<TValue>(LauncherItemId launcherItemId, string key, TValue value)
            where TValue : notnull
        {
            return Set(launcherItemId, key, value, PluginPersistenceFormat.Json);
        }

        public bool Delete(LauncherItemId launcherItemId, string key)
        {
            return DeleteImpl((launcherItemId, key), (p, d) => {
                var pluginLauncherItemSettingsEntityDao = new PluginLauncherItemSettingsEntityDao(d.DatabaseContexts.Context, d.DatabaseStatementLoader, d.DatabaseContexts.Implementation, d.LoggerFactory);
                return pluginLauncherItemSettingsEntityDao.DeletePluginLauncherItemSetting(PluginId, p.launcherItemId, NormalizeKey(p.key));
            });
        }

        #endregion
    }

    /// <inheritdoc cref="ILauncherItemAddonFiles"/>
    public class LauncherItemAddonFiles: ILauncherItemAddonFiles
    {
        public LauncherItemAddonFiles(LauncherItemAddonFileStorage user, LauncherItemAddonFileStorage machine, LauncherItemAddonFileStorage temporary)
        {
            User = user;
            Machine = machine;
            Temporary = temporary;
        }

        #region ILauncherItemAddonFiles

        /// <inheritdoc cref="ILauncherItemAddonFiles.User"/>
        public LauncherItemAddonFileStorage User { get; }
        ILauncherItemAddonFileStorage ILauncherItemAddonFiles.User => User;
        /// <inheritdoc cref="ILauncherItemAddonFiles.Machine"/>
        public LauncherItemAddonFileStorage Machine { get; }
        ILauncherItemAddonFileStorage ILauncherItemAddonFiles.Machine => Machine;
        /// <inheritdoc cref="ILauncherItemAddonFiles.Temporary"/>
        public LauncherItemAddonFileStorage Temporary { get; }
        ILauncherItemAddonFileStorage ILauncherItemAddonFiles.Temporary => Temporary;

        #endregion
    }

    /// <inheritdoc cref="ILauncherItemAddonPersistence"/>
    public class LauncherItemAddonPersistence: ILauncherItemAddonPersistence
    {
        public LauncherItemAddonPersistence(LauncherItemAddonPersistenceStorage normal, LauncherItemAddonPersistenceStorage large, LauncherItemAddonPersistenceStorage temporary)
        {
            Normal = normal;
            Large = large;
            Temporary = temporary;
        }

        #region IPluginPersistence

        /// <inheritdoc cref="ILauncherItemAddonPersistence.Normal"/>
        public LauncherItemAddonPersistenceStorage Normal { get; }
        ILauncherItemAddonPersistenceStorage ILauncherItemAddonPersistence.Normal => Normal;
        /// <inheritdoc cref="ILauncherItemAddonPersistence.Large"/>
        public LauncherItemAddonPersistenceStorage Large { get; }
        ILauncherItemAddonPersistenceStorage ILauncherItemAddonPersistence.Large => Large;
        /// <inheritdoc cref="ILauncherItemAddonPersistence.Temporary"/>
        public LauncherItemAddonPersistenceStorage Temporary { get; }
        ILauncherItemAddonPersistenceStorage ILauncherItemAddonPersistence.Temporary => Temporary;

        #endregion
    }

    /// <inheritdoc cref="ILauncherItemAddonStorage"/>
    public class LauncherItemAddonStorage: ILauncherItemAddonStorage
    {
        public LauncherItemAddonStorage(LauncherItemAddonFiles file, LauncherItemAddonPersistence persistence)
        {
            File = file;
            Persistence = persistence;
        }

        #region ILauncherItemAddonStorage

        /// <inheritdoc cref="ILauncherItemAddonStorage.File"/>
        public LauncherItemAddonFiles File { get; }
        ILauncherItemAddonFiles ILauncherItemAddonStorage.File => File;

        /// <inheritdoc cref="ILauncherItemAddonStorage.Persistence"/>
        public LauncherItemAddonPersistence Persistence { get; }
        ILauncherItemAddonPersistence ILauncherItemAddonStorage.Persistence => Persistence;


        #endregion
    }
}

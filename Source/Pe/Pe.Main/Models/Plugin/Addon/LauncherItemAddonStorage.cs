using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Standard.Database;
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

    /// <inheritdoc cref="ILauncherItemAddonPersistentStorage"/>
    public sealed class LauncherItemAddonPersistentStorage: PluginPersistentStorageBase, ILauncherItemAddonPersistentStorage
    {
        /// <inheritdoc cref="PluginPersistentStorageBase.PluginPersistentStorageBase(IPluginIdentifiers, IPluginVersions, IDatabaseContexts, IDatabaseStatementLoader, bool, ILoggerFactory)"/>
        public LauncherItemAddonPersistentStorage(IPluginIdentifiers pluginIdentifiers, IPluginVersions pluginVersions, IDatabaseContexts databaseContexts, IDatabaseStatementLoader databaseStatementLoader, bool isReadOnly, ILoggerFactory loggerFactory)
            : base(pluginIdentifiers, pluginVersions, databaseContexts, databaseStatementLoader, isReadOnly, loggerFactory)
        { }

        /// <inheritdoc cref="PluginPersistentStorageBase.PluginPersistentStorageBase(IPluginIdentifiers, IPluginVersions, IDatabaseBarrier, IDatabaseStatementLoader, bool, ILoggerFactory)"/>
        public LauncherItemAddonPersistentStorage(IPluginIdentifiers pluginIdentifiers, IPluginVersions pluginVersions, IDatabaseBarrier databaseBarrier, IDatabaseStatementLoader databaseStatementLoader, bool isReadOnly, ILoggerFactory loggerFactory)
            : base(pluginIdentifiers, pluginVersions, databaseBarrier, databaseStatementLoader, isReadOnly, loggerFactory)
        { }

        /// <inheritdoc cref="PluginPersistentStorageBase.PluginPersistentStorageBase(IPluginIdentifiers, IPluginVersions, IDatabaseBarrier, IDatabaseLazyWriter, IDatabaseStatementLoader, ILoggerFactory)"/>
        public LauncherItemAddonPersistentStorage(IPluginIdentifiers pluginIdentifiers, IPluginVersions pluginVersions, IDatabaseBarrier databaseBarrier, IDatabaseLazyWriter databaseLazyWriter, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(pluginIdentifiers, pluginVersions, databaseBarrier, databaseLazyWriter, databaseStatementLoader, loggerFactory)
        { }

        #region ILauncherItemAddonPersistentStorage

        public bool Exists(LauncherItemId launcherItemId, string key)
        {
            return ExistsImpl((launcherItemId, key), (p, d) => {
                var pluginLauncherItemSettingsEntityDao = new PluginLauncherItemSettingsEntityDao(d.DatabaseContexts.Context, d.DatabaseStatementLoader, d.DatabaseContexts.Implementation, d.LoggerFactory);
                return pluginLauncherItemSettingsEntityDao.SelecteExistsPluginLauncherItemSetting(PluginId, launcherItemId, NormalizeKey(key));
            });
        }

        public bool TryGet<TValue>(LauncherItemId launcherItemId, string key, [MaybeNullWhen(returnValue: false)] out TValue value)
        {
            return TryGetImpl((launcherItemId, key), (p, d) => {
                var pluginLauncherItemSettingsEntityDao = new PluginLauncherItemSettingsEntityDao(d.DatabaseContexts.Context, d.DatabaseStatementLoader, d.DatabaseContexts.Implementation, d.LoggerFactory);
                return pluginLauncherItemSettingsEntityDao.SelectPluginLauncherItemValue(PluginId, p.launcherItemId, NormalizeKey(p.key));
            }, out value);
        }

        public bool Set<TValue>(LauncherItemId launcherItemId, string key, TValue value, PluginPersistentFormat format)
        {
            return SetImpl(value, format, (launcherItemId, key), (p, d, v) => {
                var pluginLauncherItemSettingsEntityDao = new PluginLauncherItemSettingsEntityDao(d.DatabaseContexts.Context, d.DatabaseStatementLoader, d.DatabaseContexts.Implementation, d.LoggerFactory);
                var normalizedKey = NormalizeKey(p.key);
                if(pluginLauncherItemSettingsEntityDao.SelecteExistsPluginLauncherItemSetting(PluginId, p.launcherItemId, normalizedKey)) {
                    pluginLauncherItemSettingsEntityDao.UpdatePluginLauncherItemSetting(PluginId, p.launcherItemId, normalizedKey, v, DatabaseCommonStatus.CreatePluginAccount(PluginIdentifiers, PluginVersions));
                } else {
                    pluginLauncherItemSettingsEntityDao.InsertPluginLauncherItemSetting(PluginId, p.launcherItemId, normalizedKey, v, DatabaseCommonStatus.CreatePluginAccount(PluginIdentifiers, PluginVersions));
                }
            });
        }
        public bool Set<TValue>(LauncherItemId launcherItemId, string key, TValue value) => Set(launcherItemId, key, value, PluginPersistentFormat.Json);

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

    /// <inheritdoc cref="ILauncherItemAddonPersistents"/>
    public class LauncherItemAddonPersistents: ILauncherItemAddonPersistents
    {
        public LauncherItemAddonPersistents(LauncherItemAddonPersistentStorage normal, LauncherItemAddonPersistentStorage large, LauncherItemAddonPersistentStorage temporary)
        {
            Normal = normal;
            Large = large;
            Temporary = temporary;
        }

        #region IPluginPersistent

        /// <inheritdoc cref="ILauncherItemAddonPersistents.Normal"/>
        public LauncherItemAddonPersistentStorage Normal { get; }
        ILauncherItemAddonPersistentStorage ILauncherItemAddonPersistents.Normal => Normal;
        /// <inheritdoc cref="ILauncherItemAddonPersistents.Large"/>
        public LauncherItemAddonPersistentStorage Large { get; }
        ILauncherItemAddonPersistentStorage ILauncherItemAddonPersistents.Large => Large;
        /// <inheritdoc cref="ILauncherItemAddonPersistents.Temporary"/>
        public LauncherItemAddonPersistentStorage Temporary { get; }
        ILauncherItemAddonPersistentStorage ILauncherItemAddonPersistents.Temporary => Temporary;

        #endregion
    }

    /// <inheritdoc cref="ILauncherItemAddonStorage"/>
    public class LauncherItemAddonStorage: ILauncherItemAddonStorage
    {
        public LauncherItemAddonStorage(LauncherItemAddonFiles file, LauncherItemAddonPersistents persistent)
        {
            File = file;
            Persistent = persistent;
        }

        #region ILauncherItemAddonStorage

        /// <inheritdoc cref="ILauncherItemAddonStorage.File"/>
        public LauncherItemAddonFiles File { get; }
        ILauncherItemAddonFiles ILauncherItemAddonStorage.File => File;

        /// <inheritdoc cref="ILauncherItemAddonStorage.Persistent"/>
        public LauncherItemAddonPersistents Persistent { get; }
        ILauncherItemAddonPersistents ILauncherItemAddonStorage.Persistent => Persistent;


        #endregion
    }
}

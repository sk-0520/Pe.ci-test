using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.Json;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
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

        string ToDirectoryName(Guid launcherItemId) => launcherItemId.ToString("D");

        #endregion

        #region ILauncherItemAddonFileStorage

        /// <inheritdoc cref="ILauncherItemAddonFileStorage.Exists(Guid, string)"/>
        public bool Exists(Guid launcherItemId, string name)
        {
            return Exists(ToDirectoryName(launcherItemId), name);
        }

        /// <inheritdoc cref="ILauncherItemAddonFileStorage.Rename(Guid, string, string, bool)"/>
        public void Rename(Guid launcherItemId, string sourceName, string destinationName, bool overwrite)
        {
            Rename(ToDirectoryName(launcherItemId), sourceName, destinationName, overwrite);
        }

        /// <inheritdoc cref="ILauncherItemAddonFileStorage.Copy(Guid, string, string, bool)"/>
        public void Copy(Guid launcherItemId, string sourceName, string destinationName, bool overwrite)
        {
            Copy(ToDirectoryName(launcherItemId), sourceName, destinationName, overwrite);
        }

        /// <inheritdoc cref="ILauncherItemAddonFileStorage.Delete(Guid, string)"/>
        public void Delete(Guid launcherItemId, string name)
        {
            Delete(ToDirectoryName(launcherItemId), name);
        }

        /// <inheritdoc cref="ILauncherItemAddonFileStorage.Open(Guid, string, FileMode)"/>
        public Stream Open(Guid launcherItemId, string name, FileMode fileMode)
        {
            return Open(ToDirectoryName(launcherItemId), name, fileMode);
        }

        #endregion
    }

    /// <inheritdoc cref="ILauncherItemAddonPersistentStorage"/>
    public sealed class LauncherItemAddonPersistentStorage: PluginPersistentStorageBase, ILauncherItemAddonPersistentStorage
    {
        /// <inheritdoc cref="PluginPersistentStorageBase.PluginPersistentStorageBase(IPluginIdentifiers, IPluginVersions, IDatabaseCommands, IDatabaseStatementLoader, bool, ILoggerFactory)"/>
        public LauncherItemAddonPersistentStorage(IPluginIdentifiers pluginIdentifiers, IPluginVersions pluginVersions, IDatabaseCommands databaseCommands, IDatabaseStatementLoader databaseStatementLoader, bool isReadOnly, ILoggerFactory loggerFactory)
            : base(pluginIdentifiers, pluginVersions, databaseCommands, databaseStatementLoader, isReadOnly, loggerFactory)
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

        public bool Exists(Guid launcherItemId, string key)
        {
            switch(Mode) {
                case PluginPersistentMode.Commander: {
                        Debug.Assert(DatabaseCommander != null);
                        Debug.Assert(DatabaseImplementation != null);

                        var pluginLauncherItemSettingsEntityDao = new PluginLauncherItemSettingsEntityDao(DatabaseCommander, DatabaseStatementLoader, DatabaseImplementation, LoggerFactory);
                        return pluginLauncherItemSettingsEntityDao.SelecteExistsPluginLauncherItemSetting(PluginId, launcherItemId, NormalizeKey(key));
                    }

                case PluginPersistentMode.Barrier:
                case PluginPersistentMode.LazyWriter: {
                        Debug.Assert(DatabaseBarrier != null);

                        if(Mode == PluginPersistentMode.LazyWriter) {
                            Debug.Assert(DatabaseLazyWriter != null);
                            DatabaseLazyWriter.Flush();
                        }

                        return DatabaseBarrier.ReadData(c => {
                            var pluginLauncherItemSettingsEntityDao = new PluginLauncherItemSettingsEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                            return pluginLauncherItemSettingsEntityDao.SelecteExistsPluginLauncherItemSetting(PluginId, launcherItemId, NormalizeKey(key));
                        });
                    }

                default:
                    throw new NotImplementedException();
            }

        }

        public bool TryGet<TValue>(Guid launcherItemId, string key, [MaybeNullWhen(returnValue: false)] out TValue value)
        {
            if(Mode == PluginPersistentMode.LazyWriter) {
                // 遅延書き込み待機を終了
                Debug.Assert(DatabaseLazyWriter != null);
                DatabaseLazyWriter.Flush();
            }

            PluginSettingRawValue? data;
            switch(Mode) {
                case PluginPersistentMode.Commander: {
                        Debug.Assert(DatabaseCommander != null);
                        Debug.Assert(DatabaseImplementation != null);

                        var pluginLauncherItemSettingsEntityDao = new PluginLauncherItemSettingsEntityDao(DatabaseCommander, DatabaseStatementLoader, DatabaseImplementation, LoggerFactory);
                        data = pluginLauncherItemSettingsEntityDao.SelectPluginLauncherItemValue(PluginId, launcherItemId, NormalizeKey(key));
                    }
                    break;

                case PluginPersistentMode.Barrier:
                case PluginPersistentMode.LazyWriter: {
                        Debug.Assert(DatabaseBarrier != null);

                        if(Mode == PluginPersistentMode.LazyWriter) {
                            Debug.Assert(DatabaseLazyWriter != null);
                            DatabaseLazyWriter.Flush();
                        }

                        data = DatabaseBarrier.ReadData(c => {
                            var pluginLauncherItemSettingsEntityDao = new PluginLauncherItemSettingsEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                            return pluginLauncherItemSettingsEntityDao.SelectPluginLauncherItemValue(PluginId, launcherItemId, NormalizeKey(key));
                        });
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }

            if(data == null) {
                value = default;
                return false;
            }

            switch(data.Format) {
                case PluginPersistentFormat.SimpleXml:
                case PluginPersistentFormat.DataXml: {
                        SerializerBase serializer = data.Format switch
                        {
                            PluginPersistentFormat.SimpleXml => new XmlSerializer(),
                            PluginPersistentFormat.DataXml => new XmlDataContractSerializer(),
                            _ => throw new NotImplementedException(),
                        };
                        try {
                            var binary = Encoding.UTF8.GetBytes(data.Value);
                            using(var stream = new MemoryStream(binary)) {
                                value = serializer.Load<TValue>(stream);
                                return true;
                            }
                        } catch(Exception ex) {
                            Logger.LogError(ex, ex.Message);
                            value = default;
                            return false;
                        }
                    }

                case PluginPersistentFormat.Json: {
                        try {
                            value = JsonSerializer.Deserialize<TValue>(data.Value);
                            return true;
                        } catch(Exception ex) {
                            Logger.LogError(ex, ex.Message);
                            value = default;
                            return false;
                        }
                    }

                case PluginPersistentFormat.Text: {
                        if(typeof(TValue) != typeof(string)) {
                            Logger.LogWarning("文字列であるべきデータ: {0} -> {1}", nameof(value), typeof(TValue));
                            value = default;
                            return false;
                        }

                        value = (TValue)(object)data.Value;
                        return true;
                    }

                default:
                    throw new NotImplementedException();
            }
        }

        public bool Set<TValue>(Guid launcherItemId, string key, TValue value, PluginPersistentFormat format)
        {
            if(IsReadOnly) {
                throw new InvalidOperationException(nameof(IsReadOnly));
            }

            if(value == null) {
                Logger.LogWarning("value は null のため処理終了");
                return false;
            }

            string textValue;

            switch(format) {
                case PluginPersistentFormat.SimpleXml:
                case PluginPersistentFormat.DataXml: {
                        SerializerBase serializer = format switch
                        {
                            PluginPersistentFormat.SimpleXml => new XmlSerializer(),
                            PluginPersistentFormat.DataXml => new XmlDataContractSerializer(),
                            _ => throw new NotImplementedException(),
                        };
                        try {
                            using(var stream = new MemoryStream()) {
                                serializer.Save(value, stream);
                                textValue = serializer.Encoding.GetString(stream.GetBuffer(), 0, (int)stream.Length);
                            }
                        } catch(Exception ex) {
                            Logger.LogError(ex, ex.Message);
                            return false;
                        }
                    }
                    break;

                case PluginPersistentFormat.Json: {
                        try {
                            textValue = JsonSerializer.Serialize(value);
                        } catch(Exception ex) {
                            Logger.LogError(ex, ex.Message);
                            return false;
                        }
                    }
                    break;

                case PluginPersistentFormat.Text: {
                        if(typeof(TValue) != typeof(string)) {
                            Logger.LogWarning("文字列であるべきデータ: {0} -> {1}", nameof(value), typeof(TValue));
                            textValue = value.ToString()! ?? string.Empty;
                        } else {
                            textValue = (string)(object)value;
                        }
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }

            var data = new PluginSettingRawValue(format, textValue);

            switch(Mode) {
                case PluginPersistentMode.Commander: {
                        Debug.Assert(DatabaseCommander != null);
                        Debug.Assert(DatabaseImplementation != null);

                        var pluginLauncherItemSettingsEntityDao = new PluginLauncherItemSettingsEntityDao(DatabaseCommander, DatabaseStatementLoader, DatabaseImplementation, LoggerFactory);

                        var normalizedKey = NormalizeKey(key);
                        if(pluginLauncherItemSettingsEntityDao.SelecteExistsPluginLauncherItemSetting(PluginId, launcherItemId, normalizedKey)) {
                            pluginLauncherItemSettingsEntityDao.UpdatePluginLauncherItemSetting(PluginId, launcherItemId, normalizedKey, data, DatabaseCommonStatus.CreatePluginAccount(PluginIdentifiers, PluginVersions));
                        } else {
                            pluginLauncherItemSettingsEntityDao.InsertPluginLauncherItemSetting(PluginId, launcherItemId, normalizedKey, data, DatabaseCommonStatus.CreatePluginAccount(PluginIdentifiers, PluginVersions));
                        }
                    }
                    break;

                case PluginPersistentMode.Barrier: {
                        Debug.Assert(DatabaseBarrier != null);

                        using(var commander = DatabaseBarrier.WaitWrite()) {
                            var pluginLauncherItemSettingsEntityDao = new PluginLauncherItemSettingsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                            var normalizedKey = NormalizeKey(key);
                            if(pluginLauncherItemSettingsEntityDao.SelecteExistsPluginLauncherItemSetting(PluginId, launcherItemId, normalizedKey)) {
                                pluginLauncherItemSettingsEntityDao.UpdatePluginLauncherItemSetting(PluginId, launcherItemId, normalizedKey, data, DatabaseCommonStatus.CreatePluginAccount(PluginIdentifiers, PluginVersions));
                            } else {
                                pluginLauncherItemSettingsEntityDao.InsertPluginLauncherItemSetting(PluginId, launcherItemId, normalizedKey, data, DatabaseCommonStatus.CreatePluginAccount(PluginIdentifiers, PluginVersions));
                            }
                            commander.Commit();
                        }
                    }
                    break;

                case PluginPersistentMode.LazyWriter: {
                        Debug.Assert(DatabaseLazyWriter != null);

                        DatabaseLazyWriter.Stock(c => {
                            var pluginLauncherItemSettingsEntityDao = new PluginLauncherItemSettingsEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                            var normalizedKey = NormalizeKey(key);
                            if(pluginLauncherItemSettingsEntityDao.SelecteExistsPluginLauncherItemSetting(PluginId, launcherItemId, normalizedKey)) {
                                pluginLauncherItemSettingsEntityDao.UpdatePluginLauncherItemSetting(PluginId, launcherItemId, normalizedKey, data, DatabaseCommonStatus.CreatePluginAccount(PluginIdentifiers, PluginVersions));
                            } else {
                                pluginLauncherItemSettingsEntityDao.InsertPluginLauncherItemSetting(PluginId, launcherItemId, normalizedKey, data, DatabaseCommonStatus.CreatePluginAccount(PluginIdentifiers, PluginVersions));
                            }
                        });
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }

            return true;
        }
        public bool Set<TValue>(Guid launcherItemId, string key, TValue value) => Set(launcherItemId, key, value, PluginPersistentFormat.Json);

        public bool Delete(Guid launcherItemId, string key)
        {
            if(IsReadOnly) {
                throw new InvalidOperationException(nameof(IsReadOnly));
            }

            if(Mode == PluginPersistentMode.LazyWriter) {
                // 遅延書き込み待機を終了
                Debug.Assert(DatabaseLazyWriter != null);
                DatabaseLazyWriter.Flush();
            }

            switch(Mode) {
                case PluginPersistentMode.Commander: {
                        Debug.Assert(DatabaseCommander != null);
                        Debug.Assert(DatabaseImplementation != null);

                        var pluginLauncherItemSettingsEntityDao = new PluginLauncherItemSettingsEntityDao(DatabaseCommander, DatabaseStatementLoader, DatabaseImplementation, LoggerFactory);
                        return pluginLauncherItemSettingsEntityDao.DeletePluginLauncherItemSetting(PluginId, launcherItemId, NormalizeKey(key));
                    }

                case PluginPersistentMode.Barrier: {
                        Debug.Assert(DatabaseBarrier != null);

                        using(var commander = DatabaseBarrier.WaitWrite()) {
                            var pluginLauncherItemSettingsEntityDao = new PluginLauncherItemSettingsEntityDao(commander, DatabaseStatementLoader, commander.Implementation, LoggerFactory);
                            var result = pluginLauncherItemSettingsEntityDao.DeletePluginLauncherItemSetting(PluginId, launcherItemId, NormalizeKey(key));
                            commander.Commit();
                            return result;
                        }
                    }

                case PluginPersistentMode.LazyWriter: {
                        Debug.Assert(DatabaseLazyWriter != null);

                        DatabaseLazyWriter.Stock(c => {
                            var pluginLauncherItemSettingsEntityDao = new PluginLauncherItemSettingsEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                            pluginLauncherItemSettingsEntityDao.DeletePluginLauncherItemSetting(PluginId, launcherItemId, NormalizeKey(key));
                        });
                        // 成功したかどうか不明
                        return false;
                    }

                default:
                    throw new NotImplementedException();
            }

            #endregion
        }
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

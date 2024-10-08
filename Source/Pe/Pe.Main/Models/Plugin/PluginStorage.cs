using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Core.Models.Serialization;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    public abstract class PluginFileStorageBase
    {
        protected PluginFileStorageBase(DirectoryInfo directoryInfo)
        {
            DirectoryInfo = directoryInfo;
        }

        #region property

        protected DirectoryInfo DirectoryInfo { get; }

        #endregion

        #region function

        protected string AdjustFileName(string name)
        {
            if(name == null) {
                throw new ArgumentNullException(nameof(name));
            }

            if(string.IsNullOrWhiteSpace(name)) {
                throw new ArgumentException(null, nameof(name));
            }

            var s = name.Trim();
            var cs = Path.GetInvalidFileNameChars();
            if(s.Any(i => cs.Any(cc => cc == i))) {
                throw new ArgumentException(null, nameof(name));
            }

            return s;
        }

        protected string CombinePath(string directoryName, string fileName)
        {
            var path = string.IsNullOrEmpty(directoryName)
                ? Path.Combine(DirectoryInfo.FullName, fileName)
                : Path.Combine(DirectoryInfo.FullName, directoryName, fileName)
            ;
            return path;
        }

        protected bool Exists(string directoryName, string fileName)
        {
            Debug.Assert(directoryName != null);

            var tunedFileName = AdjustFileName(fileName);
            var path = CombinePath(directoryName, tunedFileName);

            return File.Exists(path);
        }

        protected void Rename(string directoryName, string sourceName, string destinationName, bool overwrite)
        {
            Debug.Assert(directoryName != null);

            if(sourceName == destinationName) {
                throw new ArgumentException($"{nameof(sourceName)} == {nameof(destinationName)}");
            }

            var tunedSourceFileName = AdjustFileName(sourceName);
            var tunedDestinationFileName = AdjustFileName(destinationName);

            var tunedSourceFilePath = CombinePath(directoryName, tunedSourceFileName);
            var tunedDestinationFilePath = CombinePath(directoryName, tunedDestinationFileName);

            File.Move(tunedSourceFilePath, tunedDestinationFilePath, overwrite);
        }

        protected void Copy(string directoryName, string sourceName, string destinationName, bool overwrite)
        {
            Debug.Assert(directoryName != null);

            if(sourceName == destinationName) {
                throw new ArgumentException($"{nameof(sourceName)} == {nameof(destinationName)}");
            }

            var tunedSourceFileName = AdjustFileName(sourceName);
            var tunedDestinationFileName = AdjustFileName(destinationName);

            var tunedSourceFilePath = CombinePath(directoryName, tunedSourceFileName);
            var tunedDestinationFilePath = CombinePath(directoryName, tunedDestinationFileName);

            File.Copy(tunedSourceFilePath, tunedDestinationFilePath, overwrite);
        }

        protected void Delete(string directoryName, string name)
        {
            Debug.Assert(directoryName != null);

            var tunedFileName = AdjustFileName(name);
            var path = CombinePath(directoryName, tunedFileName);
            File.Delete(path);
        }

        protected Stream Open(string directoryName, string name, FileMode fileMode)
        {
            Debug.Assert(directoryName != null);

            var tunedFileName = AdjustFileName(name);
            var path = CombinePath(directoryName, tunedFileName);
            IOUtility.MakeFileParentDirectory(path);
            var stream = new FileStream(path, fileMode, FileAccess.ReadWrite, FileShare.Read);
            return stream;
        }

        #endregion
    }

    /// <inheritdoc cref="IPluginFileStorage"/>
    public class PluginFileStorage: PluginFileStorageBase, IPluginFileStorage
    {
        public PluginFileStorage(DirectoryInfo directoryInfo)
            : base(directoryInfo)
        { }

        #region property
        #endregion

        #region function
        #endregion

        #region IPluginFileStorage

        /// <inheritdoc cref="IPluginFileStorage.Exists(string)"/>
        public bool Exists(string name)
        {
            return Exists(string.Empty, name);
        }

        /// <inheritdoc cref="IPluginFileStorage.Rename(string, string, bool)"/>
        public void Rename(string sourceName, string destinationName, bool overwrite)
        {
            Rename(string.Empty, sourceName, destinationName, overwrite);
        }

        /// <inheritdoc cref="IPluginFileStorage.Copy(string, string, bool)"/>
        public void Copy(string sourceName, string destinationName, bool overwrite)
        {
            Copy(string.Empty, sourceName, destinationName, overwrite);
        }

        /// <inheritdoc cref="IPluginFileStorage.Delete(string)"/>
        public void Delete(string name)
        {
            Delete(string.Empty, name);
        }

        /// <inheritdoc cref="IPluginFileStorage.Open(string, FileMode)"/>
        public Stream Open(string name, FileMode fileMode)
        {
            return Open(string.Empty, name, fileMode);
        }

        #endregion
    }

    public enum PluginPersistenceMode
    {
        /// <summary>
        /// 上流に影響される読み書き。
        /// </summary>
        /// <remarks>
        /// <para>書き込みは状況により不可。</para>
        /// </remarks>
        Context,
        /// <summary>
        /// 単独実施する読み書き。
        /// </summary>
        /// <remarks>
        /// <para>書き込みは状況により不可。</para>
        /// </remarks>
        Barrier,
        /// <summary>
        /// 遅延書き込み。
        /// </summary>
        DelayWriter,
    }

    public abstract class PluginPersistenceStorageBase: IPluginId
    {
        #region define

        protected class DatabaseParameter
        {
            public DatabaseParameter(IDatabaseStatementLoader databaseStatementLoader, IDatabaseContexts databaseContexts, ILoggerFactory loggerFactory)
            {
                DatabaseStatementLoader = databaseStatementLoader;
                DatabaseContexts = databaseContexts;
                LoggerFactory = loggerFactory;
            }

            #region property

            /// <inheritdoc cref="ILoggerFactory"/>
            public ILoggerFactory LoggerFactory { get; }
            public IDatabaseStatementLoader DatabaseStatementLoader { get; }
            public IDatabaseContexts DatabaseContexts { get; }

            #endregion
        }

        #endregion

        /// <summary>
        /// プラグイン用DB操作処理構築。
        /// </summary>
        /// <remarks>
        /// <para>読み込み専用・書き込み可能に分かれる。書き込み専用は基本的に設定画面くらい。</para>
        /// </remarks>
        /// <param name="pluginIdentifiers"></param>
        /// <param name="pluginVersions"></param>
        /// <param name="databaseContexts"></param>
        /// <param name="databaseStatementLoader"></param>
        /// <param name="isReadOnly">読み込み専用か。</param>
        /// <param name="loggerFactory"></param>
        protected PluginPersistenceStorageBase(IPluginIdentifiers pluginIdentifiers, IPluginVersions pluginVersions, IDatabaseContexts databaseContexts, IDatabaseStatementLoader databaseStatementLoader, bool isReadOnly, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            PluginIdentifiers = pluginIdentifiers;
            PluginVersions = pluginVersions;
            DatabaseContext = databaseContexts.Context;
            DatabaseImplementation = databaseContexts.Implementation;
            IsReadOnly = isReadOnly;
            DatabaseStatementLoader = databaseStatementLoader;
            Mode = PluginPersistenceMode.Context;
        }

        /// <summary>
        /// プラグイン用DB操作処理構築。
        /// </summary>
        /// <remarks>
        /// <para>読み込み専用・書き込み可能に分かれる。逐次処理される。</para>
        /// </remarks>
        /// <param name="pluginIdentifiers"></param>
        /// <param name="pluginVersions"></param>
        /// <param name="databaseBarrier"></param>
        /// <param name="databaseStatementLoader"></param>
        /// <param name="isReadOnly">読み込み専用か。</param>
        /// <param name="loggerFactory"></param>
        protected PluginPersistenceStorageBase(IPluginIdentifiers pluginIdentifiers, IPluginVersions pluginVersions, IDatabaseBarrier databaseBarrier, IDatabaseStatementLoader databaseStatementLoader, bool isReadOnly, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            PluginIdentifiers = pluginIdentifiers;
            PluginVersions = pluginVersions;
            DatabaseBarrier = databaseBarrier;
            IsReadOnly = isReadOnly;
            DatabaseStatementLoader = databaseStatementLoader;
            Mode = PluginPersistenceMode.Barrier;
        }

        /// <summary>
        /// プラグイン用DB操作処理構築。
        /// </summary>
        /// <remarks>
        /// <para>通常実行時の書き込み処理で、遅延処理される。</para>
        /// </remarks>
        /// <param name="pluginIdentifiers"></param>
        /// <param name="pluginVersions"></param>
        /// <param name="databaseBarrier"></param>
        /// <param name="databaseDelayWriter"></param>
        /// <param name="databaseStatementLoader"></param>
        /// <param name="loggerFactory"></param>
        protected PluginPersistenceStorageBase(IPluginIdentifiers pluginIdentifiers, IPluginVersions pluginVersions, IDatabaseBarrier databaseBarrier, IDatabaseDelayWriter databaseDelayWriter, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            PluginIdentifiers = pluginIdentifiers;
            PluginVersions = pluginVersions;
            DatabaseBarrier = databaseBarrier;
            DatabaseDelayWriter = databaseDelayWriter;
            DatabaseStatementLoader = databaseStatementLoader;
            IsReadOnly = false;
            Mode = PluginPersistenceMode.DelayWriter;
        }

        #region property

        /// <inheritdoc cref="ILoggerFactory"/>
        protected ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        protected ILogger Logger { get; }

        protected IPluginIdentifiers PluginIdentifiers { get; }
        public string PluginName => PluginIdentifiers.PluginName;
        protected IPluginVersions PluginVersions { get; }

        protected PluginPersistenceMode Mode { get; }
        protected IDatabaseImplementation? DatabaseImplementation { get; }
        protected IDatabaseContext? DatabaseContext { get; }
        protected IDatabaseBarrier? DatabaseBarrier { get; }
        protected IDatabaseDelayWriter? DatabaseDelayWriter { get; }
        protected IDatabaseStatementLoader DatabaseStatementLoader { get; }

        public bool IsReadOnly { get; }

        #endregion

        #region function

        protected string NormalizeKey(string key)
        {
            return string.IsNullOrWhiteSpace(key)
                ? string.Empty
                : key.Trim()
            ;
        }

        protected IEnumerable<string> GetKeysImpl(Func<DatabaseParameter, IEnumerable<string>> func)
        {
            switch(Mode) {
                case PluginPersistenceMode.Context: {
                        Debug.Assert(DatabaseContext != null);
                        Debug.Assert(DatabaseImplementation != null);

                        return func(new DatabaseParameter(DatabaseStatementLoader, new DatabaseContexts(DatabaseContext, DatabaseImplementation), LoggerFactory));
                    }

                case PluginPersistenceMode.Barrier:
                case PluginPersistenceMode.DelayWriter: {
                        Debug.Assert(DatabaseBarrier != null);

                        if(Mode == PluginPersistenceMode.DelayWriter) {
                            Debug.Assert(DatabaseDelayWriter != null);
                            DatabaseDelayWriter.Flush();
                        }

                        return DatabaseBarrier.ReadData(c => {
                            return func(new DatabaseParameter(DatabaseStatementLoader, new DatabaseContexts(c, c.Implementation), LoggerFactory));
                        });
                    }

                default:
                    throw new NotImplementedException();
            }
        }

        protected bool ExistsImpl<TParameter>(TParameter parameter, Func<TParameter, DatabaseParameter, bool> func)
        {
            switch(Mode) {
                case PluginPersistenceMode.Context: {
                        Debug.Assert(DatabaseContext != null);
                        Debug.Assert(DatabaseImplementation != null);

                        return func(parameter, new DatabaseParameter(DatabaseStatementLoader, new DatabaseContexts(DatabaseContext, DatabaseImplementation), LoggerFactory));
                    }

                case PluginPersistenceMode.Barrier:
                case PluginPersistenceMode.DelayWriter: {
                        Debug.Assert(DatabaseBarrier != null);

                        if(Mode == PluginPersistenceMode.DelayWriter) {
                            Debug.Assert(DatabaseDelayWriter != null);
                            DatabaseDelayWriter.Flush();
                        }

                        return DatabaseBarrier.ReadData(c => {
                            return func(parameter, new DatabaseParameter(DatabaseStatementLoader, new DatabaseContexts(c, c.Implementation), LoggerFactory));
                        });
                    }

                default:
                    throw new NotImplementedException();
            }
        }

        protected bool TryGetImpl<TValue, TParameter>(TParameter parameter, Func<TParameter, DatabaseParameter, PluginSettingRawValue?> func, [MaybeNullWhen(returnValue: false)] out TValue value)
        {
            if(Mode == PluginPersistenceMode.DelayWriter) {
                // 遅延書き込み待機を終了
                Debug.Assert(DatabaseDelayWriter != null);
                DatabaseDelayWriter.Flush();
            }

            PluginSettingRawValue? data;
            switch(Mode) {
                case PluginPersistenceMode.Context: {
                        Debug.Assert(DatabaseContext != null);
                        Debug.Assert(DatabaseImplementation != null);

                        data = func(parameter, new DatabaseParameter(DatabaseStatementLoader, new DatabaseContexts(DatabaseContext, DatabaseImplementation), LoggerFactory));
                    }
                    break;

                case PluginPersistenceMode.Barrier:
                case PluginPersistenceMode.DelayWriter: {
                        Debug.Assert(DatabaseBarrier != null);

                        if(Mode == PluginPersistenceMode.DelayWriter) {
                            Debug.Assert(DatabaseDelayWriter != null);
                            DatabaseDelayWriter.Flush();
                        }

                        data = DatabaseBarrier.ReadData(c => {
                            return func(parameter, new DatabaseParameter(DatabaseStatementLoader, new DatabaseContexts(c, c.Implementation), LoggerFactory));
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
                case PluginPersistenceFormat.SimpleXml:
                case PluginPersistenceFormat.DataXml: {
                        SerializerBase serializer = data.Format switch {
                            PluginPersistenceFormat.SimpleXml => new XmlSerializer(),
                            PluginPersistenceFormat.DataXml => new XmlDataContractSerializer(),
                            _ => throw new NotImplementedException(),
                        };
                        try {
                            var binary = Encoding.UTF8.GetBytes(data.Value);
                            using(var stream = new MemoryReleaseStream(binary)) {
                                value = serializer.Load<TValue>(stream);
                                return true;
                            }
                        } catch(Exception ex) {
                            Logger.LogError(ex, ex.Message);
                            value = default;
                            return false;
                        }
                    }

                case PluginPersistenceFormat.Json: {
                        try {
                            value = JsonSerializer.Deserialize<TValue>(data.Value)!;
                            return true;
                        } catch(Exception ex) {
                            Logger.LogError(ex, ex.Message);
                            value = default;
                            return false;
                        }
                    }

                case PluginPersistenceFormat.Text: {
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

        protected bool SetImpl<TValue, TParameter>(TValue value, PluginPersistenceFormat format, TParameter parameter, Action<TParameter, DatabaseParameter, PluginSettingRawValue> action)
            where TValue : notnull
        {
            if(IsReadOnly) {
                throw new InvalidOperationException(nameof(IsReadOnly));
            }

            string textValue;

            switch(format) {
                case PluginPersistenceFormat.SimpleXml:
                case PluginPersistenceFormat.DataXml: {
                        SerializerBase serializer = format switch {
                            PluginPersistenceFormat.SimpleXml => new XmlSerializer(),
                            PluginPersistenceFormat.DataXml => new XmlDataContractSerializer(),
                            _ => throw new NotImplementedException(),
                        };
                        try {
                            using(var stream = new MemoryReleaseStream()) {
                                serializer.Save(value, stream);
                                textValue = serializer.Encoding.GetString(stream.GetBuffer(), 0, (int)stream.Length);
                            }
                        } catch(Exception ex) {
                            Logger.LogError(ex, ex.Message);
                            return false;
                        }
                    }
                    break;

                case PluginPersistenceFormat.Json: {
                        try {
                            textValue = JsonSerializer.Serialize(value);
                        } catch(Exception ex) {
                            Logger.LogError(ex, ex.Message);
                            return false;
                        }
                    }
                    break;

                case PluginPersistenceFormat.Text: {
                        if(typeof(TValue) != typeof(string)) {
                            Logger.LogWarning("文字列であるべきデータ: {Value} -> {TValue}", nameof(value), typeof(TValue));
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
                case PluginPersistenceMode.Context: {
                        Debug.Assert(DatabaseContext != null);
                        Debug.Assert(DatabaseImplementation != null);

                        action(parameter, new DatabaseParameter(DatabaseStatementLoader, new DatabaseContexts(DatabaseContext, DatabaseImplementation), LoggerFactory), data);
                    }
                    break;

                case PluginPersistenceMode.Barrier: {
                        Debug.Assert(DatabaseBarrier != null);

                        using(var context = DatabaseBarrier.WaitWrite()) {
                            action(parameter, new DatabaseParameter(DatabaseStatementLoader, new DatabaseContexts(context, context.Implementation), LoggerFactory), data);
                            context.Commit();
                        }
                    }
                    break;

                case PluginPersistenceMode.DelayWriter: {
                        Debug.Assert(DatabaseDelayWriter != null);

                        DatabaseDelayWriter.Stock(c => {
                            action(parameter, new DatabaseParameter(DatabaseStatementLoader, new DatabaseContexts(c, c.Implementation), LoggerFactory), data);
                        });
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }

            return true;
        }

        protected bool DeleteImpl<TParameter>(TParameter parameter, Func<TParameter, DatabaseParameter, bool> func)
        {
            if(IsReadOnly) {
                throw new InvalidOperationException(nameof(IsReadOnly));
            }

            if(Mode == PluginPersistenceMode.DelayWriter) {
                // 遅延書き込み待機を終了
                Debug.Assert(DatabaseDelayWriter != null);
                DatabaseDelayWriter.Flush();
            }

            switch(Mode) {
                case PluginPersistenceMode.Context: {
                        Debug.Assert(DatabaseContext != null);
                        Debug.Assert(DatabaseImplementation != null);

                        return func(parameter, new DatabaseParameter(DatabaseStatementLoader, new DatabaseContexts(DatabaseContext, DatabaseImplementation), LoggerFactory));
                    }

                case PluginPersistenceMode.Barrier: {
                        Debug.Assert(DatabaseBarrier != null);

                        using(var context = DatabaseBarrier.WaitWrite()) {
                            var result = func(parameter, new DatabaseParameter(DatabaseStatementLoader, new DatabaseContexts(context, context.Implementation), LoggerFactory));
                            context.Commit();
                            return result;
                        }
                    }

                case PluginPersistenceMode.DelayWriter: {
                        Debug.Assert(DatabaseDelayWriter != null);

                        DatabaseDelayWriter.Stock(c => {
                            var result = func(parameter, new DatabaseParameter(DatabaseStatementLoader, new DatabaseContexts(c, c.Implementation), LoggerFactory));
                            Logger.LogWarning("result = {0}", result);
                        });
                        // 成功したかどうか不明
                        return false;
                    }

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        #region IPluginId

        public PluginId PluginId => PluginIdentifiers.PluginId;

        #endregion
    }

    /// <inheritdoc cref="IPluginPersistenceStorage"/>
    public sealed class PluginPersistenceStorage: PluginPersistenceStorageBase, IPluginPersistenceStorage
    {
        /// <inheritdoc cref="PluginPersistenceStorageBase.PluginPersistenceStorageBase(IPluginIdentifiers, IPluginVersions, IDatabaseContexts, IDatabaseStatementLoader, bool, ILoggerFactory)"/>
        public PluginPersistenceStorage(IPluginIdentifiers pluginIdentifiers, IPluginVersions pluginVersions, IDatabaseContexts databaseContexts, IDatabaseStatementLoader databaseStatementLoader, bool isReadOnly, ILoggerFactory loggerFactory)
            : base(pluginIdentifiers, pluginVersions, databaseContexts, databaseStatementLoader, isReadOnly, loggerFactory)
        { }

        /// <inheritdoc cref="PluginPersistenceStorageBase.PluginPersistenceStorageBase(IPluginIdentifiers, IPluginVersions, IDatabaseBarrier, IDatabaseStatementLoader, bool, ILoggerFactory)"/>
        public PluginPersistenceStorage(IPluginIdentifiers pluginIdentifiers, IPluginVersions pluginVersions, IDatabaseBarrier databaseBarrier, IDatabaseStatementLoader databaseStatementLoader, bool isReadOnly, ILoggerFactory loggerFactory)
            : base(pluginIdentifiers, pluginVersions, databaseBarrier, databaseStatementLoader, isReadOnly, loggerFactory)
        { }

        /// <inheritdoc cref="PluginPersistenceStorageBase.PluginPersistenceStorageBase(IPluginIdentifiers, IPluginVersions, IDatabaseBarrier, IDatabaseDelayWriter, IDatabaseStatementLoader, ILoggerFactory)"/>
        public PluginPersistenceStorage(IPluginIdentifiers pluginIdentifiers, IPluginVersions pluginVersions, IDatabaseBarrier databaseBarrier, IDatabaseDelayWriter databaseDelayWriter, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(pluginIdentifiers, pluginVersions, databaseBarrier, databaseDelayWriter, databaseStatementLoader, loggerFactory)
        { }

        #region property


        #endregion

        #region function

        #endregion

        #region IPluginPersistenceStorage

        /// <inheritdoc cref="IPluginPersistenceStorage.GetKeys()"/>
        public IEnumerable<string> GetKeys()
        {
            return GetKeysImpl((d) => {
                var pluginSettingsEntityDao = new PluginSettingsEntityDao(d.DatabaseContexts.Context, d.DatabaseStatementLoader, d.DatabaseContexts.Implementation, d.LoggerFactory);
                return pluginSettingsEntityDao.SelectPluginSettingKeys(PluginId);
            });
        }

        /// <inheritdoc cref="IPluginPersistenceStorage.Exists(string)"/>
        public bool Exists(string key)
        {
            return ExistsImpl(key, (p, d) => {
                var pluginSettingsEntityDao = new PluginSettingsEntityDao(d.DatabaseContexts.Context, d.DatabaseStatementLoader, d.DatabaseContexts.Implementation, d.LoggerFactory);
                return pluginSettingsEntityDao.SelectExistsPluginSetting(PluginId, NormalizeKey(key));
            });
        }

        /// <inheritdoc cref="IPluginPersistenceStorage.TryGet{TValue}(string, out TValue)"/>
        public bool TryGet<TValue>(string key, [MaybeNullWhen(returnValue: false)] out TValue value)
        {
            return TryGetImpl(key, (p, d) => {
                var pluginSettingsEntityDao = new PluginSettingsEntityDao(d.DatabaseContexts.Context, d.DatabaseStatementLoader, d.DatabaseContexts.Implementation, d.LoggerFactory);
                return pluginSettingsEntityDao.SelectPluginSettingValue(PluginId, NormalizeKey(key));
            }, out value);
        }

        /// <inheritdoc cref="IPluginPersistenceStorage.Set{TValue}(string, TValue, PluginPersistenceFormat)"/>
        public bool Set<TValue>(string key, TValue value, PluginPersistenceFormat format)
            where TValue : notnull
        {
            return SetImpl(value, format, key, (p, d, v) => {
                var pluginSettingsEntityDao = new PluginSettingsEntityDao(d.DatabaseContexts.Context, d.DatabaseStatementLoader, d.DatabaseContexts.Implementation, d.LoggerFactory);
                var normalizedKey = NormalizeKey(p);
                if(pluginSettingsEntityDao.SelectExistsPluginSetting(PluginId, normalizedKey)) {
                    pluginSettingsEntityDao.UpdatePluginSetting(PluginId, normalizedKey, v, DatabaseCommonStatus.CreatePluginAccount(PluginIdentifiers, PluginVersions));
                } else {
                    pluginSettingsEntityDao.InsertPluginSetting(PluginId, normalizedKey, v, DatabaseCommonStatus.CreatePluginAccount(PluginIdentifiers, PluginVersions));
                }
            });
        }
        /// <inheritdoc cref="IPluginPersistenceStorage.Set{TValue}(string, TValue)"/>
        public bool Set<TValue>(string key, TValue value)
            where TValue : notnull
        {
            return Set(key, value, PluginPersistenceFormat.Json);
        }

        /// <inheritdoc cref="IPluginPersistenceStorage.Delete(string)"/>
        public bool Delete(string key)
        {
            return DeleteImpl(key, (p, d) => {
                var pluginSettingsEntityDao = new PluginSettingsEntityDao(d.DatabaseContexts.Context, d.DatabaseStatementLoader, d.DatabaseContexts.Implementation, d.LoggerFactory);
                return pluginSettingsEntityDao.DeletePluginSetting(PluginId, NormalizeKey(key));
            });
        }

        #endregion
    }

    /// <inheritdoc cref="IPluginFiles"/>
    public class PluginFiles: IPluginFiles
    {
        public PluginFiles(PluginFileStorage user, PluginFileStorage machine, PluginFileStorage temporary)
        {
            User = user;
            Machine = machine;
            Temporary = temporary;
        }

        #region IPluginFile

        /// <inheritdoc cref="IPluginFiles.User"/>
        public PluginFileStorage User { get; }
        IPluginFileStorage IPluginFiles.User => User;
        /// <inheritdoc cref="IPluginFiles.Machine"/>
        public PluginFileStorage Machine { get; }
        IPluginFileStorage IPluginFiles.Machine => Machine;
        /// <inheritdoc cref="IPluginFiles.Temporary"/>
        public PluginFileStorage Temporary { get; }
        IPluginFileStorage IPluginFiles.Temporary => Temporary;

        #endregion
    }

    /// <inheritdoc cref="IPluginPersistence"/>
    public class PluginPersistence: IPluginPersistence
    {
        public PluginPersistence(PluginPersistenceStorage normal, PluginPersistenceStorage large, PluginPersistenceStorage temporary)
        {
            Normal = normal;
            Large = large;
            Temporary = temporary;
        }

        #region IPluginPersistence

        /// <inheritdoc cref="IPluginPersistence.Normal"/>
        public PluginPersistenceStorage Normal { get; }
        IPluginPersistenceStorage IPluginPersistence.Normal => Normal;
        /// <inheritdoc cref="IPluginPersistence.Large"/>
        public PluginPersistenceStorage Large { get; }
        IPluginPersistenceStorage IPluginPersistence.Large => Large;
        /// <inheritdoc cref="IPluginPersistence.Temporary"/>
        public PluginPersistenceStorage Temporary { get; }
        IPluginPersistenceStorage IPluginPersistence.Temporary => Temporary;

        #endregion
    }

    /// <inheritdoc cref="IPluginStorage"/>
    public class PluginStorage: IPluginStorage
    {
        public PluginStorage(PluginFiles file, PluginPersistence persistence)
        {
            File = file;
            Persistence = persistence;
        }

        #region IPluginStorage

        /// <inheritdoc cref="IPluginStorage.File"/>
        public PluginFiles File { get; }
        IPluginFiles IPluginStorage.File => File;
        /// <inheritdoc cref="IPluginStorage.Persistence"/>
        public PluginPersistence Persistence { get; }
        IPluginPersistence IPluginStorage.Persistence => Persistence;

        #endregion
    }
}

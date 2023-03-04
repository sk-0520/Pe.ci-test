using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Standard.Base;
using ContentTypeTextNet.Pe.Standard.Database;
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

        protected string TuneFileName(string name)
        {
            if(name == null) {
                throw new ArgumentNullException(nameof(name));
            }

            if(string.IsNullOrWhiteSpace(name)) {
                throw new ArgumentException(nameof(name));
            }

            var s = name.Trim();
            var cs = Path.GetInvalidFileNameChars();
            if(s.Any(i => cs.Any(cc => cc == i))) {
                throw new ArgumentException(nameof(name));
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

            var tunedFileName = TuneFileName(fileName);
            var path = CombinePath(directoryName, tunedFileName);

            return File.Exists(path);
        }

        protected void Rename(string directoryName, string sourceName, string destinationName, bool overwrite)
        {
            Debug.Assert(directoryName != null);

            if(sourceName == destinationName) {
                throw new ArgumentException($"{nameof(sourceName)} == {nameof(destinationName)}");
            }

            var tunedSourceFileName = TuneFileName(sourceName);
            var tunedDestinationFileName = TuneFileName(destinationName);

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

            var tunedSourceFileName = TuneFileName(sourceName);
            var tunedDestinationFileName = TuneFileName(destinationName);

            var tunedSourceFilePath = CombinePath(directoryName, tunedSourceFileName);
            var tunedDestinationFilePath = CombinePath(directoryName, tunedDestinationFileName);

            File.Copy(tunedSourceFilePath, tunedDestinationFilePath, overwrite);
        }

        protected void Delete(string directoryName, string name)
        {
            Debug.Assert(directoryName != null);

            var tunedFileName = TuneFileName(name);
            var path = CombinePath(directoryName, tunedFileName);
            File.Delete(path);
        }

        protected Stream Open(string directoryName, string name, FileMode fileMode)
        {
            Debug.Assert(directoryName != null);

            var tunedFileName = TuneFileName(name);
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

    public enum PluginPersistentMode
    {
        /// <summary>
        /// 上流に影響される読み書き。
        /// <para>書き込みは状況により不可。</para>
        /// </summary>
        Context,
        /// <summary>
        /// 単独実施する読み書き。
        /// <para>書き込みは状況により不可。</para>
        /// </summary>
        Barrier,
        /// <summary>
        /// 遅延書き込み。
        /// </summary>
        LazyWriter,
    }

    public abstract class PluginPersistentStorageBase: IPluginId
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
        /// <para>読み込み専用・書き込み可能に分かれる。書き込み専用は基本的に設定画面くらい。</para>
        /// </summary>
        /// <param name="pluginIdentifiers"></param>
        /// <param name="pluginVersions"></param>
        /// <param name="databaseContexts"></param>
        /// <param name="databaseStatementLoader"></param>
        /// <param name="isReadOnly">読み込み専用か。</param>
        /// <param name="loggerFactory"></param>
        protected PluginPersistentStorageBase(IPluginIdentifiers pluginIdentifiers, IPluginVersions pluginVersions, IDatabaseContexts databaseContexts, IDatabaseStatementLoader databaseStatementLoader, bool isReadOnly, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            PluginIdentifiers = pluginIdentifiers;
            PluginVersions = pluginVersions;
            DatabaseContext = databaseContexts.Context;
            DatabaseImplementation = databaseContexts.Implementation;
            IsReadOnly = isReadOnly;
            DatabaseStatementLoader = databaseStatementLoader;
            Mode = PluginPersistentMode.Context;
        }

        /// <summary>
        /// プラグイン用DB操作処理構築。
        /// <para>読み込み専用・書き込み可能に分かれる。逐次処理される。</para>
        /// </summary>
        /// <param name="pluginIdentifiers"></param>
        /// <param name="pluginVersions"></param>
        /// <param name="databaseBarrier"></param>
        /// <param name="databaseStatementLoader"></param>
        /// <param name="isReadOnly">読み込み専用か。</param>
        /// <param name="loggerFactory"></param>
        protected PluginPersistentStorageBase(IPluginIdentifiers pluginIdentifiers, IPluginVersions pluginVersions, IDatabaseBarrier databaseBarrier, IDatabaseStatementLoader databaseStatementLoader, bool isReadOnly, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            PluginIdentifiers = pluginIdentifiers;
            PluginVersions = pluginVersions;
            DatabaseBarrier = databaseBarrier;
            IsReadOnly = isReadOnly;
            DatabaseStatementLoader = databaseStatementLoader;
            Mode = PluginPersistentMode.Barrier;
        }

        /// <summary>
        /// プラグイン用DB操作処理構築。
        /// <para>通常実行時の書き込み処理で、遅延処理される。</para>
        /// </summary>
        /// <param name="pluginIdentifiers"></param>
        /// <param name="pluginVersions"></param>
        /// <param name="databaseBarrier"></param>
        /// <param name="databaseLazyWriter"></param>
        /// <param name="databaseStatementLoader"></param>
        /// <param name="loggerFactory"></param>
        protected PluginPersistentStorageBase(IPluginIdentifiers pluginIdentifiers, IPluginVersions pluginVersions, IDatabaseBarrier databaseBarrier, IDatabaseLazyWriter databaseLazyWriter, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            PluginIdentifiers = pluginIdentifiers;
            PluginVersions = pluginVersions;
            DatabaseBarrier = databaseBarrier;
            DatabaseLazyWriter = databaseLazyWriter;
            DatabaseStatementLoader = databaseStatementLoader;
            IsReadOnly = false;
            Mode = PluginPersistentMode.LazyWriter;
        }

        #region property

        /// <inheritdoc cref="ILoggerFactory"/>
        protected ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        protected ILogger Logger { get; }

        protected IPluginIdentifiers PluginIdentifiers { get; }
        public string PluginName => PluginIdentifiers.PluginName;
        protected IPluginVersions PluginVersions { get; }

        protected PluginPersistentMode Mode { get; }
        protected IDatabaseImplementation? DatabaseImplementation { get; }
        protected IDatabaseContext? DatabaseContext { get; }
        protected IDatabaseBarrier? DatabaseBarrier { get; }
        protected IDatabaseLazyWriter? DatabaseLazyWriter { get; }
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

        protected bool ExistsImpl<TParameter>(TParameter parameter, Func<TParameter, DatabaseParameter, bool> func)
        {
            switch(Mode) {
                case PluginPersistentMode.Context: {
                        Debug.Assert(DatabaseContext != null);
                        Debug.Assert(DatabaseImplementation != null);

                        return func(parameter, new DatabaseParameter(DatabaseStatementLoader, new DatabaseContexts(DatabaseContext, DatabaseImplementation), LoggerFactory));
                    }

                case PluginPersistentMode.Barrier:
                case PluginPersistentMode.LazyWriter: {
                        Debug.Assert(DatabaseBarrier != null);

                        if(Mode == PluginPersistentMode.LazyWriter) {
                            Debug.Assert(DatabaseLazyWriter != null);
                            DatabaseLazyWriter.Flush();
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
            if(Mode == PluginPersistentMode.LazyWriter) {
                // 遅延書き込み待機を終了
                Debug.Assert(DatabaseLazyWriter != null);
                DatabaseLazyWriter.Flush();
            }

            PluginSettingRawValue? data;
            switch(Mode) {
                case PluginPersistentMode.Context: {
                        Debug.Assert(DatabaseContext != null);
                        Debug.Assert(DatabaseImplementation != null);

                        data = func(parameter, new DatabaseParameter(DatabaseStatementLoader, new DatabaseContexts(DatabaseContext, DatabaseImplementation), LoggerFactory));
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
                case PluginPersistentFormat.SimpleXml:
                case PluginPersistentFormat.DataXml: {
                        SerializerBase serializer = data.Format switch {
                            PluginPersistentFormat.SimpleXml => new XmlSerializer(),
                            PluginPersistentFormat.DataXml => new XmlDataContractSerializer(),
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

                case PluginPersistentFormat.Json: {
                        try {
                            value = JsonSerializer.Deserialize<TValue>(data.Value)!;
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

        protected bool SetImpl<TValue, TParameter>(TValue value, PluginPersistentFormat format, TParameter parameter, Action<TParameter, DatabaseParameter, PluginSettingRawValue> action)
        {
            if(IsReadOnly) {
                throw new InvalidOperationException(nameof(IsReadOnly));
            }

            if(value == null) {
                Logger.LogWarning($"{nameof(value)} は null のため処理終了");
                return false;
            }

            string textValue;

            switch(format) {
                case PluginPersistentFormat.SimpleXml:
                case PluginPersistentFormat.DataXml: {
                        SerializerBase serializer = format switch {
                            PluginPersistentFormat.SimpleXml => new XmlSerializer(),
                            PluginPersistentFormat.DataXml => new XmlDataContractSerializer(),
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
                            Logger.LogWarning($"文字列であるべきデータ: {nameof(value)} -> {typeof(TValue)}");
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
                case PluginPersistentMode.Context: {
                        Debug.Assert(DatabaseContext != null);
                        Debug.Assert(DatabaseImplementation != null);

                        action(parameter, new DatabaseParameter(DatabaseStatementLoader, new DatabaseContexts(DatabaseContext, DatabaseImplementation), LoggerFactory), data);
                    }
                    break;

                case PluginPersistentMode.Barrier: {
                        Debug.Assert(DatabaseBarrier != null);

                        using(var context = DatabaseBarrier.WaitWrite()) {
                            action(parameter, new DatabaseParameter(DatabaseStatementLoader, new DatabaseContexts(context, context.Implementation), LoggerFactory), data);
                            context.Commit();
                        }
                    }
                    break;

                case PluginPersistentMode.LazyWriter: {
                        Debug.Assert(DatabaseLazyWriter != null);

                        DatabaseLazyWriter.Stock(c => {
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

            if(Mode == PluginPersistentMode.LazyWriter) {
                // 遅延書き込み待機を終了
                Debug.Assert(DatabaseLazyWriter != null);
                DatabaseLazyWriter.Flush();
            }

            switch(Mode) {
                case PluginPersistentMode.Context: {
                        Debug.Assert(DatabaseContext != null);
                        Debug.Assert(DatabaseImplementation != null);

                        return func(parameter, new DatabaseParameter(DatabaseStatementLoader, new DatabaseContexts(DatabaseContext, DatabaseImplementation), LoggerFactory));
                    }

                case PluginPersistentMode.Barrier: {
                        Debug.Assert(DatabaseBarrier != null);

                        using(var context = DatabaseBarrier.WaitWrite()) {
                            var result = func(parameter, new DatabaseParameter(DatabaseStatementLoader, new DatabaseContexts(context, context.Implementation), LoggerFactory));
                            context.Commit();
                            return result;
                        }
                    }

                case PluginPersistentMode.LazyWriter: {
                        Debug.Assert(DatabaseLazyWriter != null);

                        DatabaseLazyWriter.Stock(c => {
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

    /// <inheritdoc cref="IPluginPersistentStorage"/>
    public sealed class PluginPersistentStorage: PluginPersistentStorageBase, IPluginPersistentStorage
    {
        /// <inheritdoc cref="PluginPersistentStorageBase.PluginPersistentStorageBase(IPluginIdentifiers, IPluginVersions, IDatabaseContexts, IDatabaseStatementLoader, bool, ILoggerFactory)"/>
        public PluginPersistentStorage(IPluginIdentifiers pluginIdentifiers, IPluginVersions pluginVersions, IDatabaseContexts databaseContexts, IDatabaseStatementLoader databaseStatementLoader, bool isReadOnly, ILoggerFactory loggerFactory)
            : base(pluginIdentifiers, pluginVersions, databaseContexts, databaseStatementLoader, isReadOnly, loggerFactory)
        { }

        /// <inheritdoc cref="PluginPersistentStorageBase.PluginPersistentStorageBase(IPluginIdentifiers, IPluginVersions, IDatabaseBarrier, IDatabaseStatementLoader, bool, ILoggerFactory)"/>
        public PluginPersistentStorage(IPluginIdentifiers pluginIdentifiers, IPluginVersions pluginVersions, IDatabaseBarrier databaseBarrier, IDatabaseStatementLoader databaseStatementLoader, bool isReadOnly, ILoggerFactory loggerFactory)
            : base(pluginIdentifiers, pluginVersions, databaseBarrier, databaseStatementLoader, isReadOnly, loggerFactory)
        { }

        /// <inheritdoc cref="PluginPersistentStorageBase.PluginPersistentStorageBase(IPluginIdentifiers, IPluginVersions, IDatabaseBarrier, IDatabaseLazyWriter, IDatabaseStatementLoader, ILoggerFactory)"/>
        public PluginPersistentStorage(IPluginIdentifiers pluginIdentifiers, IPluginVersions pluginVersions, IDatabaseBarrier databaseBarrier, IDatabaseLazyWriter databaseLazyWriter, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
            : base(pluginIdentifiers, pluginVersions, databaseBarrier, databaseLazyWriter, databaseStatementLoader, loggerFactory)
        { }

        #region property


        #endregion

        #region function

        #endregion

        #region IPluginPersistentStorage

        /// <inheritdoc cref="IPluginPersistentStorage.Exists(string)"/>
        public bool Exists(string key)
        {
            return ExistsImpl(key, (p, d) => {
                var pluginSettingsEntityDao = new PluginSettingsEntityDao(d.DatabaseContexts.Context, d.DatabaseStatementLoader, d.DatabaseContexts.Implementation, d.LoggerFactory);
                return pluginSettingsEntityDao.SelecteExistsPluginSetting(PluginId, NormalizeKey(key));
            });
        }

        /// <inheritdoc cref="IPluginPersistentStorage.TryGet{TValue}(string, out TValue)"/>
        public bool TryGet<TValue>(string key, [MaybeNullWhen(returnValue: false)] out TValue value)
        {
            return TryGetImpl(key, (p, d) => {
                var pluginSettingsEntityDao = new PluginSettingsEntityDao(d.DatabaseContexts.Context, d.DatabaseStatementLoader, d.DatabaseContexts.Implementation, d.LoggerFactory);
                return pluginSettingsEntityDao.SelectPluginSettingValue(PluginId, NormalizeKey(key));
            }, out value);
        }

        /// <inheritdoc cref="IPluginPersistentStorage.Set{TValue}(string, TValue, PluginPersistentFormat)"/>
        public bool Set<TValue>(string key, TValue value, PluginPersistentFormat format)
        {
            return SetImpl(value, format, key, (p, d, v) => {
                var pluginSettingsEntityDao = new PluginSettingsEntityDao(d.DatabaseContexts.Context, d.DatabaseStatementLoader, d.DatabaseContexts.Implementation, d.LoggerFactory);
                var normalizedKey = NormalizeKey(p);
                if(pluginSettingsEntityDao.SelecteExistsPluginSetting(PluginId, normalizedKey)) {
                    pluginSettingsEntityDao.UpdatePluginSetting(PluginId, normalizedKey, v, DatabaseCommonStatus.CreatePluginAccount(PluginIdentifiers, PluginVersions));
                } else {
                    pluginSettingsEntityDao.InsertPluginSetting(PluginId, normalizedKey, v, DatabaseCommonStatus.CreatePluginAccount(PluginIdentifiers, PluginVersions));
                }
            });
        }
        /// <inheritdoc cref="IPluginPersistentStorage.Set{TValue}(string, TValue)"/>
        public bool Set<TValue>(string key, TValue value) => Set(key, value, PluginPersistentFormat.Json);

        /// <inheritdoc cref="IPluginPersistentStorage.Delete(string)"/>
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

    /// <inheritdoc cref="IPluginPersistents"/>
    public class PluginPersistents: IPluginPersistents
    {
        public PluginPersistents(PluginPersistentStorage normal, PluginPersistentStorage large, PluginPersistentStorage temporary)
        {
            Normal = normal;
            Large = large;
            Temporary = temporary;
        }

        #region IPluginPersistent

        /// <inheritdoc cref="IPluginPersistents.Normal"/>
        public PluginPersistentStorage Normal { get; }
        IPluginPersistentStorage IPluginPersistents.Normal => Normal;
        /// <inheritdoc cref="IPluginPersistents.Large"/>
        public PluginPersistentStorage Large { get; }
        IPluginPersistentStorage IPluginPersistents.Large => Large;
        /// <inheritdoc cref="IPluginPersistents.Temporary"/>
        public PluginPersistentStorage Temporary { get; }
        IPluginPersistentStorage IPluginPersistents.Temporary => Temporary;

        #endregion
    }

    /// <inheritdoc cref="IPluginStorage"/>
    public class PluginStorage: IPluginStorage
    {
        public PluginStorage(PluginFiles file, PluginPersistents persistent)
        {
            File = file;
            Persistent = persistent;
        }

        #region IPluginStorage

        /// <inheritdoc cref="IPluginStorage.File"/>
        public PluginFiles File { get; }
        IPluginFiles IPluginStorage.File => File;
        /// <inheritdoc cref="IPluginStorage.Persistent"/>
        public PluginPersistents Persistent { get; }
        IPluginPersistents IPluginStorage.Persistent => Persistent;

        #endregion
    }
}

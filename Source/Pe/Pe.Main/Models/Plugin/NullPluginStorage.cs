using System.Diagnostics.CodeAnalysis;
using System.IO;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    /// <summary>
    /// すべてなかったことにする <see cref="IPluginFileStorage"/>。
    /// </summary>
    internal sealed class NullPluginFileStorage: IPluginFileStorage
    {
        public NullPluginFileStorage(IPluginIdentifiers pluginIdentifiers, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            PluginIdentifiers = pluginIdentifiers;
        }

        #region property

        ILogger Logger { get; }
        IPluginIdentifiers PluginIdentifiers { get; }

        #endregion

        #region IPluginFileStorage

        public void Copy(string sourceName, string destinationName, bool overwrite)
        {
            Logger.LogTrace("{0}({1}): {2} = {3}, {4} = {5}, {6} = {7}", PluginIdentifiers.PluginName, PluginIdentifiers.PluginId, nameof(sourceName), sourceName, nameof(destinationName), destinationName, nameof(overwrite), overwrite);
        }

        public void Delete(string name)
        {
            Logger.LogTrace("{0}({1}): {2} = {3}", PluginIdentifiers.PluginName, PluginIdentifiers.PluginId, nameof(name), name);
        }

        public bool Exists(string name)
        {
            Logger.LogTrace("{0}({1}): {2} = {3}", PluginIdentifiers.PluginName, PluginIdentifiers.PluginId, nameof(name), name);
            return true;
        }

        public Stream Open(string name, FileMode fileMode)
        {
            Logger.LogTrace("{0}({1}): {2} = {3}, {4} = {5}", PluginIdentifiers.PluginName, PluginIdentifiers.PluginId, nameof(name), name, nameof(fileMode), fileMode);
            return Stream.Null;
        }

        public void Rename(string sourceName, string destinationName, bool overwrite)
        {
            Logger.LogTrace("{0}({1}): {2} = {3}, {4} = {5}, {6} = {7}", PluginIdentifiers.PluginName, PluginIdentifiers.PluginId, nameof(sourceName), sourceName, nameof(destinationName), destinationName, nameof(overwrite), overwrite);
        }

        #endregion
    }

    internal sealed class NullPluginPersistentStorage: IPluginPersistentStorage
    {
        public NullPluginPersistentStorage(IPluginIdentifiers pluginIdentifiers, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            PluginIdentifiers = pluginIdentifiers;
        }

        #region property

        ILogger Logger { get; }
        IPluginIdentifiers PluginIdentifiers { get; }

        #endregion

        #region IPluginPersistentStorage

        public bool IsReadOnly => true;

        public bool Delete(string key)
        {
            Logger.LogTrace("{0}({1}): {2} = {3}", PluginIdentifiers.PluginName, PluginIdentifiers.PluginId, nameof(key), key);
            return true;
        }

        public bool Exists(string key)
        {
            Logger.LogTrace("{0}({1}): {2} = {3}", PluginIdentifiers.PluginName, PluginIdentifiers.PluginId, nameof(key), key);
            return true;
        }

        public bool Set<TValue>(string key, TValue value, PluginPersistentFormat format)
        {
            Logger.LogTrace("{0}({1}): {2} = {3}, {4} = {5}, {6} = {7}", PluginIdentifiers.PluginName, PluginIdentifiers.PluginId, nameof(key), key, nameof(value), value, nameof(format), format);
            return true;
        }

        public bool Set<TValue>(string key, TValue value) => Set(key, value, PluginPersistentFormat.Text);

        public bool TryGet<TValue>(string key, [MaybeNullWhen(returnValue: false)] out TValue value)
        {
            Logger.LogTrace("{0}({1}): {2} = {3}", PluginIdentifiers.PluginName, PluginIdentifiers.PluginId, nameof(key), key);
            value = default;
            return false;
        }

        #endregion
    }

    internal sealed class NullPluginFiles: IPluginFiles
    {
        public NullPluginFiles(IPluginIdentifiers pluginIdentifiers, ILoggerFactory loggerFactory)
        {
            var nullStorage = new NullPluginFileStorage(pluginIdentifiers, loggerFactory);
            User = nullStorage;
            Machine = nullStorage;
            Temporary = nullStorage;
        }

        #region IPluginFiles

        public IPluginFileStorage User { get; }

        public IPluginFileStorage Machine { get; }

        public IPluginFileStorage Temporary { get; }

        #endregion
    }

    internal sealed class NullPluginPersistents: IPluginPersistents
    {
        public NullPluginPersistents(IPluginIdentifiers pluginIdentifiers, ILoggerFactory loggerFactory)
        {
            var nullStorage = new NullPluginPersistentStorage(pluginIdentifiers, loggerFactory);
            Normal = nullStorage;
            Large = nullStorage;
            Temporary = nullStorage;
        }

        #region IPluginPersistents

        public IPluginPersistentStorage Normal { get; }

        public IPluginPersistentStorage Large { get; }

        public IPluginPersistentStorage Temporary { get; }

        #endregion
    }

    internal sealed class NullPluginStorage: IPluginStorage
    {
        public NullPluginStorage(IPluginIdentifiers pluginIdentifiers, ILoggerFactory loggerFactory)
        {
            File = new NullPluginFiles(pluginIdentifiers, loggerFactory);
            Persistent = new NullPluginPersistents(pluginIdentifiers, loggerFactory);
        }

        #region IPluginStorage

        public IPluginFiles File { get; }

        public IPluginPersistents Persistent { get; }

        #endregion
    }
}

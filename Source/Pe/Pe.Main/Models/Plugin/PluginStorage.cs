using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    /// <inheritdoc cref="IPluginFileStorage"/>
    public class PluginFileStorage: IPluginFileStorage
    {
        public PluginFileStorage(DirectoryInfo directory)
        {
            DirectoryInfo = directory;
        }

        #region property

        DirectoryInfo DirectoryInfo { get; }

        #endregion

        #region function

        private string TuneFileName(string name)
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

        #endregion

        #region IPluginFileStorage

        /// <inheritdoc cref="IPluginFileStorage.Exists(string)"/>
        public bool Exists(string name)
        {
            var tunedFileName = TuneFileName(name);
            var path = Path.Combine(DirectoryInfo.FullName, tunedFileName);
            return File.Exists(path);
        }

        /// <inheritdoc cref="IPluginFileStorage.Rename(string, string, bool)"/>
        public void Rename(string sourceName, string destinationName, bool overwrite)
        {
            if(sourceName == destinationName) {
                throw new ArgumentException($"{nameof(sourceName)} == {nameof(destinationName)}");
            }

            var tunedSourceFileName = TuneFileName(sourceName);
            var tunedDestinationFileName = TuneFileName(destinationName);

            var tunedSourceFilePath = Path.Combine(DirectoryInfo.FullName, tunedSourceFileName);
            var tunedDestinationFilePath = Path.Combine(DirectoryInfo.FullName, tunedDestinationFileName);

            File.Move(tunedSourceFilePath, tunedDestinationFilePath, overwrite);
        }

        /// <inheritdoc cref="IPluginFileStorage.Copy(string, string, bool)"/>
        public void Copy(string sourceName, string destinationName, bool overwrite)
        {
            if(sourceName == destinationName) {
                throw new ArgumentException($"{nameof(sourceName)} == {nameof(destinationName)}");
            }

            var tunedSourceFileName = TuneFileName(sourceName);
            var tunedDestinationFileName = TuneFileName(destinationName);

            var tunedSourceFilePath = Path.Combine(DirectoryInfo.FullName, tunedSourceFileName);
            var tunedDestinationFilePath = Path.Combine(DirectoryInfo.FullName, tunedDestinationFileName);

            File.Copy(tunedSourceFilePath, tunedDestinationFilePath, overwrite);
        }

        /// <inheritdoc cref="IPluginFileStorage.Delete(string)"/>
        public void Delete(string name)
        {
            var tunedFileName = TuneFileName(name);
            var path = Path.Combine(DirectoryInfo.FullName, tunedFileName);
            File.Delete(path);
        }

        /// <inheritdoc cref="IPluginFileStorage.Open(string, FileMode)"/>
        public Stream Open(string name, FileMode fileMode)
        {
            var tunedFileName = TuneFileName(name);
            var path = Path.Combine(DirectoryInfo.FullName, tunedFileName);
            var stream = new FileStream(path, fileMode, FileAccess.ReadWrite, FileShare.Read);
            return stream;
        }

        #endregion
    }

    public class PluginPersistentStorage: IPluginPersistentStorage
    {
        #region IPluginPersistentStorage

        #endregion
    }

    /// <inheritdoc cref="IPluginFiles"/>
    public class PluginFile: IPluginFiles
    {
        public PluginFile(PluginFileStorage user, PluginFileStorage machine, PluginFileStorage temporary)
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
    public class PluginPersistent: IPluginPersistents
    {
        public PluginPersistent(PluginPersistentStorage normal, PluginPersistentStorage large, PluginPersistentStorage temporary)
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
        public PluginStorage(PluginFile file, PluginPersistent persistent)
        {
            File = file;
            Persistent = persistent;
        }

        #region IPluginStorage

        /// <inheritdoc cref="IPluginStorage.File"/>
        public PluginFile File { get; }
        IPluginFiles IPluginStorage.File => File;
        /// <inheritdoc cref="IPluginStorage.Persistent"/>
        public PluginPersistent Persistent { get; }
        IPluginPersistents IPluginStorage.Persistent => Persistent;

        #endregion
    }
}

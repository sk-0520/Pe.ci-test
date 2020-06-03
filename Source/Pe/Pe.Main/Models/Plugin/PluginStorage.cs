using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;

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

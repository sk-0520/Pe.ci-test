using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;

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
    public class LauncherItemAddonPersistentStorage: ILauncherItemAddonPersistentStorage
    {
        #region ILauncherItemAddonPersistentStorage

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

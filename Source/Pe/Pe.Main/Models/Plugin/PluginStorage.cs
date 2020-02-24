using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    public class PluginFileStorage : IPluginFileStorage
    {
        public PluginFileStorage(DirectoryInfo directory)
        {
            DirectoryInfo = directory;
        }

        #region property

        DirectoryInfo DirectoryInfo { get; }

        #endregion

        #region IPluginFileStorage

        #endregion
    }

    public class PluginPersistentStorage : IPluginPersistentStorage
    {
        #region IPluginPersistentStorage

        #endregion
    }

    public class PluginFile : IPluginFile
    {
        public PluginFile(PluginFileStorage user, PluginFileStorage machine, PluginFileStorage temporary)
        {
            User = user;
            Machine = machine;
            Temporary = temporary;
        }

        #region IPluginFile

        public PluginFileStorage User { get; }
        IPluginFileStorage IPluginFile.User => User;
        public PluginFileStorage Machine { get; }
        IPluginFileStorage IPluginFile.Machine => Machine;
        public PluginFileStorage Temporary { get; }
        IPluginFileStorage IPluginFile.Temporary => Temporary;

        #endregion
    }

    public class PluginPersistent : IPluginPersistent
    {
        public PluginPersistent(PluginPersistentStorage normal, PluginPersistentStorage large, PluginPersistentStorage temporary)
        {
            Normal = normal;
            Large = large;
            Temporary = temporary;
        }

        #region IPluginPersistent

        public PluginPersistentStorage Normal { get; }
        IPluginPersistentStorage IPluginPersistent.Normal => Normal;
        public PluginPersistentStorage Large { get; }
        IPluginPersistentStorage IPluginPersistent.Large => Large;
        public PluginPersistentStorage Temporary { get; }
        IPluginPersistentStorage IPluginPersistent.Temporary => Temporary;

        #endregion
    }

    public class PluginStorage : IPluginStorage
    {
        public PluginStorage(PluginFile file, PluginPersistent persistent)
        {
            File = file;
            Persistent = persistent;
        }

        #region IPluginStorage

        public PluginFile File { get; }
        IPluginFile IPluginStorage.File => File;
        public PluginPersistent Persistent { get; }
        IPluginPersistent IPluginStorage.Persistent => Persistent;

        #endregion
    }
}

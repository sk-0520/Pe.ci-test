using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;

#if false

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    /// <inheritdoc cref="ILauncherItemAddonFileStorage"/>
    internal class LauncherItemAddonFileStorage: ILauncherItemAddonFileStorage
    {
#region ILauncherItemAddonFileStorage

#endregion
    }

    /// <inheritdoc cref="ILauncherItemAddonPersistentStorage"/>
    internal class LauncherItemAddonPersistentStorage: ILauncherItemAddonPersistentStorage
    {
#region ILauncherItemAddonPersistentStorage

#endregion
    }

    /// <inheritdoc cref="ILauncherItemAddonFiles"/>
    internal class LauncherItemAddonFiles:  ILauncherItemAddonFiles
    {
#region ILauncherItemAddonFiles

#endregion
    }

    /// <inheritdoc cref="ILauncherItemAddonPersistents"/>
    internal class LauncherItemAddonPersistents: ILauncherItemAddonPersistents
    {
#region ILauncherItemAddonPersistents

#endregion
    }

    /// <inheritdoc cref="ILauncherItemAddonStorage"/>
    internal class LauncherItemAddonStorage: ILauncherItemAddonStorage
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

#endif

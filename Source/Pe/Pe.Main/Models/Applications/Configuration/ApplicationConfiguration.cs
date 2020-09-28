using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    public class ApplicationConfiguration: ConfigurationBase
    {
        public ApplicationConfiguration(IConfigurationRoot configurationRoot)
            : base(configurationRoot)
        { }

        #region property

        [Configuration]
        public GeneralConfiguration General { get; } = default!;
        [Configuration]
        public WebConfiguration Web { get; } = default!;
        [Configuration]
        public ApiConfiguration Api { get; } = default!;
        [Configuration]
        public BackupConfiguration Backup { get; } = default!;
        [Configuration]
        public FileConfiguration File { get; } = default!;
        [Configuration]
        public DisplayConfiguration Display { get; } = default!;
        [Configuration]
        public HookConfiguration Hook { get; } = default!;
        [Configuration]
        public NotifyLogConfiguration NotifyLog { get; } = default!;
        [Configuration]
        public LauncherItemConfiguration LauncherItem { get; } = default!;
        [Configuration]
        public LauncherToolbarConfiguration LauncherToolbar { get; } = default!;
        [Configuration]
        public LauncherGroupConfiguration LauncherGroup { get; } = default!;
        [Configuration]
        public NoteConfiguration Note { get; } = default!;
        [Configuration]
        public CommandConfiguration Command { get; } = default!;
        [Configuration]
        public PlatformConfiguration Platform { get; } = default!;
        [Configuration]
        public ScheduleConfiguration Schedule { get; } = default!;
        [Configuration]
        public PluginConfiguration Plugin { get; } = default!;
        #endregion
    }
}

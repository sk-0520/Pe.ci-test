using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    /// <summary>
    /// アプリケーション構成。
    /// </summary>
    public class ApplicationConfiguration: ConfigurationBase
    {
        public ApplicationConfiguration(IConfigurationRoot configurationRoot)
            : base(configurationRoot)
        { }

        #region property

        /// <inheritdoc cref="GeneralConfiguration"/>
        [Configuration]
        public GeneralConfiguration General { get; } = default!;
        /// <inheritdoc cref="WebConfiguration"/>
        [Configuration]
        public WebConfiguration Web { get; } = default!;
        /// <inheritdoc cref="ApiConfiguration"/>
        [Configuration]
        public ApiConfiguration Api { get; } = default!;
        /// <inheritdoc cref="BackupConfiguration"/>
        [Configuration]
        public BackupConfiguration Backup { get; } = default!;
        /// <inheritdoc cref="FileConfiguration"/>
        [Configuration]
        public FileConfiguration File { get; } = default!;
        /// <inheritdoc cref="DisplayConfiguration"/>
        [Configuration]
        public DisplayConfiguration Display { get; } = default!;
        /// <inheritdoc cref="HookConfiguration"/>
        [Configuration]
        public HookConfiguration Hook { get; } = default!;
        /// <inheritdoc cref="NotifyLogConfiguration"/>
        [Configuration]
        public NotifyLogConfiguration NotifyLog { get; } = default!;
        /// <inheritdoc cref="LauncherItemConfiguration"/>
        [Configuration]
        public LauncherItemConfiguration LauncherItem { get; } = default!;
        /// <inheritdoc cref="LauncherToolbarConfiguration"/>
        [Configuration]
        public LauncherToolbarConfiguration LauncherToolbar { get; } = default!;
        /// <inheritdoc cref="LauncherGroupConfiguration"/>
        [Configuration]
        public LauncherGroupConfiguration LauncherGroup { get; } = default!;
        /// <inheritdoc cref="NoteConfiguration"/>
        [Configuration]
        public NoteConfiguration Note { get; } = default!;
        /// <inheritdoc cref="CommandConfiguration"/>
        [Configuration]
        public CommandConfiguration Command { get; } = default!;
        /// <inheritdoc cref="PlatformConfiguration"/>
        [Configuration]
        public PlatformConfiguration Platform { get; } = default!;
        /// <inheritdoc cref="ScheduleConfiguration"/>
        [Configuration]
        public ScheduleConfiguration Schedule { get; } = default!;
        /// <inheritdoc cref="PluginConfiguration"/>
        [Configuration]
        public PluginConfiguration Plugin { get; } = default!;
        #endregion

        #region ConfigurationBase

        public override string ToString()
        {
            var conf = (IConfigurationRoot)Configuration;
            return conf.GetDebugView();
        }

        #endregion
    }
}

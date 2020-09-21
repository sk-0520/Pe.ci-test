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
        {
            //General = new GeneralConfiguration(configurationRoot.GetSection("general"));
            //Web = new WebConfiguration(configurationRoot.GetSection("web"));
            //Api = new ApiConfiguration(configurationRoot.GetSection("api"));
            //Backup = new BackupConfiguration(configurationRoot.GetSection("backup"));
            //File = new FileConfiguration(configurationRoot.GetSection("file"));
            //Display = new DisplayConfiguration(configurationRoot.GetSection("display"));
            //Hook = new HookConfiguration(configurationRoot.GetSection("hook"));
            //NotifyLog = new NotifyLogConfiguration(configurationRoot.GetSection("notify_log"));
            //LauncherItem = new LauncherItemConfiguration(configurationRoot.GetSection("launcher_item"));
            //LauncherToobar = new LauncherToolbarConfiguration(configurationRoot.GetSection("launcher_toolbar"));
            //LauncherGroup = new LauncherGroupConfiguration(configurationRoot.GetSection("launcher_group"));
            //Note = new NoteConfiguration(configurationRoot.GetSection("note"));
            //Command = new CommandConfiguration(configurationRoot.GetSection("command"));
            //Platform = new PlatformConfiguration(configurationRoot.GetSection("platform"));
            //Schedule = new ScheduleConfiguration(configurationRoot.GetSection("schedule"));
            Plugin = new PluginConfiguration(configurationRoot.GetSection("plugin"));
        }

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
        public PluginConfiguration Plugin { get; }
        #endregion
    }
}

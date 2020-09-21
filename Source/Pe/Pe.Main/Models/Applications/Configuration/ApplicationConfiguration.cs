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
            File = new FileConfiguration(configurationRoot.GetSection("file"));
            Display = new DisplayConfiguration(configurationRoot.GetSection("display"));
            Hook = new HookConfiguration(configurationRoot.GetSection("hook"));
            NotifyLog = new NotifyLogConfiguration(configurationRoot.GetSection("notify_log"));
            LauncherItem = new LauncherItemConfiguration(configurationRoot.GetSection("launcher_item"));
            LauncherToobar = new LauncherToolbarConfiguration(configurationRoot.GetSection("launcher_toolbar"));
            LauncherGroup = new LauncherGroupConfiguration(configurationRoot.GetSection("launcher_group"));
            Note = new NoteConfiguration(configurationRoot.GetSection("note"));
            Command = new CommandConfiguration(configurationRoot.GetSection("command"));
            Platform = new PlatformConfiguration(configurationRoot.GetSection("platform"));
            Schedule = new ScheduleConfiguration(configurationRoot.GetSection("schedule"));
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
        public FileConfiguration File { get; }
        public DisplayConfiguration Display { get; }
        public HookConfiguration Hook { get; }
        public NotifyLogConfiguration NotifyLog { get; }
        public LauncherItemConfiguration LauncherItem { get; }
        public LauncherToolbarConfiguration LauncherToobar { get; }
        public LauncherGroupConfiguration LauncherGroup { get; }
        public NoteConfiguration Note { get; }
        public CommandConfiguration Command { get; }
        public PlatformConfiguration Platform { get; }
        public ScheduleConfiguration Schedule { get; }
        public PluginConfiguration Plugin { get; }
        #endregion
    }
}

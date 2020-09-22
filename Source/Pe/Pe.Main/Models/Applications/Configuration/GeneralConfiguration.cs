using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    public class GeneralConfiguration: ConfigurationBase
    {
        public GeneralConfiguration(IConfigurationSection section) : base(section)
        {
            //ProjectName = section.GetValue<string>("project_name");
            //MutexName = section.GetValue<string>("mutex_name");
            //LoggingConfigFileName = section.GetValue<string>("log_conf_file_name");
            //SupportCultures = section.GetSection("support_cultures").Get<string[]>();

            //LicenseName = section.GetValue<string>("license_name");
            //LicenseUri = section.GetValue<Uri>("license_uri");

            //ProjectRepositoryUri = section.GetValue<Uri>("project_repository_uri");
            //ProjectForumUri = section.GetValue<Uri>("project_forum_uri");
            //ProjectWebSiteUri = section.GetValue<Uri>("project_website_uri");
            //UpdateCheckUri = section.GetValue<Uri>("version_check_uri");
            //UpdateWait = section.GetValue<TimeSpan>("update_wait");
            //DispatcherWait = section.GetValue<TimeSpan>("dispatcher_wait");

            //CanSendCrashReport = section.GetValue<bool>("can_send_crash_report");
            //UnhandledExceptionHandled = section.GetValue<bool>("unhandled_exception_handled");
        }

        #region property

        [Configuration]
        public string ProjectName { get; } = default!;
        [Configuration]
        public string MutexName { get; } = default!;
        [Configuration("log_conf_file_name")]
        public string LogConfigFileName { get; } = default!;

        [Configuration]
        public IReadOnlyList<string> SupportCultures { get; } = default!;

        [Configuration]
        public string LicenseName { get; } = default!;
        [Configuration]
        public Uri LicenseUri { get; } = default!;

        [Configuration]
        public Uri ProjectRepositoryUri { get; } = default!;
        [Configuration]
        public Uri ProjectForumUri { get; } = default!;
        [Configuration("project_website_uri")]
        public Uri ProjectWebSiteUri { get; } = default!;
        [Configuration("version_check_uri")]
        public Uri UpdateCheckUri { get; } = default!;
        [Configuration]
        public TimeSpan UpdateWait { get; }
        [Configuration]
        public TimeSpan DispatcherWait { get; }

        [Configuration]
        public bool CanSendCrashReport { get; }
        [Configuration]
        public bool UnhandledExceptionHandled { get; }

        #endregion
    }
}

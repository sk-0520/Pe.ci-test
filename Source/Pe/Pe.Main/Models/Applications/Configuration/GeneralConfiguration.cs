using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    public class GeneralConfiguration: ConfigurationBase
    {
        public GeneralConfiguration(IConfigurationSection section) : base(section)
        { }

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
        [Configuration("author_website_uri")]
        public Uri AuthorWebSiteUri { get; } = default!;
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

using System;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    public class ApiConfiguration: ConfigurationBase
    {
        public ApiConfiguration(IConfigurationSection section)
            : base(section)
        { }

        #region property

        [Configuration]
        public Uri CrashReportUri { get; } = default!;
        [Configuration]
        public Uri CrashReportSourceUri { get; } = default!;

        [Configuration]
        public Uri FeedbackUri { get; } = default!;
        [Configuration]
        public Uri FeedbackSourceUri { get; } = default!;

        #endregion
    }
}

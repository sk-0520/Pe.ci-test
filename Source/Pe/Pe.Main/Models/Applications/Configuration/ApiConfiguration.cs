using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    public class ApiConfiguration: ConfigurationBase
    {
        public ApiConfiguration(IConfigurationSection section)
            : base(section)
        {
            //CrashReportUri = section.GetValue<Uri>("crash_report_uri");
            //CrashReportSourceUri = section.GetValue<Uri>("crash_report_src_uri");

            //FeedbackUri = section.GetValue<Uri>("feedback_uri");
            //FeedbackSourceUri = section.GetValue<Uri>("feedback_src_uri");
        }

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

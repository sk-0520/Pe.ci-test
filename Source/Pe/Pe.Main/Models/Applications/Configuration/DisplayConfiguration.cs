using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    public class DisplayConfiguration: ConfigurationBase
    {
        public DisplayConfiguration(IConfigurationSection section)
            : base(section)
        {
            //ChangedRetryCount = section.GetValue<int>("changed_retry_count");
            //ChangedRetryWaitTime = section.GetValue<TimeSpan>("changed_retry_wait");
        }

        #region property

        [Configuration]
        public int ChangedRetryCount { get; }
        [Configuration]
        public TimeSpan ChangedRetryWaitTime { get; }

        #endregion
    }
}

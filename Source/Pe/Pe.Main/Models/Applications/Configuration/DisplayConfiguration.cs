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
        { }

        #region property

        [Configuration]
        public int ChangedRetryCount { get; }
        [Configuration]
        public TimeSpan ChangedRetryWaitTime { get; }

        #endregion
    }
}

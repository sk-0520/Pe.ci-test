using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    public class NotifyLogConfiguration: ConfigurationBase
    {
        public NotifyLogConfiguration(IConfigurationSection section)
            : base(section)
        { }

        #region property

        [Configuration]
        public TimeSpan NormalLogDisplayTime { get; }
        [Configuration]
        public TimeSpan UndoLogDisplayTime { get; }
        [Configuration]
        public TimeSpan CommandLogDisplayTime { get; }

        [Configuration]
        public TimeSpan FadeoutTime { get; }

        #endregion
    }
}

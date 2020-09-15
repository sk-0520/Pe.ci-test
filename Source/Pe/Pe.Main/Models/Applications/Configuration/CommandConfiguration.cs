using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Command;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    public class CommandConfiguration: ConfigurationBase
    {
        public CommandConfiguration(IConfigurationSection section)
            : base(section)
        {
            IconClearWaitTime = section.GetValue<TimeSpan>("icon_clear_wait_time");
            ViewCloseWaitTime = section.GetValue<TimeSpan>("view_close_wait_time");

            var applicationCommand = section.GetSection("application_command");
            ApplicationPrefix = applicationCommand.GetValue<string>("prefix");
            ApplicationSeparator = applicationCommand.GetValue<string>("separator");
            ApplicationMapping = applicationCommand.GetSection("mapping").Get<Dictionary<string, string>>()
                .ToDictionary(k => EnumUtility.Parse<ApplicationCommand>(k.Key), v => v.Value)
            ;
        }

        #region property

        public TimeSpan IconClearWaitTime { get; }
        public TimeSpan ViewCloseWaitTime { get; }

        public string ApplicationPrefix { get; }
        public string ApplicationSeparator { get; }
        internal IReadOnlyDictionary<ApplicationCommand, string> ApplicationMapping { get; }

        #endregion
    }
}

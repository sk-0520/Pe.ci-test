using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main
{
    partial class Constants
    {
        public static partial class Config
        {
            #region property

            public static TimeSpan LauncherToolbarMainDatabaseLazyWriterWaitTime { get; } = TimeSpan.Parse(ConfigurationManager.AppSettings["launcher-toolbar-main-database-lazy-writer-wait-time"]);

            #endregion
        }
    }
}

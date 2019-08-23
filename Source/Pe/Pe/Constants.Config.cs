using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace ContentTypeTextNet.Pe.Main
{
    partial class Constants
    {
        public static partial class Config
        {
            #region property

            public static TimeSpan LauncherToolbarMainDatabaseLazyWriterWaitTime { get; } = TimeSpan.Parse(ConfigurationManager.AppSettings["launcher-toolbar-main-database-lazy-writer-wait-time"]);
            public static TimeSpan NoteMainDatabaseLazyWriterWaitTime { get; } = TimeSpan.Parse(ConfigurationManager.AppSettings["note-main-database-lazy-writer-wait-time"]);
            public static TimeSpan NoteContentMainDatabaseLazyWriterWaitTime { get; } = TimeSpan.Parse(ConfigurationManager.AppSettings["note-content-main-database-lazy-writer-wait-time"]);
            public static TimeSpan FontMainDatabaseLazyWriterWaitTime { get; } = TimeSpan.Parse(ConfigurationManager.AppSettings["font-database-lazy-writer-wait-time"]);

            #endregion
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    public interface IApplicationDatabaseLazyWriter : IDatabaseLazyWriter
    { }

    public interface IMainDatabaseLazyWriter : IApplicationDatabaseLazyWriter
    { }
    public interface IFileDatabaseLazyWriter : IApplicationDatabaseLazyWriter
    { }
    public interface ITemporaryDatabaseLazyWriter : IApplicationDatabaseLazyWriter
    { }

    public class ApplicationDatabaseLazyWriter : DatabaseLazyWriter, IMainDatabaseLazyWriter, IFileDatabaseLazyWriter, ITemporaryDatabaseLazyWriter
    {
        public ApplicationDatabaseLazyWriter(IDatabaseBarrier databaseBarrier, TimeSpan pauseRetryTime, ILoggerFactory loggerFactory)
            : base(databaseBarrier, pauseRetryTime, loggerFactory)
        { }
    }
}

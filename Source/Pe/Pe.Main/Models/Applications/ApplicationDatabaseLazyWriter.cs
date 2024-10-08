using System;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    public interface IApplicationDatabaseDelayWriter: IDatabaseDelayWriter
    { }

    public interface IMainDatabaseDelayWriter: IApplicationDatabaseDelayWriter
    { }
    public interface ILargeDatabaseDelayWriter: IApplicationDatabaseDelayWriter
    { }
    public interface ITemporaryDatabaseDelayWriter: IApplicationDatabaseDelayWriter
    { }

    internal class ApplicationDatabaseDelayWriter: DatabaseDelayWriter, IMainDatabaseDelayWriter, ILargeDatabaseDelayWriter, ITemporaryDatabaseDelayWriter
    {
        public ApplicationDatabaseDelayWriter(IDatabaseBarrier databaseBarrier, TimeSpan pauseRetryTime, ILoggerFactory loggerFactory)
            : base(databaseBarrier, pauseRetryTime, loggerFactory)
        { }
    }
}

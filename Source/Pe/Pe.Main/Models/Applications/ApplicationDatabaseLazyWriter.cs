using System;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Standard.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    public interface IApplicationDatabaseLazyWriter: IDatabaseLazyWriter
    { }

    public interface IMainDatabaseLazyWriter: IApplicationDatabaseLazyWriter
    { }
    public interface ILargeDatabaseLazyWriter: IApplicationDatabaseLazyWriter
    { }
    public interface ITemporaryDatabaseLazyWriter: IApplicationDatabaseLazyWriter
    { }

    internal class ApplicationDatabaseLazyWriter: DatabaseLazyWriter, IMainDatabaseLazyWriter, ILargeDatabaseLazyWriter, ITemporaryDatabaseLazyWriter
    {
        public ApplicationDatabaseLazyWriter(IDatabaseBarrier databaseBarrier, TimeSpan pauseRetryTime, ILoggerFactory loggerFactory)
            : base(databaseBarrier, pauseRetryTime, loggerFactory)
        { }
    }
}

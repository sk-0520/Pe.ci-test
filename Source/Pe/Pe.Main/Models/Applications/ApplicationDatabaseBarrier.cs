using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Library.Database;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    public interface IApplicationDatabaseBarrier: IDatabaseBarrier
    { }

    public interface IMainDatabaseBarrier: IApplicationDatabaseBarrier
    { }
    public interface ILargeDatabaseBarrier: IApplicationDatabaseBarrier
    { }
    public interface ITemporaryDatabaseBarrier: IApplicationDatabaseBarrier
    { }

    internal sealed class ApplicationDatabaseBarrier: DatabaseBarrier, IMainDatabaseBarrier, ILargeDatabaseBarrier, ITemporaryDatabaseBarrier
    {
        public ApplicationDatabaseBarrier(IDatabaseAccessor accessor, ReadWriteLockHelper locker)
            : base(accessor, locker)
        { }
    }
}

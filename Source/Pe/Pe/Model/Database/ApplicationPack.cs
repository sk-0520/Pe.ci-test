using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Database
{
    public interface IApplicationPack<T>
    {
        #region property

        T Main { get; }
        T File { get; }
        T Temporary { get; }

        #endregion
    }

    public abstract class TApplicationPackBase<TInterface, TObject> : IApplicationPack<TInterface>
        where TObject : TInterface
    {
        protected TApplicationPackBase(TObject main, TObject file, TObject temporary)
        {
            Main = main;
            File = file;
            Temporary = temporary;
        }

        #region IApplicationPack

        public TObject Main { get; }
        TInterface IApplicationPack<TInterface>.Main => Main;

        public TObject File { get; }
        TInterface IApplicationPack<TInterface>.File => File;

        public TObject Temporary { get; }
        TInterface IApplicationPack<TInterface>.Temporary => Temporary;

        #endregion
    }

    public interface IDatabaseFactoryPack : IApplicationPack<IDatabaseFactory>
    { }

    public sealed class DatabaseFactoryPack : TApplicationPackBase<IDatabaseFactory, ApplicationDatabaseFactory>, IDatabaseFactoryPack
    {
        public DatabaseFactoryPack(ApplicationDatabaseFactory main, ApplicationDatabaseFactory file, ApplicationDatabaseFactory temporary)
            : base(main, file, temporary)
        { }
    }

    public interface IDatabaseAccessorPack : IApplicationPack<IDatabaseAccessor>
    { }

    public sealed class DatabaseAccessorPack : TApplicationPackBase<IDatabaseAccessor, ApplicationDatabaseAccessor>, IDatabaseAccessorPack
    {
        public DatabaseAccessorPack(ApplicationDatabaseAccessor main, ApplicationDatabaseAccessor file, ApplicationDatabaseAccessor temporary)
            : base(main, file, temporary)
        { }

        #region function

        public static DatabaseAccessorPack Create(DatabaseFactoryPack factoryPack, ILoggerFactory loggerFactory)
        {
            return new DatabaseAccessorPack(
                new ApplicationDatabaseAccessor(factoryPack.Main, loggerFactory),
                new ApplicationDatabaseAccessor(factoryPack.File, loggerFactory),
                new ApplicationDatabaseAccessor(factoryPack.Temporary, loggerFactory)
            );
        }

        #endregion
    }
}

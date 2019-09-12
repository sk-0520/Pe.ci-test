using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Common.Model;
using ContentTypeTextNet.Pe.Core.Model;
using ContentTypeTextNet.Pe.Core.Model.Database;
using ContentTypeTextNet.Pe.Main.Model.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Model.Applications
{
    public enum Pack
    {
        Main,
        File,
        Temporary,
    }

    public interface IApplicationPack<out T> : IDisposable
    {
        #region property

        T Main { get; }
        T File { get; }
        T Temporary { get; }

        IReadOnlyList<T> Items { get; }

        T this[Pack index] { get; }

        #endregion
    }

    public abstract class TApplicationPackBase<TInterface, TObject> : DisposerBase, IApplicationPack<TInterface>
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

        public IReadOnlyList<TObject> Items => new[] {
            Main,
            File,
            Temporary,
        };
        IReadOnlyList<TInterface> IApplicationPack<TInterface>.Items => (IReadOnlyList<TInterface>)Items; // あっれぇ

        public TObject this[Pack index] => Items[(int)index];
        TInterface IApplicationPack<TInterface>.this[Pack index] => this[index];

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    foreach(var item in Items) {
                        if(item is IDisposable disposer) {
                            disposer.Dispose();
                        }
                    }
                }
            }

            base.Dispose(disposing);
        }

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

    public interface IReadWriteLockPack : IApplicationPack<ReaderWriterLocker>
    { }

    public sealed class ReadWriteLockPack : TApplicationPackBase<ReaderWriterLocker, ApplicationReaderWriterLockerBase>, IReadWriteLockPack
    {
        public ReadWriteLockPack(ApplicationMainReaderWriterLocker main, ApplicationFileReaderWriterLocker file, ApplicationTemporaryReaderWriterLocker temporary)
            : base(main, file, temporary)
        { }
    }
}

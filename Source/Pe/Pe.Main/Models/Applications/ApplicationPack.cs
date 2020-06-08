using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    public enum Pack
    {
        Main,
        File,
        Temporary,
    }

    public interface IApplicationPack<out T>: IDisposable
    {
        #region property

        T Main { get; }
        T File { get; }
        T Temporary { get; }

        IReadOnlyList<T> Items { get; }

        T this[Pack index] { get; }

        #endregion
    }

    public abstract class TApplicationPackBase<TInterface, TObject>: DisposerBase, IApplicationPack<TInterface>
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

    public interface IReaderWriterLockerPack: IApplicationPack<IReaderWriterLocker>
    { }

    public sealed class ApplicationReaderWriterLockerPack: TApplicationPackBase<IReaderWriterLocker, ApplicationReaderWriterLockerBase>, IReaderWriterLockerPack
    {
        public ApplicationReaderWriterLockerPack(ApplicationMainReaderWriterLocker main, ApplicationFileReaderWriterLocker file, ApplicationTemporaryReaderWriterLocker temporary)
            : base(main, file, temporary)
        { }
    }

    public interface IDatabaseFactoryPack: IApplicationPack<IDatabaseFactory>
    { }

    public sealed class ApplicationDatabaseFactoryPack: TApplicationPackBase<IDatabaseFactory, ApplicationDatabaseFactory>, IDatabaseFactoryPack
    {
        public ApplicationDatabaseFactoryPack(ApplicationDatabaseFactory main, ApplicationDatabaseFactory file, ApplicationDatabaseFactory temporary)
            : base(main, file, temporary)
        { }
    }

    public class LazyWriterWaitTimePack: TApplicationPackBase<TimeSpan, TimeSpan>
    {
        public LazyWriterWaitTimePack(TimeSpan main, TimeSpan file, TimeSpan temporary)
            : base(main, file, temporary)
        { }
    }

    public interface IDatabaseLazyWriterPack: IApplicationPack<IDatabaseLazyWriter>
    { }

    public sealed class ApplicationDatabaseLazyWriterPack: TApplicationPackBase<IDatabaseLazyWriter, ApplicationDatabaseLazyWriter>, IDatabaseLazyWriterPack
    {
        public ApplicationDatabaseLazyWriterPack(ApplicationDatabaseLazyWriter main, ApplicationDatabaseLazyWriter file, ApplicationDatabaseLazyWriter temporary)
            : base(main, file, temporary)
        { }
    }

    public interface IDatabaseAccessorPack: IApplicationPack<IDatabaseAccessor>
    { }

    public sealed class ApplicationDatabaseAccessorPack: TApplicationPackBase<IDatabaseAccessor, ApplicationDatabaseAccessor>, IDatabaseAccessorPack
    {
        public ApplicationDatabaseAccessorPack(ApplicationDatabaseAccessor main, ApplicationDatabaseAccessor file, ApplicationDatabaseAccessor temporary)
            : base(main, file, temporary)
        { }

        #region function

        public static ApplicationDatabaseAccessorPack Create(ApplicationDatabaseFactoryPack factoryPack, ILoggerFactory loggerFactory)
        {
            return new ApplicationDatabaseAccessorPack(
                new ApplicationDatabaseAccessor(factoryPack.Main, loggerFactory),
                new ApplicationDatabaseAccessor(factoryPack.File, loggerFactory),
                new ApplicationDatabaseAccessor(factoryPack.Temporary, loggerFactory)
            );
        }

        #endregion
    }

    public interface IDatabaseCommandsPack: IApplicationPack<IDatabaseCommands>
    {
        #region property

        IDatabaseCommonStatus CommonStatus { get; }

        #endregion
    }

    internal class ApplicationDatabaseCommandsPack: TApplicationPackBase<IDatabaseCommands, DatabaseCommands>, IDatabaseCommandsPack
    {
        public ApplicationDatabaseCommandsPack(DatabaseCommands main, DatabaseCommands file, DatabaseCommands temporary, IDatabaseCommonStatus commonStatus)
            : base(main, file, temporary)
        {
            CommonStatus = commonStatus;
        }

        #region IDatabaseCommandsPack

        /// <inheritdoc cref="IDatabaseCommandsPack.CommonStatus"/>
        public IDatabaseCommonStatus CommonStatus { get; }

        #endregion
    }

    public interface IDatabaseBarrierPack: IApplicationPack<IDatabaseBarrier>
    {

        #region function

        IDatabaseCommandsPack WaitRead();
        IDatabaseCommandsPack WaitWrite();

        /// <summary>
        /// トランザクション処理を確定する。
        /// <para>トランザクション中でない場合は特に何も起きない。</para>
        /// </summary>
        void Save();

        #endregion
    }

    public sealed class ApplicationDatabaseBarrierPack: TApplicationPackBase<IDatabaseBarrier, ApplicationDatabaseBarrier>, IDatabaseBarrierPack
    {
        #region define

        internal class Barriers: ApplicationDatabaseCommandsPack
        {
            public Barriers(DatabaseCommands main, DatabaseCommands file, DatabaseCommands temporary, IDatabaseCommonStatus commonStatus, bool isReadOnly)
                : base(main, file, temporary, commonStatus)
            {
                IsReadOnly = isReadOnly;
            }

            #region property

            public bool IsReadOnly { get; }

            #endregion

            #region function

            public void Commit()
            {
                foreach(var tran in Items.OfType<IDatabaseTransaction>()) {
                    tran.Commit();
                }
            }

            #endregion

            #region TApplicationPackBase

            protected override void Dispose(bool disposing)
            {
                if(!IsDisposed) {
                    if(disposing) {
                        var disposableItems = Items
                            .Select(i => i.Commander)
                            .OfType<IDisposable>()
                            .ToList()
                        ;
                        foreach(var disposableItem in disposableItems) {
                            disposableItem.Dispose();
                        }
                    }
                }

                base.Dispose(disposing);
            }

            #endregion
        }

        #endregion
        public ApplicationDatabaseBarrierPack(ApplicationDatabaseBarrier main, ApplicationDatabaseBarrier file, ApplicationDatabaseBarrier temporary)
            : base(main, file, temporary)
        { }

        #region property

        Barriers? CurrentBarriers { get; set; }

        #endregion

        #region function

        public static ApplicationDatabaseAccessorPack Create(ApplicationDatabaseFactoryPack factoryPack, ILoggerFactory loggerFactory)
        {
            return new ApplicationDatabaseAccessorPack(
                new ApplicationDatabaseAccessor(factoryPack.Main, loggerFactory),
                new ApplicationDatabaseAccessor(factoryPack.File, loggerFactory),
                new ApplicationDatabaseAccessor(factoryPack.Temporary, loggerFactory)
            );
        }

        DatabaseCommands WaitReadCore(IDatabaseBarrier barrier)
        {
            var tran = barrier.WaitRead();
            return new DatabaseCommands(tran, tran.Implementation);
        }

        DatabaseCommands WaitWriteCore(IDatabaseBarrier barrier)
        {
            var tran = barrier.WaitWrite();
            return new DatabaseCommands(tran, tran.Implementation);
        }

        #endregion

        #region IDatabaseBarrierPack

        internal Barriers WaitRead()
        {
            if(CurrentBarriers != null) {
                throw new InvalidOperationException();
            }

            CurrentBarriers = new Barriers(WaitReadCore(Main), WaitReadCore(File), WaitReadCore(Temporary), DatabaseCommonStatus.CreateCurrentAccount(), true);
            return CurrentBarriers;
        }
        IDatabaseCommandsPack IDatabaseBarrierPack.WaitRead() => WaitRead();

        internal Barriers WaitWrite(IDatabaseCommonStatus databaseCommonStatus)
        {
            if(CurrentBarriers != null) {
                throw new InvalidOperationException();
            }

            CurrentBarriers = new Barriers(WaitWriteCore(Main), WaitWriteCore(File), WaitWriteCore(Temporary), databaseCommonStatus, true);
            return CurrentBarriers;
        }
        IDatabaseCommandsPack IDatabaseBarrierPack.WaitWrite() => WaitRead();

        public void Save()
        {
            CurrentBarriers?.Commit();
        }

        #endregion
    }

}

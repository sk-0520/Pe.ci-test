using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    /// <summary>
    /// DB区分。
    /// </summary>
    public enum Pack
    {
        /// <summary>
        /// 通常設定。
        /// </summary>
        Main,
        /// <summary>
        /// 大きいデータ。
        /// </summary>
        Large,
        /// <summary>
        /// 一時データ。
        /// </summary>
        /// <remarks>
        /// <para>次回起動時は存在しない。</para>
        /// </remarks>
        Temporary,
    }

    public interface IApplicationPack<out T>: IDisposable
    {
        #region property

        [NotNull]
        T Main { get; }
        [NotNull]
        T Large { get; }
        [NotNull]
        T Temporary { get; }

        IReadOnlyList<T> Items { get; }

        T this[Pack index] { get; }

        #endregion
    }

    internal abstract class TApplicationPackBase<TInterface, TObject>: DisposerBase, IApplicationPack<TInterface>
        where TObject : TInterface
    {
        protected TApplicationPackBase([DisallowNull] TObject main, [DisallowNull] TObject large, [DisallowNull] TObject temporary)
        {
            Main = main;
            Large = large;
            Temporary = temporary;
        }

        #region IApplicationPack

        [NotNull]
        public TObject Main { get; }
        [NotNull]
        TInterface IApplicationPack<TInterface>.Main => Main;

        [NotNull]
        public TObject Large { get; }
        [NotNull]
        TInterface IApplicationPack<TInterface>.Large => Large;

        [NotNull]
        public TObject Temporary { get; }
        [NotNull]
        TInterface IApplicationPack<TInterface>.Temporary => Temporary;

        public IReadOnlyList<TObject> Items => new[] {
            Main,
            Large,
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

    public interface IReadWriteLockHelperPack: IApplicationPack<IReadWriteLockHelper>
    { }

    internal sealed class ApplicationReadWriteLockHelperPack: TApplicationPackBase<IReadWriteLockHelper, ApplicationReadWriteLockHelperBase>, IReadWriteLockHelperPack
    {
        public ApplicationReadWriteLockHelperPack(ApplicationMainReadWriteLockHelper main, ApplicationLargeReadWriteLockHelper large, ApplicationTemporaryReadWriteLockHelper temporary)
            : base(main, large, temporary)
        { }
    }

    public interface IDatabaseFactoryPack: IApplicationPack<IDatabaseFactory>
    { }

    internal sealed class ApplicationDatabaseFactoryPack: TApplicationPackBase<IDatabaseFactory, ApplicationDatabaseFactory>, IDatabaseFactoryPack
    {
        public ApplicationDatabaseFactoryPack(ApplicationDatabaseFactory main, ApplicationDatabaseFactory large, ApplicationDatabaseFactory temporary)
            : base(main, large, temporary)
        { }
    }

    internal class DelayWriterWaitTimePack: TApplicationPackBase<TimeSpan, TimeSpan>
    {
        public DelayWriterWaitTimePack(TimeSpan main, TimeSpan large, TimeSpan temporary)
            : base(main, large, temporary)
        { }
    }

    public interface IDatabaseDelayWriterPack: IApplicationPack<IDatabaseDelayWriter>
    { }

    internal sealed class ApplicationDatabaseDelayWriterPack: TApplicationPackBase<IDatabaseDelayWriter, ApplicationDatabaseDelayWriter>, IDatabaseDelayWriterPack
    {
        public ApplicationDatabaseDelayWriterPack(ApplicationDatabaseDelayWriter main, ApplicationDatabaseDelayWriter large, ApplicationDatabaseDelayWriter temporary)
            : base(main, large, temporary)
        { }
    }

    public interface IDatabaseAccessorPack: IApplicationPack<IDatabaseAccessor>
    { }

    internal sealed class ApplicationDatabaseAccessorPack: TApplicationPackBase<IDatabaseAccessor, ApplicationDatabaseAccessor>, IDatabaseAccessorPack
    {
        public ApplicationDatabaseAccessorPack(ApplicationDatabaseAccessor main, ApplicationDatabaseAccessor large, ApplicationDatabaseAccessor temporary)
            : base(main, large, temporary)
        { }

        #region function

        public static ApplicationDatabaseAccessorPack Create(ApplicationDatabaseFactoryPack factoryPack, ILoggerFactory loggerFactory)
        {
            return new ApplicationDatabaseAccessorPack(
                new ApplicationDatabaseAccessor(factoryPack.Main, loggerFactory),
                new ApplicationDatabaseAccessor(factoryPack.Large, loggerFactory),
                new ApplicationDatabaseAccessor(factoryPack.Temporary, loggerFactory)
            );
        }

        #endregion
    }

    public interface IDatabaseContextsPack: IApplicationPack<IDatabaseContexts>
    {
        #region property

        IDatabaseCommonStatus CommonStatus { get; }

        #endregion
    }

    internal class ApplicationDatabaseContextsPack: TApplicationPackBase<IDatabaseContexts, DatabaseContexts>, IDatabaseContextsPack
    {
        public ApplicationDatabaseContextsPack(DatabaseContexts main, DatabaseContexts large, DatabaseContexts temporary, IDatabaseCommonStatus commonStatus)
            : base(main, large, temporary)
        {
            CommonStatus = commonStatus;
        }

        #region IDatabaseContextsPack

        /// <inheritdoc cref="IDatabaseContextsPack.CommonStatus"/>
        public IDatabaseCommonStatus CommonStatus { get; }

        #endregion
    }

    public interface IDatabaseBarrierPack: IApplicationPack<IDatabaseBarrier>
    {

        #region function

        IDatabaseContextsPack WaitRead();
        IDatabaseContextsPack WaitWrite();

        /// <summary>
        /// トランザクション処理を確定する。
        /// </summary>
        /// <remarks>
        /// <para>トランザクション中でない場合は特に何も起きない。</para>
        /// </remarks>
        void Save();

        #endregion
    }

    internal sealed class ApplicationDatabaseBarrierPack: TApplicationPackBase<IDatabaseBarrier, ApplicationDatabaseBarrier>, IDatabaseBarrierPack
    {
        #region define

        internal class Barriers: ApplicationDatabaseContextsPack
        {
            public Barriers(DatabaseContexts main, DatabaseContexts large, DatabaseContexts temporary, IDatabaseCommonStatus commonStatus, bool isReadOnly)
                : base(main, large, temporary, commonStatus)
            {
                IsReadOnly = isReadOnly;
            }

            #region property

            public bool IsReadOnly { get; }

            #endregion

            #region function

            public void Commit()
            {
                foreach(var transaction in Items.Select(i => i.Context).OfType<IDatabaseTransaction>()) {
                    transaction.Commit();
                }
            }

            #endregion

            #region TApplicationPackBase

            protected override void Dispose(bool disposing)
            {
                if(!IsDisposed) {
                    if(disposing) {
                        var disposableItems = Items
                            .Select(i => i.Context)
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
        public ApplicationDatabaseBarrierPack(ApplicationDatabaseBarrier main, ApplicationDatabaseBarrier large, ApplicationDatabaseBarrier temporary)
            : base(main, large, temporary)
        { }

        #region property

        Barriers? CurrentBarriers { get; set; }

        #endregion

        #region function

        public static ApplicationDatabaseAccessorPack Create(ApplicationDatabaseFactoryPack factoryPack, ILoggerFactory loggerFactory)
        {
            return new ApplicationDatabaseAccessorPack(
                new ApplicationDatabaseAccessor(factoryPack.Main, loggerFactory),
                new ApplicationDatabaseAccessor(factoryPack.Large, loggerFactory),
                new ApplicationDatabaseAccessor(factoryPack.Temporary, loggerFactory)
            );
        }

        DatabaseContexts WaitReadCore(IDatabaseBarrier barrier)
        {
            var transaction = barrier.WaitRead();
            return new DatabaseContexts(transaction, transaction.Implementation);
        }

        DatabaseContexts WaitWriteCore(IDatabaseBarrier barrier)
        {
            var transaction = barrier.WaitWrite();
            return new DatabaseContexts(transaction, transaction.Implementation);
        }

        #endregion

        #region IDatabaseBarrierPack

        internal Barriers WaitRead()
        {
            if(CurrentBarriers != null) {
                throw new InvalidOperationException();
            }

            CurrentBarriers = new Barriers(WaitReadCore(Main), WaitReadCore(Large), WaitReadCore(Temporary), DatabaseCommonStatus.CreateCurrentAccount(), true);
            CurrentBarriers.Disposing += CurrentBarriers_Disposing;
            return CurrentBarriers;
        }

        IDatabaseContextsPack IDatabaseBarrierPack.WaitRead() => WaitRead();

        internal Barriers WaitWrite(/*IDatabaseCommonStatus databaseCommonStatus*/)
        {
            if(CurrentBarriers != null) {
                throw new InvalidOperationException();
            }

            CurrentBarriers = new Barriers(WaitWriteCore(Main), WaitWriteCore(Large), WaitWriteCore(Temporary), DatabaseCommonStatus.CreateCurrentAccount(), false);
            CurrentBarriers.Disposing += CurrentBarriers_Disposing;
            return CurrentBarriers;
        }
        IDatabaseContextsPack IDatabaseBarrierPack.WaitWrite() => WaitWrite();

        public void Save()
        {
            CurrentBarriers?.Commit();
        }

        #endregion

        #region TApplicationPackBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    if(CurrentBarriers != null) {
                        CurrentBarriers.Disposing -= CurrentBarriers_Disposing;
                        CurrentBarriers.Dispose();
                        CurrentBarriers = null;
                    }
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        private void CurrentBarriers_Disposing(object? sender, EventArgs e)
        {
            Debug.Assert(CurrentBarriers != null);

            CurrentBarriers.Disposing -= CurrentBarriers_Disposing;
            CurrentBarriers = null;
        }
    }
}

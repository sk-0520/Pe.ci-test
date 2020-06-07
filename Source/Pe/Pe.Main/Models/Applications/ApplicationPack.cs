using System;
using System.Collections.Generic;
using System.DirectoryServices;
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

    internal sealed class ApplicationDatabaseCommandsPack: TApplicationPackBase<IDatabaseCommands, DatabaseCommands>, IDatabaseCommandsPack
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

    internal class Barriers: TApplicationPackBase<IDatabaseCommander, IDatabaseTransaction>
    {
        public Barriers(IDatabaseTransaction main, IDatabaseTransaction file, IDatabaseTransaction temporary, bool isReadOnly)
            : base(main, file, temporary)
        {
            IsReadOnly = isReadOnly;
        }

        #region property

        public bool IsReadOnly { get; }

        #endregion

        #region TApplicationPackBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    Main.Dispose();
                    File.Dispose();
                    Temporary.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    public interface IDatabaseBarrierPack: IApplicationPack<IDatabaseBarrier>
    {

        #region function

        IApplicationPack<IDatabaseCommander> WaitRead();
        IApplicationPack<IDatabaseCommander> WaitWrite();

        #endregion
    }

    public sealed class ApplicationDatabaseBarrierPack: TApplicationPackBase<IDatabaseBarrier, ApplicationDatabaseBarrier>, IDatabaseBarrierPack
    {
        public ApplicationDatabaseBarrierPack(ApplicationDatabaseBarrier main, ApplicationDatabaseBarrier file, ApplicationDatabaseBarrier temporary)
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

        #region IDatabaseBarrierPack

        internal Barriers WaitRead()
        {
            return new Barriers(Main.WaitRead(), File.WaitRead(), Temporary.WaitRead(), true);
        }
        IApplicationPack<IDatabaseCommander> IDatabaseBarrierPack.WaitRead() => WaitRead();

        internal Barriers WaitWrite()
        {
            return new Barriers(Main.WaitWrite(), File.WaitWrite(), Temporary.WaitWrite(), false);
        }
        IApplicationPack<IDatabaseCommander> IDatabaseBarrierPack.WaitWrite() => WaitRead();

        #endregion
    }

    internal sealed class TApplicationPack<TInterface, TObject>: TApplicationPackBase<TInterface, TObject>
        where TObject : TInterface
    {
        public TApplicationPack(TObject main, TObject file, TObject temporary)
            : base(main, file, temporary)
        { }
    }

    internal static class TApplicationPack
    {
        #region function

        public static TApplicationPack<TObject, TObject> Create<TObject>(TObject main, TObject file, TObject temporary) => new TApplicationPack<TObject, TObject>(main, file, temporary);

        #endregion
    }
}

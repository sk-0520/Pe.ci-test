using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using Prism.Mvvm;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
    public interface IReadOnlyData
    { }

    public interface IData : IReadOnlyData
    { }

    [Serializable, DataContract]
    public abstract class DataBase : IData
    { }

    interface IReadOnlyTransferModel : IReadOnlyData
    { }

    interface ITransferModel : IData
    { }

    [Serializable, DataContract]
    public abstract class TransferModelBase : DataBase, ITransferModel
    { }

    public abstract class DataAccessModelBase
    {
        public DataAccessModelBase(IDatabaseCommander databaseCommander, ILogger logger)
        {
            DatabaseCommander = databaseCommander;
            Logger = logger;
        }

        public DataAccessModelBase(IDatabaseCommander databaseCommander, ILoggerFactory loggerFactory)
        {
            DatabaseCommander = databaseCommander;
            Logger = loggerFactory.CreateTartget(GetType());
        }


        #region property

        protected IDatabaseCommander DatabaseCommander { get; }
        protected ILogger Logger { get; }

        #endregion
    }

    public abstract class BindModelBase : BindableBase, IDisposable
    {
        public BindModelBase(ILogger logger)
        {
            Logger = logger;
        }

        public BindModelBase(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateTartget(GetType());
        }

        ~BindModelBase()
        {
            Dispose(false);
        }

        #region property

        protected ILogger Logger { get; private set; }

        #endregion

        #region IDisposable

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>時に呼び出されるイベント。
        /// <para>呼び出し時点では<see cref="IsDisposed"/>は偽のまま。</para>
        /// </summary>
        [field: NonSerialized]
        public event EventHandler Disposing;

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>されたか。
        /// </summary>
        [IgnoreDataMember, XmlIgnore]
        public bool IsDisposed { get; private set; }

        protected void ThrowIfDisposed([CallerMemberName] string _callerMemberName = "")
        {
            if(IsDisposed) {
                throw new ObjectDisposedException(_callerMemberName);
            }
        }


        /// <summary>
        /// <see cref="IDisposable.Dispose"/>の内部処理。
        /// <para>継承先クラスでは本メソッドを呼び出す必要がある。</para>
        /// </summary>
        /// <param name="disposing">CLRの管理下か。</param>
        protected virtual void Dispose(bool disposing)
        {
            if(IsDisposed) {
                return;
            }

            if(Disposing != null) {
                Disposing(this, EventArgs.Empty);
            }

            if(disposing) {
                if(Logger is IDisposable disposer) {
                    disposer.Dispose();
                }

                GC.SuppressFinalize(this);
            }

            IsDisposed = true;
        }

        /// <summary>
        /// 解放。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

    }

    public interface IRawModel : IReadOnlyData
    {
        #region property

        object BaseRawObject { get; }

        #endregion
    }

    public interface IRawModel<T> : IData, IRawModel
    {
        #region property

        T Raw { get; }

        #endregion
    }

    public class RawModel : DisposerBase, IRawModel
    {
        public RawModel(object rawObject)
        {
            BaseRawObject = rawObject;
        }

        #region IRawModel

        public object BaseRawObject { get; }

        #endregion
    }

    public class RawModel<T> : RawModel, IRawModel<T>
    {
        public RawModel(T rawObject)
            : base(rawObject)
        {
            Raw = rawObject;
        }

        #region IRawModel

        public T Raw { get; }

        #endregion
    }
}

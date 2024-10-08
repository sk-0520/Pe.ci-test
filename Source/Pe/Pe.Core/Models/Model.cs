using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;
using Prism.Mvvm;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public abstract class NotifyPropertyBase: BindableBase, IDisposer
    {
        protected NotifyPropertyBase()
        { }

        ~NotifyPropertyBase()
        {
            Dispose(false);
        }

        #region property
        #endregion

        #region IDisposable

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>時に呼び出されるイベント。
        /// </summary>
        /// <remarks>
        /// <para>呼び出し時点では<see cref="IsDisposed"/>は偽のまま。</para>
        /// </remarks>
        [field: NonSerialized]
        public event EventHandler<EventArgs>? Disposing;

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>されたか。
        /// </summary>
        [IgnoreDataMember, XmlIgnore]
        public bool IsDisposed { get; private set; }

        protected void ThrowIfDisposed()
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
        }

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>の内部処理。
        /// </summary>
        /// <remarks>
        /// <para>継承先クラスでは本メソッドを呼び出す必要がある。</para>
        /// </remarks>
        /// <param name="disposing">CLRの管理下か。</param>
        protected virtual void Dispose(bool disposing)
        {
            if(IsDisposed) {
                return;
            }

            Disposing?.Invoke(this, EventArgs.Empty);

            IsDisposed = true;
        }

        /// <summary>
        /// 解放。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }

    public abstract class BindModelBase: NotifyPropertyBase
    {
        protected BindModelBase(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            LoggerFactory = loggerFactory;
        }

        ~BindModelBase()
        {
            Dispose(false);
        }

        #region property

        protected ILogger Logger { get; private set; }
        protected ILoggerFactory LoggerFactory { get; private set; }

        #endregion

        //#region NotifyPropertyBase

        //protected override void Dispose(bool disposing)
        //{
        //    if(!IsDisposed) {
        //        if(disposing) {
        //            if(Logger is IDisposable disposer) {
        //                disposer.Dispose();
        //            }
        //        }
        //    }

        //    base.Dispose(disposing);
        //}

        //#endregion

    }
}

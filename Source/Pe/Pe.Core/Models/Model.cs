using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using Microsoft.Extensions.Logging;
using Prism.Mvvm;

namespace ContentTypeTextNet.Pe.Core.Models
{

    public abstract class BindModelBase : BindableBase, IDisposable, IDisposer
    {
        public BindModelBase(ILoggerFactory loggerFactory)
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

        #region IDisposable

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>時に呼び出されるイベント。
        /// <para>呼び出し時点では<see cref="IsDisposed"/>は偽のまま。</para>
        /// </summary>
        [field: NonSerialized]
        public event EventHandler? Disposing;

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

}

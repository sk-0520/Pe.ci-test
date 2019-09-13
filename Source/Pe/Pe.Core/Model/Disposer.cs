using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace ContentTypeTextNet.Pe.Core.Model
{
    public interface IDisposer : IDisposable
    {
        #region event

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>時に呼び出されるイベント。
        /// <para>呼び出し時点では<see cref="IsDisposed"/>は偽のまま。</para>
        /// </summary>
        event EventHandler Disposing;

        #endregion

        #region propert

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>されたか。
        /// </summary>
        bool IsDisposed { get; }

        #endregion
    }
    /// <summary>
    /// <see cref="IDisposable.Dispose"/>をサポートする基底クラス。
    /// </summary>
    public abstract class DisposerBase : IDisposer, IDisposable
    {
        ~DisposerBase()
        {
            Dispose(false);
        }

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

    public sealed class ActionDisposer : DisposerBase
    {
        public ActionDisposer(Action action)
            : this(d => action())
        {
            if(action == null) {
                throw new ArgumentNullException(nameof(action));
            }
        }

        public ActionDisposer(Action<bool> action)
        {
            if(action == null) {
                throw new ArgumentNullException(nameof(action));
            }

            Action = action;
        }

        #region property

        Action<bool> Action { get; set; }

        #endregion

        #region ActionDisposer

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(Action != null) {
                    Action(disposing);
                    //Action = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    public class GroupDisposer : DisposerBase
    {
        #region property

        IList<IDisposable> StockItems { get; } = new List<IDisposable>();

        #endregion

        #region function

        public TDisposable Add<TDisposable>(TDisposable disposable)
            where TDisposable : IDisposable
        {
            ThrowIfDisposed();

            if(disposable != null) {
                StockItems.Add(disposable);
            }

            return disposable;
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    for(var i = StockItems.Count - 1; 0 < i; i--) {
                        StockItems[i].Dispose();
                    }
                    StockItems.Clear();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}

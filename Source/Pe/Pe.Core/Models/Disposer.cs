using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public interface IDisposedChackable
    {
        #region propert

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>されたか。
        /// </summary>
        bool IsDisposed { get; }

        #endregion
    }

    public interface IDisposer : IDisposedChackable, IDisposable
    {
        #region event

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>時に呼び出されるイベント。
        /// <para>呼び出し時点では<see cref="IsDisposed"/>は偽のまま。</para>
        /// </summary>
        event EventHandler Disposing;

        #endregion
    }
    /// <summary>
    /// <see cref="IDisposable.Dispose"/>をサポートする基底クラス。
    /// </summary>
    public abstract class DisposerBase: IDisposer
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
        public event EventHandler? Disposing;

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>されたか。
        /// </summary>
        [IgnoreDataMember, XmlIgnore]
        public bool IsDisposed { get; protected set; }

        protected void ThrowIfDisposed([CallerMemberName] string _callerMemberName = "")
        {
            if(IsDisposed) {
                throw new ObjectDisposedException(_callerMemberName);
            }
        }

        protected void OnDisposing()
        {
            Disposing?.Invoke(this, EventArgs.Empty);
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

            OnDisposing();

            if(disposing) {
#pragma warning disable S3971 // "GC.SuppressFinalize" should not be called
                GC.SuppressFinalize(this);
#pragma warning restore S3971 // "GC.SuppressFinalize" should not be called
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
        public ActionDisposer(Action<bool> action)
        {
            Action = action ?? throw new ArgumentNullException(nameof(action));
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
                    Action = null!;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    public sealed class ActionDisposer<TArgument> : DisposerBase
    {
        public ActionDisposer(Action<bool, TArgument> action, TArgument argument)
        {
            Action = action ?? throw new ArgumentNullException(nameof(action));
            Argument = argument;
        }

        #region property

        Action<bool, TArgument> Action { get; set; }
        TArgument Argument { get; set; }

        #endregion

        #region ActionDisposer

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(Action != null) {
                    Action(disposing, Argument);
                    Action = null!;
                    Argument = default!;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    /// <summary>
    /// <see cref="ActionDisposer"/>, <see cref="ActionDisposer{TArgument}"/> の生成ヘルパー。
    /// </summary>
    public static class ActionDisposerHelper
    {
        #region define

        class EmptyDisposer: IDisposable
        {
            public void Dispose()
            {
                GC.SuppressFinalize(this);
            }
        }

        #endregion
        #region function

        public static ActionDisposer Create(Action<bool> action) => new ActionDisposer(action);
        public static ActionDisposer<TArgument> Create<TArgument>(Action<bool, TArgument> action, TArgument argument) => new ActionDisposer<TArgument>(action, argument);

        /// <summary>
        /// <see cref="IDisposable"/>とのIFを合わせるための空処理。
        /// </summary>
        /// <returns></returns>
        public static IDisposable CreateEmpty() => new EmptyDisposer();

        #endregion
    }

    public class DisposableStocker : DisposerBase
    {
        #region property

        IList<IDisposable> StockItems { get; } = new List<IDisposable>();

        #endregion

        #region function

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0601:Value type to reference type conversion causing boxing allocation")]
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

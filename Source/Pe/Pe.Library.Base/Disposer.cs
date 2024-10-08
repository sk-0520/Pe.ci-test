using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using ContentTypeTextNet.Pe.Library.Base.Linq;

namespace ContentTypeTextNet.Pe.Library.Base
{
    /// <summary>
    /// <see cref="IDisposable.Dispose"/>が行われたかどうかを確認できるようにする。
    /// </summary>
    public interface IDisposed
    {
        #region property

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>されたか。
        /// </summary>
        bool IsDisposed { get; }

        #endregion
    }

    /// <summary>
    /// <see cref="IDisposable"/> と <see cref="IDisposed"/> を組み合わせたIF。
    /// </summary>
    public interface IDisposer: IDisposed, IDisposable
    {
        #region event

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>時に呼び出されるイベント。
        /// </summary>
        /// <remarks>
        /// <para>呼び出し時点では<see cref="IDisposed.IsDisposed"/>は偽のまま。</para>
        /// </remarks>
        event EventHandler<EventArgs>? Disposing;

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

        #region property

        private WeakEvent<EventArgs> WeakDisposing { get; } = new WeakEvent<EventArgs>(nameof(Disposing));

        #endregion

        #region IDisposable

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>時に呼び出されるイベント。
        /// </summary>
        /// <remarks>
        /// <para>呼び出し時点では<see cref="IsDisposed"/>は偽のまま。</para>
        /// </remarks>
        public event EventHandler<EventArgs>? Disposing
        {
            add => WeakDisposing.Add(value);
            remove => WeakDisposing.Remove(value);
        }

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>されたか。
        /// </summary>
        [IgnoreDataMember, XmlIgnore]
        public bool IsDisposed { get; protected set; }

        /// <summary>
        /// 既に破棄済みの場合は処理を中断する。
        /// </summary>
        /// <exception cref="ObjectDisposedException">破棄済み。</exception>
        /// <seealso cref="IDisposed"/>
        protected void ThrowIfDisposed()
        {
            if(IsDisposed) {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }

        protected void OnDisposing()
        {
            WeakDisposing.Raise(this, EventArgs.Empty);
        }


        /// <summary>
        /// <see cref="IDisposable.Dispose"/>の内部処理。
        /// </summary>
        /// <remarks>
        /// <para>継承先クラスでは本メソッドを呼び出す必要がある。</para>
        /// </remarks>
        /// <param name="disposing">CLRの管理下か。</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose メソッドは、SuppressFinalize を呼び出す必要があります", Justification = "<保留中>")]
        protected virtual void Dispose(bool disposing)
        {
            if(IsDisposed) {
                return;
            }

            OnDisposing();

            IsDisposed = true;
        }


        /// <inheritdoc cref="IDisposable.Dispose"/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose メソッドは、SuppressFinalize を呼び出す必要があります")]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }

    /// <summary>
    /// その場で破棄する処理。
    /// </summary>
    /// <remarks>
    /// <para><c>using var xxx = new ActionDisposer(d => ...)</c>で実装する前提。</para>
    /// </remarks>
    public sealed class ActionDisposer: DisposerBase
    {
        public ActionDisposer(Action<bool> action)
        {
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }

        #region property

        private Action<bool>? Action { get; set; }

        #endregion

        #region ActionDisposer

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(Action != null) {
                    Action(disposing);
                    Action = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    /// <inheritdoc cref="ActionDisposer"/>
    public sealed class ActionDisposer<TArgument>: DisposerBase
    {
        public ActionDisposer(Action<bool, TArgument> action, TArgument argument)
        {
            Action = action ?? throw new ArgumentNullException(nameof(action));
            Argument = argument;
        }

        #region property

        private Action<bool, TArgument>? Action { get; set; }
        private TArgument Argument { get; set; }

        #endregion

        #region ActionDisposer

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(Action != null) {
                    Action(disposing, Argument);
                    Action = null;
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

        private sealed class EmptyDisposer: IDisposable
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

    /// <summary>
    /// <see cref="IDisposable"/> をまとめて保持する。
    /// </summary>
    /// <remarks>
    /// <para>破棄順序は後入れ先出になる。</para>
    /// </remarks>
    public sealed class DisposableStocker: DisposerBase
    {
        #region property

        private IList<IDisposable> StockItems { get; } = new List<IDisposable>();

        #endregion

        #region function

        /// <summary>
        /// 破棄対象として追加。
        /// </summary>
        /// <typeparam name="TDisposable"></typeparam>
        /// <param name="disposable"></param>
        /// <returns>追加したデータ。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0601:Value type to reference type conversion causing boxing allocation")]
        public TDisposable Add<TDisposable>(TDisposable disposable)
            where TDisposable : IDisposable
        {
            if(disposable is null) {
                throw new ArgumentNullException(nameof(disposable));
            }

            ThrowIfDisposed();

            StockItems.Add(disposable);

            return disposable;
        }

        /// <summary>
        /// 破棄対象として集合を追加。
        /// </summary>
        /// <param name="disposables"></param>
        public void AddRange(IEnumerable<IDisposable> disposables)
        {
            if(disposables == null) {
                throw new ArgumentNullException(nameof(disposables));
            }

            ThrowIfDisposed();

            StockItems.AddRange(disposables);
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    for(var i = StockItems.Count - 1; 0 <= i; i--) {
                        StockItems[i].Dispose();
                    }
                    StockItems.Clear();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    /// <summary>
    /// <see cref="ArrayPool{T}"/> のラッパー。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly ref struct ArrayPoolValue<T>
    {
        public ArrayPoolValue(int length)
            : this(length, ArrayPool<T>.Shared)
        { }

        public ArrayPoolValue(int length, ArrayPool<T> pool)
        {
            Pool = pool;
            Items = Pool.Rent(length);
            Length = length;
        }

        #region property

        private ArrayPool<T> Pool { get; }
        public T[] Items { get; }

        public int Length { get; }

        #endregion

        #region function

        public ref T this[int index] => ref Items[index];

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Pool.Return(Items);
        }

        #endregion
    }

    /// <summary>
    /// <see cref="ArrayPool{T}"/> のラッパー。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ArrayPoolObject<T>: DisposerBase
    {
        public ArrayPoolObject(int length)
            : this(length, ArrayPool<T>.Shared)
        { }

        public ArrayPoolObject(int length, ArrayPool<T> pool)
        {
            Pool = pool;
            Items = Pool.Rent(length);
            Length = length;
        }

        #region property

        private ArrayPool<T> Pool { get; }
        public T[] Items { get; }

        public int Length { get; }

        #endregion

        #region function

        public ref T this[int index] => ref Items[index];

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Pool.Return(Items);
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}

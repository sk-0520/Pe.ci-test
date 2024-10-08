using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace ContentTypeTextNet.Pe.Library.Base
{
    /// <summary>
    /// <see cref="ReaderWriterLockSlim"/>のラッパー。
    /// </summary>
    public interface IReadWriteLockHelper
    {
        #region property

        /// <summary>
        /// 標準の読み込み待ち時間。
        /// </summary>
        public TimeSpan DefaultReadTimeout { get; }
        /// <summary>
        /// 標準の更新待ち時間。
        /// </summary>
        public TimeSpan DefaultUpdateTimeout { get; }
        /// <summary>
        /// 標準の書き込み待ち時間。
        /// </summary>
        public TimeSpan DefaultWriteTimeout { get; }

        #endregion

        #region function

        /// <summary>
        /// 読み込みロックを即時開始。
        /// </summary>
        /// <returns>ロック解除用オブジェクト。</returns>
        public IDisposable BeginRead();

        /// <summary>
        /// 更新ロックを即時開始。
        /// </summary>
        /// <returns>ロック解除用オブジェクト。</returns>
        public IDisposable BeginUpdate();

        /// <summary>
        /// 書き込みロックを即時開始。
        /// </summary>
        /// <returns>ロック解除用オブジェクト。</returns>
        public IDisposable BeginWrite();

        /// <summary>
        /// 読み込みロックを待機実行。
        /// </summary>
        /// <param name="timeout">待機時間。</param>
        /// <param name="lockedAction">ロック中処理。</param>
        /// <returns>ロック成功状態。</returns>
        public bool TryRead(TimeSpan timeout, Action lockedAction);

        /// <summary>
        /// 標準待ち時間を使用した読み込みロックを待機実行。
        /// </summary>
        /// <param name="lockedAction">ロック中処理。</param>
        /// <returns>ロック成功状態。</returns>
        public bool TryReadByDefaultTimeout(Action lockedAction);

        /// <summary>
        /// 更新ロックを待機実行。
        /// </summary>
        /// <param name="timeout">待機時間。</param>
        /// <param name="lockedAction">ロック中処理。</param>
        /// <returns>ロック成功状態。</returns>
        public bool TryUpdate(TimeSpan timeout, Action lockedAction);

        /// <summary>
        /// 標準待ち時間を使用した更新ロックを待機実行。
        /// </summary>
        /// <param name="lockedAction">ロック中処理。</param>
        /// <returns>ロック成功状態。</returns>
        public bool TryUpdateByDefaultTimeout(Action lockedAction);

        /// <summary>
        /// 書き込みロックを待機実行。
        /// </summary>
        /// <param name="timeout">待機時間。</param>
        /// <param name="lockedAction">ロック中処理。</param>
        /// <returns>ロック成功状態。</returns>
        public bool TryWrite(TimeSpan timeout, Action lockedAction);

        /// <summary>
        /// 標準待ち時間を使用した書き込みロックを待機実行。
        /// </summary>
        /// <param name="lockedAction">ロック中処理。</param>
        /// <returns>ロック成功状態。</returns>
        public bool TryWriteByDefaultTimeout(Action lockedAction);

        /// <summary>
        /// 読み込みロックを待機し、ロックを取得できない場合に例外処理。
        /// </summary>
        /// <param name="timeout">待機時間。</param>
        /// <returns>ロック解除用オブジェクト。</returns>
        public IDisposable WaitRead(TimeSpan timeout);

        /// <summary>
        /// 標準待ち時間を使用した読み込みロックを待機し、ロックを取得できない場合に例外処理。
        /// </summary>
        /// <param name="timeout">待機時間。</param>
        /// <returns>ロック解除用オブジェクト。</returns>
        public IDisposable WaitReadByDefaultTimeout();

        /// <summary>
        /// 更新ロックを待機し、ロックを取得できない場合に例外処理。
        /// </summary>
        /// <param name="timeout">待機時間。</param>
        /// <returns>ロック解除用オブジェクト。</returns>
        public IDisposable WaitUpdate(TimeSpan timeout);

        /// <summary>
        /// 標準待ち時間を使用した更新ロックを待機し、ロックを取得できない場合に例外処理。
        /// </summary>
        /// <param name="timeout">待機時間。</param>
        /// <returns>ロック解除用オブジェクト。</returns>
        public IDisposable WaitUpdateByDefaultTimeout();

        /// <summary>
        /// 書き込みロックを待機し、ロックを取得できない場合に例外処理。
        /// </summary>
        /// <param name="timeout">待機時間。</param>
        /// <returns>ロック解除用オブジェクト。</returns>
        public IDisposable WaitWrite(TimeSpan timeout);

        /// <summary>
        /// 標準待ち時間を使用した書き込みロックを待機し、ロックを取得できない場合に例外処理。
        /// </summary>
        /// <param name="timeout">待機時間。</param>
        /// <returns>ロック解除用オブジェクト。</returns>
        public IDisposable WaitWriteByDefaultTimeout();

        #endregion
    }

    /// <inheritdoc cref="IReadWriteLockHelper"/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0603:Delegate allocation from a method group")]
    public class ReadWriteLockHelper: DisposerBase, IReadWriteLockHelper
    {
        /// <summary>
        /// 再帰ロック不可で作成。
        /// </summary>
        /// <remarks>
        /// <para>通常はこっちでいいはず。</para>
        /// </remarks>
        public ReadWriteLockHelper()
        {
            Locker = new ReaderWriterLockSlim();
        }

        /// <summary>
        /// 再帰指定でロック指定で作成。
        /// </summary>
        /// <param name="recursionPolicy"></param>
        public ReadWriteLockHelper(LockRecursionPolicy recursionPolicy)
        {
            Locker = new ReaderWriterLockSlim(recursionPolicy);
        }

        #region property

        private ReaderWriterLockSlim Locker { get; set; }

        #endregion

        #region function

        /// <summary>
        /// ロックを即時開始。
        /// </summary>
        /// <param name="beginAction">ロック処理。</param>
        /// <param name="exitAction">ロック解除処理。</param>
        /// <returns>ロック解除用オブジェクト。</returns>
        protected IDisposable BeginCore(Action beginAction, Action exitAction)
        {
            Debug.Assert(beginAction != null);
            Debug.Assert(exitAction != null);
            ThrowIfDisposed();

            beginAction();
            return new ActionDisposer<Action>((disposing, action) => {
                if(disposing) {
                    action();
                }
            }, exitAction);
        }

        /// <summary>
        /// ロックを待機実行。
        /// </summary>
        /// <param name="timeout">待機時間。</param>
        /// <param name="lockedAction">ロック中処理。</param>
        /// <param name="tryAction">ロック処理。</param>
        /// <param name="exitAction">ロック解除処理。</param>
        /// <returns>ロック成功状態。</returns>
        protected bool TryCore(TimeSpan timeout, Action lockedAction, Func<TimeSpan, bool> tryAction, Action exitAction)
        {
            Debug.Assert(lockedAction != null);
            Debug.Assert(tryAction != null);
            Debug.Assert(exitAction != null);
            ThrowIfDisposed();

            if(tryAction(timeout)) {
                try {
                    lockedAction();
                } finally {
                    exitAction();
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// ロックを待機し、ロックを取得できない場合に例外処理。
        /// </summary>
        /// <param name="timeout">待機時間</param>
        /// <param name="tryAction">ロック処理。</param>
        /// <param name="exitAction">ロック解除処理。</param>
        /// <returns>ロック解除用オブジェクト。</returns>
        protected IDisposable WaitCore(TimeSpan timeout, Func<TimeSpan, bool> tryAction, Action exitAction)
        {
            Debug.Assert(tryAction != null);
            Debug.Assert(exitAction != null);
            ThrowIfDisposed();

            if(tryAction(timeout)) {
                return new ActionDisposer<Action>((disposing, action) => {
                    if(disposing) {
                        action();
                    }
                }, exitAction);
            }

            throw new SynchronizationLockException();
        }

        #endregion

        #region IReaderWriterLocker

        /// <inheritdoc cref="IReadWriteLockHelper"/>
        public virtual TimeSpan DefaultReadTimeout { get; set; } = TimeSpan.FromSeconds(5);
        /// <inheritdoc cref="IReadWriteLockHelper"/>
        public virtual TimeSpan DefaultUpdateTimeout { get; set; } = TimeSpan.FromSeconds(3);
        /// <inheritdoc cref="IReadWriteLockHelper"/>
        public virtual TimeSpan DefaultWriteTimeout { get; set; } = TimeSpan.FromSeconds(3);


        /// <inheritdoc cref="IReadWriteLockHelper.BeginRead"/>
        public IDisposable BeginRead()
        {
            ThrowIfDisposed();

            return BeginCore(Locker.EnterReadLock, Locker.ExitReadLock);
        }


        /// <inheritdoc cref="IReadWriteLockHelper.BeginUpdate"/>
        public IDisposable BeginUpdate()
        {
            ThrowIfDisposed();

            // write系と何が違うのか分からん
            return BeginCore(Locker.EnterUpgradeableReadLock, Locker.ExitUpgradeableReadLock);
        }

        /// <inheritdoc cref="IReadWriteLockHelper.BeginWrite"/>
        public IDisposable BeginWrite()
        {
            ThrowIfDisposed();

            return BeginCore(Locker.EnterWriteLock, Locker.ExitWriteLock);
        }

        /// <inheritdoc cref="IReadWriteLockHelper.TryRead(TimeSpan, Action)"/>
        public bool TryRead(TimeSpan timeout, Action lockedAction)
        {
            if(lockedAction == null) {
                throw new ArgumentNullException(nameof(lockedAction));
            }
            ThrowIfDisposed();

            return TryCore(timeout, lockedAction, Locker.TryEnterReadLock, Locker.ExitReadLock);
        }

        /// <inheritdoc cref="IReadWriteLockHelper.TryReadByDefaultTimeout(Action)"/>
        public bool TryReadByDefaultTimeout(Action lockedAction)
        {
            ThrowIfDisposed();

            return TryUpdate(DefaultReadTimeout, lockedAction);
        }

        /// <inheritdoc cref="IReadWriteLockHelper.TryUpdate(TimeSpan, Action)"/>
        public bool TryUpdate(TimeSpan timeout, Action lockedAction)
        {
            if(lockedAction == null) {
                throw new ArgumentNullException(nameof(lockedAction));
            }
            ThrowIfDisposed();

            return TryCore(timeout, lockedAction, Locker.TryEnterUpgradeableReadLock, Locker.ExitUpgradeableReadLock);
        }

        /// <inheritdoc cref="IReadWriteLockHelper.TryUpdateByDefaultTimeout(Action)"/>
        public bool TryUpdateByDefaultTimeout(Action lockedAction)
        {
            ThrowIfDisposed();

            return TryUpdate(DefaultUpdateTimeout, lockedAction);
        }

        /// <inheritdoc cref="IReadWriteLockHelper.TryWrite(TimeSpan, Action)"/>
        public bool TryWrite(TimeSpan timeout, Action lockedAction)
        {
            if(lockedAction == null) {
                throw new ArgumentNullException(nameof(lockedAction));
            }
            ThrowIfDisposed();

            return TryCore(timeout, lockedAction, Locker.TryEnterWriteLock, Locker.ExitWriteLock);
        }

        /// <inheritdoc cref="IReadWriteLockHelper.TryWriteByDefaultTimeout(Action)"/>
        public bool TryWriteByDefaultTimeout(Action lockedAction)
        {
            ThrowIfDisposed();

            return TryWrite(DefaultWriteTimeout, lockedAction);
        }

        /// <inheritdoc cref="IReadWriteLockHelper.WaitRead(TimeSpan)"/>
        public IDisposable WaitRead(TimeSpan timeout)
        {
            ThrowIfDisposed();

            return WaitCore(timeout, Locker.TryEnterReadLock, Locker.ExitReadLock);
        }

        /// <inheritdoc cref="IReadWriteLockHelper.WaitReadByDefaultTimeout"/>
        public IDisposable WaitReadByDefaultTimeout()
        {
            ThrowIfDisposed();

            return WaitRead(DefaultReadTimeout);
        }

        /// <inheritdoc cref="IReadWriteLockHelper.WaitUpdate(TimeSpan)"/>
        public IDisposable WaitUpdate(TimeSpan timeout)
        {
            ThrowIfDisposed();

            return WaitCore(timeout, Locker.TryEnterUpgradeableReadLock, Locker.ExitUpgradeableReadLock);
        }

        /// <inheritdoc cref="IReadWriteLockHelper.WaitUpdateByDefaultTimeout"/>
        public IDisposable WaitUpdateByDefaultTimeout()
        {
            ThrowIfDisposed();

            return WaitUpdate(DefaultUpdateTimeout);
        }

        /// <inheritdoc cref="IReadWriteLockHelper.WaitWrite(TimeSpan)"/>
        public IDisposable WaitWrite(TimeSpan timeout)
        {
            ThrowIfDisposed();

            return WaitCore(timeout, Locker.TryEnterWriteLock, Locker.ExitWriteLock);
        }

        /// <inheritdoc cref="IReadWriteLockHelper.WaitWriteByDefaultTimeout"/>
        public IDisposable WaitWriteByDefaultTimeout()
        {
            ThrowIfDisposed();

            return WaitWrite(DefaultWriteTimeout);
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    Locker.Dispose();
                    Locker = null!;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}

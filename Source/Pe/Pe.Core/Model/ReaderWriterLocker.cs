using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Text;
using System.Threading;
using ContentTypeTextNet.Pe.Core.Model;

namespace ContentTypeTextNet.Pe.Core.Model
{
    /// <summary>
    /// <see cref="ReaderWriterLockSlim"/>のラッパー。
    /// </summary>
    public class ReaderWriterLocker : DisposerBase
    {
        /// <summary>
        /// 再帰ロック不可で作成。
        /// </summary>
        public ReaderWriterLocker()
        {
            Locker = new ReaderWriterLockSlim();
        }

        /// <summary>
        /// 再帰指定でロック指定で作成。
        /// </summary>
        /// <param name="recursionPolicy"></param>
        public ReaderWriterLocker(LockRecursionPolicy recursionPolicy)
        {
            Locker = new ReaderWriterLockSlim(recursionPolicy);
        }

        #region property

        ReaderWriterLockSlim Locker { get; set; }

        /// <summary>
        /// 標準の読み込み待ち時間。
        /// </summary>
        public virtual TimeSpan DefaultReadTimeout { get; set; } = TimeSpan.FromSeconds(5);
        /// <summary>
        /// 標準の更新待ち時間。
        /// </summary>
        public virtual TimeSpan DefaultUpdateTimeout { get; set; } = TimeSpan.FromSeconds(3);
        /// <summary>
        /// 標準の書き込み待ち時間。
        /// </summary>
        public virtual TimeSpan DefaultWriteTimeout { get; set; } = TimeSpan.FromSeconds(3);

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

            beginAction();
            return new ActionDisposer(disposing => {
                if(disposing) {
                    exitAction();
                }
            });
        }

        /// <summary>
        /// 読み込みロックを即時開始。
        /// </summary>
        /// <returns>ロック解除用オブジェクト。</returns>
        public IDisposable BeginRead()
        {
            return BeginCore(Locker.EnterReadLock, Locker.ExitReadLock);
        }

        /// <summary>
        /// 更新ロックを即時開始。
        /// </summary>
        /// <returns>ロック解除用オブジェクト。</returns>
        public IDisposable BeginUpdate()
        {
            // write系と何が違うのか分からん
            return BeginCore(Locker.EnterUpgradeableReadLock, Locker.ExitUpgradeableReadLock);
        }

        /// <summary>
        /// 書き込みロックを即時開始。
        /// </summary>
        /// <returns>ロック解除用オブジェクト。</returns>
        public IDisposable BeginWrite()
        {
            return BeginCore(Locker.EnterWriteLock, Locker.ExitWriteLock);
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
        /// 読み込みロックを待機実行。
        /// </summary>
        /// <param name="timeout">待機時間。</param>
        /// <param name="lockedAction">ロック中処理。</param>
        /// <returns>ロック成功状態。</returns>
        public bool TryRead(TimeSpan timeout, Action lockedAction)
        {
            if(lockedAction == null) {
                throw new ArgumentNullException(nameof(lockedAction));
            }

            return TryCore(timeout, lockedAction, Locker.TryEnterReadLock, Locker.ExitReadLock);
        }

        /// <summary>
        /// 標準待ち時間を使用した読み込みロックを待機実行。
        /// </summary>
        /// <param name="lockedAction">ロック中処理。</param>
        /// <returns>ロック成功状態。</returns>
        public bool TryReadByDefaultTimeout(Action lockedAction)
        {
            return TryUpdate(DefaultReadTimeout, lockedAction);
        }

        /// <summary>
        /// 更新ロックを待機実行。
        /// </summary>
        /// <param name="timeout">待機時間。</param>
        /// <param name="lockedAction">ロック中処理。</param>
        /// <returns>ロック成功状態。</returns>
        public bool TryUpdate(TimeSpan timeout, Action lockedAction)
        {
            if(lockedAction == null) {
                throw new ArgumentNullException(nameof(lockedAction));
            }

            return TryCore(timeout, lockedAction, Locker.TryEnterUpgradeableReadLock, Locker.ExitUpgradeableReadLock);
        }

        /// <summary>
        /// 標準待ち時間を使用した更新ロックを待機実行。
        /// </summary>
        /// <param name="lockedAction">ロック中処理。</param>
        /// <returns>ロック成功状態。</returns>
        public bool TryUpdateByDefaultTimeout(Action lockedAction)
        {
            return TryUpdate(DefaultUpdateTimeout, lockedAction);
        }

        /// <summary>
        /// 書き込みロックを待機実行。
        /// </summary>
        /// <param name="timeout">待機時間。</param>
        /// <param name="lockedAction">ロック中処理。</param>
        /// <returns>ロック成功状態。</returns>
        public bool TryWrite(TimeSpan timeout, Action lockedAction)
        {
            if(lockedAction == null) {
                throw new ArgumentNullException(nameof(lockedAction));
            }

            return TryCore(timeout, lockedAction, Locker.TryEnterWriteLock, Locker.ExitWriteLock);
        }

        /// <summary>
        /// 標準待ち時間を使用した書き込みロックを待機実行。
        /// </summary>
        /// <param name="lockedAction">ロック中処理。</param>
        /// <returns>ロック成功状態。</returns>
        public bool TryWriteByDefaultTimeout(Action lockedAction)
        {
            return TryWrite(DefaultWriteTimeout, lockedAction);
        }

        /// <summary>
        /// ロックを待機し、ロックを取得できない場合に例外処理。
        /// </summary>
        /// <param name="timeout">待機時間</param>
        /// <param name="tryAction">ロック処理。</param>
        /// <param name="exitAction">ロック解除処理。</param>
        /// <returns>ロック解除用オブジェクト。</returns>
        public IDisposable WaitCore(TimeSpan timeout, Func<TimeSpan, bool> tryAction, Action exitAction)
        {
            Debug.Assert(tryAction != null);
            Debug.Assert(exitAction != null);

            if(tryAction(timeout)) {
                return new ActionDisposer(disposing => {
                    if(disposing) {
                        exitAction();
                    }
                });
            }

            throw new SynchronizationLockException();
        }

        /// <summary>
        /// 読み込みロックを待機し、ロックを取得できない場合に例外処理。
        /// </summary>
        /// <param name="timeout">待機時間。</param>
        /// <returns>ロック解除用オブジェクト。</returns>
        public IDisposable WaitRead(TimeSpan timeout)
        {
            return WaitCore(timeout, Locker.TryEnterReadLock, Locker.ExitReadLock);
        }

        /// <summary>
        /// 標準待ち時間を使用した読み込みロックを待機し、ロックを取得できない場合に例外処理。
        /// </summary>
        /// <param name="timeout">待機時間。</param>
        /// <returns>ロック解除用オブジェクト。</returns>
        public IDisposable WaitReadByDefaultTimeout()
        {
            return WaitRead(DefaultReadTimeout);
        }

        /// <summary>
        /// 更新ロックを待機し、ロックを取得できない場合に例外処理。
        /// </summary>
        /// <param name="timeout">待機時間。</param>
        /// <returns>ロック解除用オブジェクト。</returns>
        public IDisposable WaitUpdate(TimeSpan timeout)
        {
            return WaitCore(timeout, Locker.TryEnterUpgradeableReadLock, Locker.ExitUpgradeableReadLock);
        }

        /// <summary>
        /// 標準待ち時間を使用した更新ロックを待機し、ロックを取得できない場合に例外処理。
        /// </summary>
        /// <param name="timeout">待機時間。</param>
        /// <returns>ロック解除用オブジェクト。</returns>
        public IDisposable WaitUpdateByDefaultTimeout()
        {
            return WaitUpdate(DefaultUpdateTimeout);
        }

        /// <summary>
        /// 書き込みロックを待機し、ロックを取得できない場合に例外処理。
        /// </summary>
        /// <param name="timeout">待機時間。</param>
        /// <returns>ロック解除用オブジェクト。</returns>
        public IDisposable WaitWrite(TimeSpan timeout)
        {
            return WaitCore(timeout, Locker.TryEnterWriteLock, Locker.ExitWriteLock);
        }

        /// <summary>
        /// 標準待ち時間を使用した書き込みロックを待機し、ロックを取得できない場合に例外処理。
        /// </summary>
        /// <param name="timeout">待機時間。</param>
        /// <returns>ロック解除用オブジェクト。</returns>
        public IDisposable WaitWriteByDefaultTimeout()
        {
            return WaitWrite(DefaultWriteTimeout);
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    Locker.Dispose();
#pragma warning disable CS8625 // null リテラルを null 非許容参照型に変換できません。
                    Locker = null;
#pragma warning restore CS8625 // null リテラルを null 非許容参照型に変換できません。
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}

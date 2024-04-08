using System;

namespace ContentTypeTextNet.Pe.Standard.Database
{
    /// <summary>
    /// データベースに対する読み書き制御。
    /// <para>NOTE: 役割が完全にSQLiteに合わせた挙動。</para>
    /// </summary>
    public interface IDatabaseBarrier
    {
        #region function

        /// <summary>
        /// 既定の待機時間で書き込み処理を実施する。
        /// </summary>
        /// <returns></returns>
        IDatabaseTransaction WaitWrite();
        /// <summary>
        /// 指定の待機時間で書き込み処理を実施する。
        /// </summary>
        /// <param name="timeout">待機時間。</param>
        /// <returns></returns>
        IDatabaseTransaction WaitWrite(TimeSpan timeout);

        /// <summary>
        /// 既定の待機時間で読み込み処理を実施する。
        /// </summary>
        /// <returns></returns>
        IDatabaseTransaction WaitRead();
        /// <summary>
        /// 指定の待機時間で読み込み処理を実施する。
        /// </summary>
        /// <param name="timeout">待機時間。</param>
        /// <returns></returns>
        IDatabaseTransaction WaitRead(TimeSpan timeout);


        #endregion
    }
}

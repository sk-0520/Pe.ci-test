using System;

namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <summary>
    /// データベースに対する読み書き制御。
    /// </summary>
    /// <remarks>
    /// <para>NOTE: 役割が完全にSQLiteに合わせた挙動。</para>
    /// </remarks>
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

using System;

namespace ContentTypeTextNet.Pe.Library.Database
{
    // これもういわらんわ
    public static class IDatabaseBarrierExtensions
    {
        #region function

        /// <summary>
        /// データ読み込み。
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="databaseBarrier"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static TResult ReadData<TResult>(this IDatabaseBarrier databaseBarrier, Func<IDatabaseTransaction, TResult> func)
        {
            using var transaction = databaseBarrier.WaitRead();
            return func(transaction);
        }

        /// <summary>
        /// データ読み込み。
        /// </summary>
        /// <remarks>
        /// <para>パラメータ付き。</para>
        /// </remarks>
        /// <typeparam name="TArgument"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="databaseBarrier"></param>
        /// <param name="func"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        public static TResult ReadData<TArgument, TResult>(this IDatabaseBarrier databaseBarrier, Func<IDatabaseTransaction, TArgument, TResult> func, TArgument argument)
        {
            using var transaction = databaseBarrier.WaitRead();
            return func(transaction, argument);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Base
{
    /// <summary>
    /// 結果データ。
    /// </summary>
    public interface IResult
    {
        #region property

        /// <summary>
        /// 結果は成功したか。
        /// </summary>
        bool Success { get; }

        #endregion
    }

    /// <summary>
    /// 成功結果データ。
    /// </summary>
    /// <typeparam name="TSuccess"></typeparam>
    public interface IResultSuccess<out TSuccess>: IResult
    {
        #region property

        /// <summary>
        /// 成功結果。
        /// </summary>
        [AllowNull]
        TSuccess SuccessValue { get; }

        #endregion
    }

    /// <summary>
    /// 失敗結果データ。
    /// </summary>
    /// <typeparam name="TFailure"></typeparam>
    public interface IResultFailure<out TFailure>: IResult
    {
        #region property

        /// <summary>
        /// 失敗結果。
        /// </summary>
        [AllowNull]
        TFailure FailureValue { get; }

        #endregion
    }

    /// <summary>
    /// <see cref="IResult"/> 実体。
    /// </summary>
    public readonly struct Result: IResult
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="success">成功状態。</param>
        public Result(bool success)
        {
            Success = success;
        }

        #region IResult

        public bool Success { get; }

        #endregion

        #region function

        /// <summary>
        /// 成功状態の生成。
        /// </summary>
        /// <typeparam name="TSuccess"></typeparam>
        /// <param name="successValue">成功データ。</param>
        /// <returns>成功データ。</returns>
        public static ResultSuccess<TSuccess> CreateSuccess<TSuccess>([AllowNull] TSuccess successValue)
        {
            return new ResultSuccess<TSuccess>(true, successValue);
        }

        /// <summary>
        /// 非成功状態の生成。
        /// </summary>
        /// <typeparam name="TSuccess"></typeparam>
        /// <returns>失敗データ。</returns>
        public static ResultSuccess<TSuccess> CreateFailure<TSuccess>()
        {
            return new ResultSuccess<TSuccess>(false, default);
        }

        /// <summary>
        /// 失敗状態の生成。
        /// </summary>
        /// <typeparam name="TFailure"></typeparam>
        /// <returns>失敗データ。</returns>
        public static ResultFailure<TFailure> CreateSuccess<TFailure>()
        {
            return new ResultFailure<TFailure>(true, default);
        }

        /// <summary>
        /// 失敗状態の生成。
        /// </summary>
        /// <typeparam name="TFailure"></typeparam>
        /// <param name="failureValue"></param>
        /// <returns>失敗データ。</returns>
        public static ResultFailure<TFailure> CreateFailure<TFailure>([AllowNull] TFailure failureValue)
        {
            return new ResultFailure<TFailure>(false, failureValue);
        }

        #endregion
    }

    /// <summary>
    /// <see cref="IResultSuccess{TSuccess}"/> 実体。
    /// </summary>
    public readonly struct ResultSuccess<TSuccess>: IResultSuccess<TSuccess>
    {
        public ResultSuccess(bool success, [AllowNull] TSuccess successValue)
        {
            Success = success;
            SuccessValue = successValue;
        }

        #region property

        public bool Success { get; }
        public Type SuccessType => typeof(TSuccess);
        [AllowNull]
        public TSuccess SuccessValue { get; }

        #endregion
    }

    /// <summary>
    /// <see cref="IResultFailure{TFailure}"/> 実体。
    /// </summary>
    public readonly struct ResultFailure<TFailure>: IResultFailure<TFailure>
    {
        public ResultFailure(bool success, [AllowNull] TFailure failureValue)
        {
            Success = success;
            FailureValue = failureValue;
        }

        #region property

        public bool Success { get; }
        public Type FailureType => typeof(TFailure);
        [AllowNull]
        public TFailure FailureValue { get; }

        #endregion
    }

    /// <summary>
    /// <see cref="IResultSuccess{TSuccess}"/>/<see cref="IResultFailure{TFailure}"/> 実体。
    /// </summary>
    public readonly struct Result<TSuccess, TFailure>: IResultSuccess<TSuccess>, IResultFailure<TFailure>
    {
        public Result(bool success, [AllowNull] TSuccess successValue, [AllowNull] TFailure failureValue)
        {
            Success = success;
            SuccessValue = successValue;
            FailureValue = failureValue;
        }

        #region property

        public bool Success { get; }

        public Type SuccessType => typeof(TSuccess);
        [AllowNull]
        public TSuccess SuccessValue { get; }

        public Type FailureType => typeof(TFailure);
        [AllowNull]
        public TFailure FailureValue { get; }

        #endregion
    }
}

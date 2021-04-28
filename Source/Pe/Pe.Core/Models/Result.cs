using System;
using System.Diagnostics.CodeAnalysis;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public interface IResult
    {
        #region property

        bool Success { get; }

        #endregion
    }

    public interface IResultSuccessValue: IResult
    {
        #region property

        Type? SuccessType { get; }

        #endregion
    }

    public interface IResultFailureValue: IResult
    {
        #region property

        Type? FailureType { get; }

        #endregion
    }

    public interface IResultSuccessValue<out TSuccess>: IResultSuccessValue
    {
        #region property

        [AllowNull]
        TSuccess SuccessValue { get; }

        #endregion
    }

    public interface IResultFailureValue<out TFailure>: IResultFailureValue
    {
        #region property

        [AllowNull]
        TFailure FailureValue { get; }

        #endregion
    }

    public interface IResultValue: IResultSuccessValue, IResultFailureValue
    { }

    public interface IResultValue<TSuccess, TFailure>: IResultSuccessValue<TSuccess>, IResultFailureValue<TFailure>, IResultValue
    { }

    public struct Result: IResult
    {
        public Result(bool success)
        {
            Success = success;
        }

        #region IResult

        public bool Success { get; }

        #endregion
    }

    public struct ResultSuccessValue<TSuccess>: IResultSuccessValue<TSuccess>
    {
        public ResultSuccessValue(bool success, [AllowNull] TSuccess successValue)
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

    public static class ResultSuccessValue
    {
        #region function

        public static ResultSuccessValue<TSuccess> Success<TSuccess>([AllowNull] TSuccess successValue)
        {
            return new ResultSuccessValue<TSuccess>(true, successValue);
        }

        public static ResultSuccessValue<TSuccess> Failure<TSuccess>()
        {
            return new ResultSuccessValue<TSuccess>(false, default);
        }

        #endregion

    }

    public struct ResultFailureValue<TFailure>: IResultFailureValue<TFailure>
    {
        public ResultFailureValue(bool success, [AllowNull] TFailure failureValue)
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

    public static class ResultFailureValue
    {
        #region function

        public static ResultFailureValue<TFailure> Success<TFailure>()
        {
            return new ResultFailureValue<TFailure>(true, default);
        }

        public static ResultFailureValue<TFailure> Failure<TFailure>([AllowNull] TFailure failureValue)
        {
            return new ResultFailureValue<TFailure>(false, failureValue);
        }

        #endregion

    }

    public struct ResultValue<TSuccess, TFailure>: IResultValue<TSuccess, TFailure>
    {
        public ResultValue(bool success, [AllowNull] TSuccess successValue, [AllowNull] TFailure failureValue)
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

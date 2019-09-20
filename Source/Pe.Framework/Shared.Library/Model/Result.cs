using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// タプルでいいじゃんと思う今日この頃。

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
    public interface IResult
    {
        #region property

        bool Success { get; }

        #endregion
    }

    public interface IResultSuccessValue : IResult
    {
        #region property

        Type SuccessType { get; }

        #endregion
    }

    public interface IResultFailureValue : IResult
    {
        #region property

        Type FailureType { get; }

        #endregion
    }

    public interface IResultSuccessValue<TSuccess> : IResultSuccessValue
    {
        #region property

        TSuccess SuccessValue { get; }

        #endregion
    }

    public interface IResultFailureValue<TFailure> : IResultFailureValue
    {
        #region property

        TFailure FailureValue { get; }

        #endregion
    }

    public interface IResultValue : IResultSuccessValue, IResultFailureValue
    { }

    public interface IResultValue<TSuccess, TFailure> : IResultSuccessValue<TSuccess>, IResultFailureValue<TFailure>, IResultValue
    { }

    public struct Result : IResult
    {
        public Result(bool success)
        {
            Success = success;
        }

        #region IResult

        public bool Success { get; }

        #endregion
    }

    public struct ResultSuccessValue<TSuccess> : IResultSuccessValue<TSuccess>
    {
        public ResultSuccessValue(bool success, TSuccess successValue)
        {
            Success = success;
            SuccessValue = successValue;
        }

        #region property

        public bool Success { get; }
        public Type SuccessType => typeof(TSuccess);
        public TSuccess SuccessValue { get; }

        #endregion
    }

    public static class ResultSuccessValue
    {
        #region function

        public static ResultSuccessValue<TSuccess> Success<TSuccess>(TSuccess successValue)
        {
            return new ResultSuccessValue<TSuccess>(true, successValue);
        }

        public static ResultSuccessValue<TSuccess> Failure<TSuccess>()
        {
            return new ResultSuccessValue<TSuccess>(false, default(TSuccess));
        }

        #endregion

    }

    public struct ResultFailureValue<TFailure> : IResultFailureValue<TFailure>
    {
        public ResultFailureValue(bool success, TFailure failureValue)
        {
            Success = success;
            FailureValue = failureValue;
        }

        #region property

        public bool Success { get; }
        public Type FailureType => typeof(TFailure);
        public TFailure FailureValue { get; }

        #endregion
    }

    public static class ResultFailureValue
    {
        #region function

        public static ResultFailureValue<TFailure> Success<TFailure>()
        {
            return new ResultFailureValue<TFailure>(true, default(TFailure));
        }

        public static ResultFailureValue<TFailure> Failure<TFailure>(TFailure failureValue)
        {
            return new ResultFailureValue<TFailure>(false, failureValue);
        }

        #endregion

    }

    public struct ResultValue<TSuccess, TFailure>: IResultValue<TSuccess, TFailure>
    {
        public ResultValue(bool success, TSuccess successValue, TFailure failureValue)
        {
            Success = success;
            SuccessValue = successValue;
            FailureValue = failureValue;
        }

        #region property

        public bool Success { get; }

        public Type SuccessType => typeof(TSuccess);
        public TSuccess SuccessValue { get; }

        public Type FailureType => typeof(TFailure);
        public TFailure FailureValue { get; }

        #endregion
    }
}

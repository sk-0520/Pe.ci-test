using System;
using System.Diagnostics;
using System.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Models
{
    [Obsolete]
    public static class CastUtility
    {
        /// <summary>
        /// オブジェクトがキャスト出来た場合に処理を行う。
        /// </summary>
        /// <typeparam name="TCast"></typeparam>
        /// <param name="arg"></param>
        /// <param name="action"></param>
        [Obsolete]
        public static bool AsAction<TCast>(object arg, Action<TCast> action)
            where TCast : class
        {
            if(arg is TCast obj) {
                action(obj);
                return true;
            }
            return false;
        }

        /// <summary>
        /// オブジェクトがキャスト出来た場合に処理を行い、結果を返す。
        /// </summary>
        /// <typeparam name="TCast"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="arg"></param>
        /// <param name="func"></param>
        /// <param name="castErrorResult">キャスト失敗時の戻り値。</param>
        /// <returns></returns>
        public static TResult AsFunc<TCast, TResult>(object arg, Func<TCast, TResult> func, TResult castErrorResult)
            where TCast : class
        {
            if(arg is TCast obj) {
                return func(obj);
            }
            return castErrorResult;
        }

        /// <summary>
        /// オブジェクトがキャスト出来た場合に処理を行い、結果を返す。
        /// </summary>
        /// <typeparam name="TCast"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="arg"></param>
        /// <param name="func"></param>
        /// <returns>キャスト失敗時はTResultの初期値。</returns>
        public static TResult AsFunc<TCast, TResult>(object arg, Func<TCast, TResult> func)
            where TCast : class
        {
            return AsFunc<TCast, TResult>(arg, func, default!);
        }

        /// <summary>
        /// キャスト値をキャストに成功・失敗に関わらず取得する。
        /// </summary>
        /// <typeparam name="TResult">キャストする型。</typeparam>
        /// <param name="value">対象の値。</param>
        /// <param name="failReturnValue">キャスト失敗時に使用する値。</param>
        /// <param name="logger"></param>
        /// <returns>valueをTにキャストした値。失敗時はfailReturnValueが返される。</returns>
        public static TResult GetCastValue<TResult>(object value, TResult failReturnValue, ILogger? logger = null)
        {
            try {
                return (TResult)value;
            } catch(InvalidCastException ex) {
                if(logger != null) {
                    logger.LogWarning(ex, ex.Message);
                } else {
                    Debug.WriteLine(ex);
                }
                return failReturnValue;
            }
        }

        public static TResult GetCastWPFValue<TResult>(object value, TResult failReturnValue, ILogger? logger = null)
        {
            if(value == DependencyProperty.UnsetValue) {
                return failReturnValue;
            }

            return GetCastValue(value, failReturnValue, logger);
        }
    }
}

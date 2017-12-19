/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.IF;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
    public static class CastUtility
    {
        /// <summary>
        /// オブジェクトがキャスト出来た場合に処理を行う。
        /// </summary>
        /// <typeparam name="TCast"></typeparam>
        /// <param name="arg"></param>
        /// <param name="action"></param>
        public static bool AsAction<TCast>(object arg, Action<TCast> action)
            where TCast : class
        {
            var obj = arg as TCast;
            if(obj != null) {
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
            var obj = arg as TCast;
            if(obj != null) {
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
            return AsFunc<TCast, TResult>(arg, func, default(TResult));
        }

        /// <summary>
        /// キャスト値をキャストに成功・失敗に関わらず取得する。
        /// </summary>
        /// <typeparam name="TResult">キャストする型。</typeparam>
        /// <param name="value">対象の値。</param>
        /// <param name="failReturnValue">キャスト失敗時に使用する値。</param>
        /// <param name="logger"></param>
        /// <returns>valueをTにキャストした値。失敗時はfailReturnValueが返される。</returns>
        public static TResult GetCastValue<TResult>(object value, TResult failReturnValue, ILogger logger = null)
        {
            try {
                return (TResult)value;
            } catch(InvalidCastException ex) {
                if(logger != null) {
                    logger.Warning(ex);
                } else {
                    Debug.WriteLine(ex);
                }
                return failReturnValue;
            }
        }

        public static TResult GetCastWPFValue<TResult>(object value, TResult failReturnValue, ILogger logger = null)
        {
            if(value == DependencyProperty.UnsetValue) {
                return failReturnValue;
            }

            return GetCastValue(value, failReturnValue, logger);
        }
    }
}

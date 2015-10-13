/**
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
namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class CastUtility
    {
        /// <summary>
        /// オブジェクトがキャスト出来た場合に処理を行う。
        /// </summary>
        /// <typeparam name="TCast"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="arg"></param>
        /// <param name="action"></param>
        public static void AsAction<TCast>(object arg, Action<TCast> action)
            where TCast : class
        {
            var obj = arg as TCast;
            if(obj != null) {
                action(obj);
            }
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
    }
}

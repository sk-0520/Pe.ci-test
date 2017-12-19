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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
    /// <summary>
    /// コーディング制限。
    /// </summary>
    public static class RestrictUtility
    {
        const int dummy = int.MinValue;

        /// <summary>
        /// ブロックを強制的に作る。
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static TResult Block<TResult>(Func<TResult> func)
        {
            return func();
        }

        /// <summary>
        /// 真偽で処理実行。
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="cond"></param>
        /// <param name="trueFunc"></param>
        /// <param name="falseFunc"></param>
        /// <returns></returns>
        public static TResult Is<TResult>(bool cond, Func<TResult> trueFunc, Func<TResult> falseFunc)
        {
            if(cond) {
                return trueFunc();
            } else {
                return falseFunc();
            }
        }

        /// <summary>
        /// 真で処理実行。
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="cond"></param>
        /// <param name="trueFunc"></param>
        /// <returns></returns>
        public static TResult Is<TResult>(bool cond, Func<TResult> trueFunc)
        {
            return Is(cond, trueFunc, () => default(TResult));
        }

        /// <summary>
        /// 真偽で処理実行。
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="trueAction"></param>
        /// <param name="falseAction"></param>
        public static void Is(bool cond, Action trueAction, Action falseAction)
        {
            Is(
                cond,
                () => { trueAction(); return dummy; },
                () => { falseAction(); return dummy; }
            );
        }

        /// <summary>
        /// 真で処理実行。
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="cond"></param>
        /// <param name="action"></param>
        public static void Is<TValue>(bool cond, Action action)
        {
            Is(cond, action, () => { });
        }

        /// <summary>
        /// 指定オブジェクトが null の場合に処理実行。
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <param name="trueFunc"></param>
        /// <param name="falseFunc"></param>
        /// <returns></returns>
        public static TResult IsNull<TValue, TResult>(TValue value, Func<TResult> trueFunc, Func<TValue, TResult> falseFunc)
        {
            if(value == null) {
                return trueFunc();
            } else {
                return falseFunc(value);
            }
        }

        /// <summary>
        /// 指定オブジェクトが null の場合に処理実行。
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <param name="trueFunc"></param>
        /// <returns>偽の場合はTResultの初期値を返す。</returns>
        public static TResult IsNull<TValue, TResult>(TValue value, Func<TResult> trueFunc)
        {
            return IsNull(value, trueFunc, _ => default(TResult));
        }

        /// <summary>
        /// 指定オブジェクトが null の場合に処理実行。
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <param name="trueAction"></param>
        /// <param name="falseAction"></param>
        public static void IsNull<TValue>(TValue value, Action trueAction, Action<TValue> falseAction)
        {
            IsNull(
                value,
                () => { trueAction(); return dummy; },
                v => { falseAction(v); return dummy; }
            );
        }

        /// <summary>
        /// 指定オブジェクトが null の場合に処理実行。
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <param name="action"></param>
        public static void IsNull<TValue>(TValue value, Action action)
        {
            IsNull(value, action, _ => { });
        }

        /// <summary>
        /// 指定オブジェクトが非 null の場合に処理実行。
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <param name="trueFunc"></param>
        /// <param name="falseFunc"></param>
        /// <returns></returns>
        public static TResult IsNotNull<TValue, TResult>(TValue value, Func<TValue, TResult> trueFunc, Func<TResult> falseFunc)
        {
            if(value != null) {
                return trueFunc(value);
            } else {
                return falseFunc();
            }
        }

        /// <summary>
        /// 指定オブジェクトが非 null の場合に処理実行。
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <param name="trueFunc"></param>
        /// <returns>偽の場合はTResultの初期値を返す。</returns>
        public static TResult IsNotNull<TValue, TResult>(TValue value, Func<TValue, TResult> trueFunc)
        {
            return IsNotNull(value, trueFunc, () => default(TResult));
        }

        public static void IsNotNull<TValue>(TValue value, Action<TValue> trueAction, Action falseAction)
        {
            IsNotNull(
                value,
                v => { trueAction(v); return dummy; },
                () => { falseAction(); return dummy; }
            );
        }

        public static void IsNotNull<TValue>(TValue value, Action<TValue> trueAction)
        {
            IsNotNull(value, trueAction, () => { });
        }

    }

    [Obsolete("CodeUtility -> RestrictUtility")]
    public static class CodeUtility
    {
        public static TResult Block<TResult>(Func<TResult> func)
        {
            return RestrictUtility.Block(func);
        }
    }

}

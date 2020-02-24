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

namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
    public static class CheckUtility
    {
        /// <summary>
        /// 真を強制させる。
        /// </summary>
        /// <typeparam name="TException">失敗時に投げられる例外。</typeparam>
        /// <param name="test">テスト。</param>
        /// <exception>TException: テスト失敗時に投げられる。</exception>
        public static void Enforce<TException>(bool test)
            where TException : Exception, new()
        {
            if(!test) {
                throw new TException();
            }
        }
        [Conditional("DEBUG")]
        public static void DebugEnforce<TException>(bool test)
            where TException : Exception, new()
        {
            Enforce<TException>(test);
        }

        /// <summary>
        /// 真を強制させる。
        /// </summary>
        /// <param name="test">テスト。</param>
        /// <exception cref="Exception">テスト失敗時に投げられる。</exception>
        public static void Enforce(bool test)
        {
            Enforce<Exception>(test);
        }
        [Conditional("DEBUG")]
        public static void DebugEnforce(bool test)
        {
            Enforce(test);
        }

        /// <summary>
        /// 非nullを強制。
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        /// <param name="obj"></param>
        /// <exception cref="ArgumentNullException">null。</exception>
        public static void EnforceNotNull<TClass>(TClass obj)
            where TClass : class
        {
            Enforce<ArgumentNullException>(obj != null);
        }
        [Conditional("DEBUG")]
        public static void DebugEnforceNotNull<TClass>(TClass obj)
            where TClass : class
        {
            EnforceNotNull(obj);
        }

        /// <summary>
        /// 非nullを強制。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nullable"></param>
        public static void EnforceNotNull<T>(Nullable<T> nullable)
            where T : struct
        {
            Enforce<ArgumentNullException>(nullable.HasValue);
        }
        [Conditional("DEBUG")]
        public static void DebugEnforceNotNull<T>(Nullable<T> nullable)
            where T : struct
        {
            EnforceNotNull<T>(nullable);
        }

        /// <summary>
        /// 文字列が非nullで長さ0でないことを強制。
        /// </summary>
        /// <param name="s"></param>
        public static void EnforceNotNullAndNotEmpty(string s)
        {
            Enforce<ArgumentException>(!string.IsNullOrEmpty(s));
        }
        [Conditional("DEBUG")]
        public static void DebugEnforceNotNullAndNotEmpty(string s)
        {
            EnforceNotNullAndNotEmpty(s);
        }

        /// <summary>
        /// 文字列が非nullでホワイトスペースのみでないことを強制。
        /// </summary>
        /// <param name="s"></param>
        public static void EnforceNotNullAndNotWhiteSpace(string s)
        {
            Enforce<ArgumentException>(!string.IsNullOrWhiteSpace(s));
        }
        [Conditional("DEBUG")]
        public static void DebugEnforceNotNullAndNotWhiteSpace(string s)
        {
            EnforceNotNullAndNotWhiteSpace(s);
        }

        public static void EnforceNotZero(IntPtr p)
        {
            Enforce<ArgumentException>(p != IntPtr.Zero);
        }
        [Conditional("DEBUG")]
        public static void DebugEnforceNotZero(IntPtr p)
        {
            Enforce<ArgumentException>(p != IntPtr.Zero);
        }
    }
}

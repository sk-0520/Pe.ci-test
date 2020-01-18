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
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
    /// <summary>
    /// App.config データのキャッシュ。
    /// </summary>
    public class ConfigurationCacher: Cacher<string, object>
    {
        /// <summary>
        /// 指定キーからデータ変換して値を取得。
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key"></param>
        /// <param name="parser"></param>
        /// <returns></returns>
        public TResult Get<TResult>(string key, Func<string, TResult> parser)
        {
            return (TResult)Get(key, () => parser(ConfigurationManager.AppSettings[key]));
        }
        /// <summary>
        /// 指定キーから文字列を取得。
        /// <para><see cref="Get{TResult}(string, Func{string, TResult})"/>と文字列取得処理を合わせるためだけI/Fとして定義。</para>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            return (string)Get(key, () => ConfigurationManager.AppSettings[key]);
        }
    }

    [Obsolete("old: ConfigurationCacher")]
    public class ConfigurationCaching: ConfigurationCacher
    { }
}

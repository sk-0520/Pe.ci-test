/**
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace ContentTypeTextNet.Pe.PeMain.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Media.Imaging;
    using ContentTypeTextNet.Library.SharedLibrary.Define;
    using ContentTypeTextNet.Library.SharedLibrary.Logic;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

    /// <summary>
    /// App.config データのキャッシュ。
    /// </summary>
    public class ConfigurationCaching: Caching<string,object>
    {
        public TResult Get<TResult>(string key, Func<string, TResult> parser)
        {
            return (TResult)Get(key, () => parser(ConfigurationManager.AppSettings[key]));
        }
        public string Get(string key)
        {
            return (string)Get(key, () => ConfigurationManager.AppSettings[key]);
        }
    }
}

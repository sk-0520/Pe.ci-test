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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Pe.PeMain.Logic;

namespace ContentTypeTextNet.Pe.PeMain
{
    partial class AppResource
    {
        #region static

        static IconCaching<string> _iconCaching = new IconCaching<string>();
        static Caching<string, BitmapSource> _imageCaching = new Caching<string, BitmapSource>();

        #endregion

        #region property

        public static string ApplicationTasktrayPath
        {
            get
            {
#if DEBUG
                return applicationTasktrayDebug;
#elif BETA
				return applicationTasktrayBeta;
#else
				return applicationTasktrayRelease;
#endif
            }
        }

        #endregion

        #region function

        static BitmapSource GetImage(string path)
        {
            return _imageCaching.Get(path, () => {
                var uri = SharedConstants.GetEntryUri(path);
                var image = new BitmapImage(uri);
                FreezableUtility.SafeFreeze(image);
                return image;
            });
        }

        static BitmapSource GetIcon(string path, IconScale iconScale, ILogger logger = null)
        {
            return _iconCaching[iconScale].Get(path, () => {
                using(var icon = new IconWrapper(path, iconScale)) {
                    var image = icon.MakeBitmapSource();
                    FreezableUtility.SafeFreeze(image);
                    return image;
                }
            });
        }

        #endregion
    }
}

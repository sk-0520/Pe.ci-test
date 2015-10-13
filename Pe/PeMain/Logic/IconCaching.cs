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
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Media.Imaging;
    using ContentTypeTextNet.Library.SharedLibrary.Define;
    using ContentTypeTextNet.Library.SharedLibrary.Logic;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

    public class IconCaching<TChildKey>: Dictionary<IconScale, Caching<TChildKey, BitmapSource>>
    {
        public IconCaching()
        {
            Initialize();
        }

        #region function

        protected void Initialize()
        {
            foreach(var iconScale in EnumUtility.GetMembers<IconScale>()) {
                this.Add(iconScale, new Caching<TChildKey, BitmapSource>());
            }
        }

        public new void Clear()
        {
            foreach(var value in this.Values) {
                value.Clear();
            }

            base.Clear();

            Initialize();
        }

        #endregion
    }
}

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
namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using ContentTypeTextNet.Library.SharedLibrary.Define;

    public static class IconScaleExtension
    {
        public static double ToWidth(this IconScale iconScale)
        {
            return (int)iconScale;
        }

        public static double ToHeight(this IconScale iconScale)
        {
            return (int)iconScale;
        }

        public static Size ToSize(this IconScale iconScale)
        {
            return new Size((int)iconScale, (int)iconScale);
        }
    }
}

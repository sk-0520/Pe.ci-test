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
namespace ContentTypeTextNet.Library.SharedLibrary.CompatibleForms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Forms = System.Windows.Forms;
    using System.Threading.Tasks;
    using System.Windows;
    using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms.Utility;
    using ContentTypeTextNet.Library.SharedLibrary.Attribute;
    using ContentTypeTextNet.Library.SharedLibrary.Define;

    [Obsolete]
    public static class SystemInformation
    {
        public static TimeSpan MouseHoverTime { get { return TimeSpan.FromMilliseconds(Forms.SystemInformation.MouseHoverTime); } }
        [PixelKind(Px.Device)]
        public static Size BorderSize { get { return DrawingUtility.Convert(Forms.SystemInformation.BorderSize); } }
    }
}

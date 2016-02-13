/*
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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;

namespace ContentTypeTextNet.Pe.PeMain.Data.Temporary
{
    public class ClipboardData
    {
        public ClipboardData()
        {
            Type = ClipboardType.None;
            Hash = new HashItemModel();
            Body = new ClipboardBodyItemModel();
        }

        #region property

        public ClipboardType Type { get; set; }
        public HashItemModel Hash { get; set; }
        public ClipboardBodyItemModel Body { get; set; }

        #endregion
    }
}

﻿/*
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Library.SharedLibrary.Define
{
    /// <summary>
    /// ピクセル情報。
    /// </summary>
    public enum Px
    {
        /// <summary>
        /// 知らん。
        /// </summary>
        Unknown,
        /// <summary>
        /// 論理座標系。
        /// </summary>
        Logical,
        /// <summary>
        /// デバイス座標系。
        /// </summary>
        Device,
    }
}

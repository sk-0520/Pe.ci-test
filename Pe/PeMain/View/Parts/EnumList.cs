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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Pe.Library.PeData.Define;

namespace ContentTypeTextNet.Pe.PeMain.View.Parts
{
    public static class EnumList
    {
        public static IEnumerable<DockType> DockTypeList
        {
            get
            {
                var result = new[] {
                    DockType.None,
                    DockType.Left,
                    DockType.Top,
                    DockType.Right,
                    DockType.Bottom,
                };

                return result;
            }
        }

        public static IEnumerable<IconScale> PlainIconScaleList
        {
            get
            {
                var result = new[] {
                    IconScale.Small,
                    IconScale.Normal,
                    IconScale.Big,
                };

                return result;
            }
        }

        public static IEnumerable<TemplateReplaceMode> TemplateReplaceModeList
        {
            get
            {
                var result = new[] {
                    TemplateReplaceMode.None,
                    TemplateReplaceMode.Text,
                    TemplateReplaceMode.Program,
                };

                return result;
            }
        }

        public static IEnumerable<ToolbarButtonPosition> ToolbarButtonPositionList
        {
            get
            {
                var result = new[] {
                    ToolbarButtonPosition.Near,
                    ToolbarButtonPosition.Center,
                    ToolbarButtonPosition.Far,
                };

                return result;
            }
        }
    }
}

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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
    internal static class FontUtility
    {
        /// <summary>
        /// 指定フォントファミリ名からFontFamily作成。
        /// </summary>
        /// <param name="fontFamily">フォントファミリ名</param>
        /// <param name="defaultFontFamily">指定名から生成できなかった場合に代替として使用されるFontFamily。</param>
        /// <returns></returns>
        public static FontFamily MakeFontFamily(string fontFamily, FontFamily defaultFontFamily)
        {
            if(!string.IsNullOrWhiteSpace(fontFamily)) {
                if(Fonts.SystemFontFamilies.Any(f => f.FamilyNames.Any(n => n.Value == fontFamily))) {
                    var result = new FontFamily(fontFamily);
                    return result;
                }
            }

            return defaultFontFamily;
        }

        /// <summary>
        /// <para>TODO: この環境で再現できないのでスタブのみ作成</para>
        /// </summary>
        /// <param name="fontFamily"></param>
        /// <returns></returns>
        public static string GetOriginalFontFamilyName(FontFamily fontFamily)
        {
            CheckUtility.DebugEnforceNotNull(fontFamily);

            if(fontFamily.FamilyNames.Any()) {
                return fontFamily.FamilyNames.First().Value;
            }

            return fontFamily.Source;
        }
    }
}

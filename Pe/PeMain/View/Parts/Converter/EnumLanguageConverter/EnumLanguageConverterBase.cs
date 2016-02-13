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
using System.Windows.Data;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Converter
{
    public abstract class EnumLanguageConverterBase<TEnum>: IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(!Enum.IsDefined(typeof(TEnum), values[0])) {
                return string.Empty;
            }
            var tag = (TEnum)values[0];
            var lang = (ILanguage)values[1];

            return LanguageUtility.GetTextFromEnum(tag, lang);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

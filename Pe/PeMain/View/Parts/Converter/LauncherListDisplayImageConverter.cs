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
namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Converter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Data;
    using ContentTypeTextNet.Library.SharedLibrary.Define;
    using ContentTypeTextNet.Library.SharedLibrary.IF;
    using ContentTypeTextNet.Pe.Library.PeData.Item;
    using ContentTypeTextNet.Pe.PeMain.Data;
    using ContentTypeTextNet.Pe.PeMain.IF;
    using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
    using ContentTypeTextNet.Pe.PeMain.ViewModel;

    /// <summary>
    /// [Obsolete]TODO: 状態を持ちすぎ、しんどい
    /// </summary>
    [Obsolete]
    public class LauncherListDisplayImageConverter: IValueConverter
    {
        #region static

        // TODO: CommonData.LauncherIconCachingを使いたかったけど渡し方分からなんだ。
        //public static LauncherIconCaching LauncherIconCaching { get; set; }
        public static IAppNonProcess NonProcess { get; set; }
        public static IAppSender AppSender { get; set; }

        #endregion

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var model = value as LauncherItemModel;
            if(model != null) {
                var vm = new LauncherItemSimpleViewModel(model, NonProcess, AppSender);
                return vm.GetIcon(IconScale.Small);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

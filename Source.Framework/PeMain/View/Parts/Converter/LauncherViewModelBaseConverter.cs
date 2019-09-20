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
using System.Windows.Data;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.PeMain.ViewModel;
using ContentTypeTextNet.Pe.PeMain.ViewModel.Control;

namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Converter
{
    public class LauncherViewModelConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var viewModel = value as LauncherListItemViewModel;
            if(viewModel != null) {
                // TODO: parameterで切り替えれた方があとあと便利そう。
                // TODO: ダサい
                var result = new LauncherItemEditViewModel(viewModel.Model, viewModel.RefreshText, viewModel.RefreshImage, viewModel.AppNonProcess, viewModel.AppSender);

                return result;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

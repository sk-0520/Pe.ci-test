namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Converter
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Data;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;
	using ContentTypeTextNet.Pe.PeMain.ViewModel.Control;

	public class LauncherViewModelConverter: IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var viewModel = value as LauncherListItemViewModel;
			if(viewModel != null) {
				// TODO: parameterで切り替えれた方があとあと便利そう。
				// TODO: ダサい
				var result = new LauncherItemEditViewModel(viewModel.Model, viewModel, viewModel.AppNonProcess, viewModel.AppSender);
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

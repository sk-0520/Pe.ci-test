namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Converter
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Data;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;

	public class LauncherItemsListSelectedItemConverter: IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var vm = value as LauncherViewModelBase;
			if(vm != null) {
				var model = vm.GetModel();
				var result = new LauncherSimpleViewModel(model, vm.LauncherIconCaching, vm.NonProcess);
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

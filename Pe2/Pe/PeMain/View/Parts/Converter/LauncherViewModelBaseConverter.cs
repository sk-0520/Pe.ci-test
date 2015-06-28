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

	public class LauncherViewModelBaseConverter: IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var model = value as LauncherViewModelBase;
			if(model != null) {
				var result = new LauncherSimpleViewModel(model.GetModel(), model.LauncherIconCaching, model.NonProcess);
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

namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Converter
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Data;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;

	public class CommandItemConverter: IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var item = value as CommandItemViewModel;
			if (item != null) {
				return item.DisplayText;
			} else {
				return string.Empty;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var item = value as CommandItemViewModel;
			if (item != null) {
				return item.DisplayText;
			} else {
				return string.Empty;
			}
		}
	}
}

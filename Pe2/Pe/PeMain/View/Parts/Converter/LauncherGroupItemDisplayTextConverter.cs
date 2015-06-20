namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Converter
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Data;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class LauncherGroupItemDisplayTextConverter: IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var model = (LauncherGroupItemModel)value;
			return DisplayTextUtility.GetDisplayName(model);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return null;
		}
	}
}

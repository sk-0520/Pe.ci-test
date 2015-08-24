namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Converter
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Data;
	using ContentTypeTextNet.Library.SharedLibrary.Define;

	public class DockTypeMenuTuneConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var dockType = (DockType)value;
			if (dockType == DockType.Left) {
				Debug.WriteLine(value);
				return DockType.Right;
			}
			return dockType;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		//public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		//{
		//	var dockType = (DockType)values[0];
		//	var isEnabledCorrection = (bool)values[1];
		//	if (dockType == DockType.Left) {
		//		return DockType.Right;
		//	} else if (dockType == DockType.Right && isEnabledCorrection) {
		//		return DockType.Left;
		//	}
		//	return dockType;
		//}

		//public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
		//{
		//	throw new NotImplementedException();
		//}
	}
}

namespace ContentTypeTextNet.Library.SharedLibrary.View.Converter
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Data;
	using System.Windows.Media;

	public class ColorBrushConverter : IValueConverter
	{
		public virtual object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var color = (Color)value;
			return new SolidColorBrush(color);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

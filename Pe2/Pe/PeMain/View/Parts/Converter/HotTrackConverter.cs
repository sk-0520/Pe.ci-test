namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Converter
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Data;
	using System.Windows.Input;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.SharedLibrary.Define;

	public class HotTrackConverter: IMultiValueConverter
	{

		//public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		//{
		//	var color = (Color)value;
		//	var ui = (UIElement)parameter;

		//	var brush = new LinearGradientBrush();
		//	brush.EndPoint = new System.Windows.Point(0.5, 1.0);
		//	brush.StartPoint = new System.Windows.Point(0.5, 0.0);
		//	brush.GradientStops.Add(new GradientStop(System.Windows.Media.Color.FromArgb(0xFF, color.R, color.G, color.B), 0.00));
		//	brush.GradientStops.Add(new GradientStop(System.Windows.Media.Color.FromArgb(0x00, color.R, color.G, color.B), 1.00));
		//	return brush;
		//}

		//public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		//{
		//	throw new NotImplementedException();
		//}
		public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var color = (Color)values[0];
			var dockType = (DockType)values[1];
			var length = (double)values[2];

			var brush = new LinearGradientBrush();
			switch(dockType) {
				case DockType.None:
				case DockType.Bottom:
					brush.StartPoint = new Point(0.5, 0.0);
					brush.EndPoint = new Point(0.5, 1.0);
					break;
				case DockType.Left:
					brush.StartPoint = new Point(1.0, 0.5);
					brush.EndPoint = new Point(0.0, 0.5);
					break;
				case DockType.Top:
					brush.StartPoint = new Point(0.5, 0.0);
					brush.EndPoint = new Point(0.5, 1.0);
					break;
				case DockType.Right:
					brush.StartPoint = new Point(0.5, 1.0);
					brush.EndPoint = new Point(0.5, 0.0);
					break;
				default:
					throw new NotImplementedException();
			}
			//brush.EndPoint = new Point(0.5, 1.0);
			//brush.StartPoint = new Point(0.5, 0.0);
			brush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, color.R, color.G, color.B), 1.00));
			brush.GradientStops.Add(new GradientStop(Color.FromArgb(0xef, color.R, color.G, color.B), 1 - length));
			brush.GradientStops.Add(new GradientStop(Color.FromArgb(0x00, color.R, color.G, color.B), 0.00));
			return brush;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

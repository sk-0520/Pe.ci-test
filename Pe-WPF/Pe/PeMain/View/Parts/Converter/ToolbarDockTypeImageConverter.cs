namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Converter
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Data;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class ToolbarDockTypeImageConverter: IValueConverter
	{
		readonly Size imageSize = new Size(IconScale.Small.ToWidth() * 2, IconScale.Small.ToHeight());

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var dockType = (DockType)value;
			var element = LauncherToolbarUtility.MakeDockIcon(dockType, imageSize);
			return element;
			//return ImageUtility.MakeBitmapBitmapSourceDefualtDpi(element);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

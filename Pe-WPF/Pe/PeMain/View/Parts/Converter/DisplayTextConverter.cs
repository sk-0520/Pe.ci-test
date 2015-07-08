namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Converter
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Data;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class DisplayTextConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var id = value as ITId<string>;
			var name = value as IName;

			if (id != null && name != null) {
				return DisplayTextUtility.GetDisplayName(id, name);
			}
			if (id != null) {
				return DisplayTextUtility.GetDisplayName(id);
			}
			if (name != null) {
				return DisplayTextUtility.GetDisplayName(name);
			}

			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

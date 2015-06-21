namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Converter
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.View.Converter;

	class DockTypeMultiEqualConverter: LogicalMultiEqualConverter
	{
		public override object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var dock = values.OfType<DockType>().ToArray();
			if(dock.Length == 2) {
				return dock[0] == dock[1];
			} else {
				return base.Convert(values, targetType, parameter, culture);
			}
		}
	}
}

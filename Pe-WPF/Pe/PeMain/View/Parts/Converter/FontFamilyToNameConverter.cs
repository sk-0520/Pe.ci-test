using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Converter
{
	/// <summary>
	/// <para>http://sourcechord.hatenablog.com/entry/2014/04/25/013631</para>
	/// </summary>
	public class FontFamilyToNameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var fontFamily = value as FontFamily;
			var currentLang = XmlLanguage.GetLanguage(culture.IetfLanguageTag);
			return fontFamily.FamilyNames.FirstOrDefault(o => o.Key == currentLang).Value ?? fontFamily.Source;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

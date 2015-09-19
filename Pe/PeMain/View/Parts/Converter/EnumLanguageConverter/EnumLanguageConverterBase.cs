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

	public abstract class EnumLanguageConverterBase<TEnum>: IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if(!Enum.IsDefined(typeof(TEnum), values[0])) {
				return string.Empty;
			}
			var tag = (TEnum)values[0];
			var lang = (ILanguage)values[1];

			return LanguageUtility.GetTextFromEnum(tag, lang);
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

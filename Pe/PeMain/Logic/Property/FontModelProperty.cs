namespace ContentTypeTextNet.Pe.PeMain.Logic.Property
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	public static class FontModelProperty
	{
		public static FontFamily GetFamily(FontModel model, FontFamily defaultFontFamily)
		{
			return FontUtility.MakeFontFamily(model.Family, defaultFontFamily);
		}
		public static FontFamily GetFamilyDefault(FontModel model)
		{
			return GetFamily(model, SystemFonts.MessageFontFamily);
		}
		public static bool SetFamily(FontModel model, FontFamily value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			if (value != null) {
				var fontFamily = FontUtility.GetOriginalFontFamilyName(value);
				if (model.Family != fontFamily) {
					model.Family = fontFamily;
					onPropertyChanged(propertyName);

					return true;
				}
			}

			return false;
		}

		public static bool GetBold(FontModel model)
		{
			return model.Bold;
		}

		public static bool SetBold(FontModel model, bool value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			if (model.Bold != value) {
				model.Bold = value;
				onPropertyChanged(propertyName);

				return true;
			}

			return false;
		}

		public static bool GetItalic(FontModel model)
		{
			return model.Italic;
		}

		public static bool SetItalic(FontModel model, bool value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			if (model.Italic != value) {
				model.Italic = value;
				onPropertyChanged(propertyName);

				return true;
			}

			return false;
		}

		public static double GetSize(FontModel model)
		{
			return model.Size;
		}

		public static bool SetSize(FontModel model, double value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			if (model.Size != value) {
				model.Size = value;
				onPropertyChanged(propertyName);

				return true;
			}

			return false;
		}

	}
}

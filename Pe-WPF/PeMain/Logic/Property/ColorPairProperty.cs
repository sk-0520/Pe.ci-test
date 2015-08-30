namespace ContentTypeTextNet.Pe.PeMain.Logic.Property
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Pe.Library.PeData.IF;

	public static class ColorPairProperty
	{
		public static Color GetForeColor(IColorPair model)
		{
			return model.ForeColor;
		}

		public static bool SetForeColor(IColorPair model, Color value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			if(model.ForeColor != value) {
				model.ForeColor = value;
				onPropertyChanged(propertyName);

				return true;
			}

			return false;
		}

		public static Color GetBackColor(IColorPair model)
		{
			return model.BackColor;
		}

		public static bool SetBackColor(IColorPair model, Color value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			if(model.BackColor != value) {
				model.BackColor = value;
				onPropertyChanged(propertyName);

				return true;
			}

			return false;
		}

		public static Color GetNoneAlphaForeColor(IColorPair model)
		{
			var result = MediaUtility.GetNoneAlphaColor(GetForeColor(model));
			return result;
		}

		public static bool SetNoneAlphaForekColor(IColorPair model, Color value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			return SetForeColor(model, MediaUtility.GetNoneAlphaColor(value), onPropertyChanged, propertyName);
		}

		public static Color GetNoneAlphaBackColor(IColorPair model)
		{
			var result = MediaUtility.GetNoneAlphaColor(GetBackColor(model));
			return result;
		}

		public static bool SetNoneAlphaBackColor(IColorPair model, Color value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			return SetBackColor(model, MediaUtility.GetNoneAlphaColor(value), onPropertyChanged, propertyName);
		}
	
	
	}
}

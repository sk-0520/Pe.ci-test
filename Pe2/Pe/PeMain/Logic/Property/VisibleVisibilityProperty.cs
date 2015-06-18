namespace ContentTypeTextNet.Pe.PeMain.Logic.Property
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Pe.Library.PeData.IF;

	public static class VisibleVisibilityProperty
	{
		public static bool GetVisible(IVisible model)
		{
			return model.Visible;
		}

		public static void SetVisible(IVisible model, bool value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			if (model.Visible != value) {
				model.Visible = value;
				onPropertyChanged(propertyName);
				onPropertyChanged("Visibility");
			}
		}

		public static Visibility GetVisibility(IVisible model)
		{
			return GetVisible(model) ? Visibility.Visible : Visibility.Hidden;
		}

		public static void SetVisibility(IVisible model, Visibility value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			SetVisible(model, value == Visibility.Visible, onPropertyChanged, propertyName);
		}


	}
}

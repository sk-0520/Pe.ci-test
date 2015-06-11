namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.View.Parts;
	using ContentTypeTextNet.Pe.PeMain.Logic.Extension;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

	public static class LanguageUtility
	{
		public static void SetLanguage(DependencyObject root, LanguageManager language)
		{
			var map = new Dictionary<Type, Action<DependencyObject>>() {
				{ typeof(Button), ui => ((Button)ui).SetUI(language) },
				{ typeof(CheckBox), ui => ((CheckBox)ui).SetUI(language) },
				{ typeof(GridViewColumnHeader), ui => ((GridViewColumnHeader)ui).SetUI(language) },
			};

			var window = root as Window;
			if (window != null) {
				window.SetUI(language);
			}

			foreach (var dependencyObject in UIUtility.FindVisualChildren<DependencyObject>(root)) {
				var type = dependencyObject.GetType();
				
				Action<DependencyObject> action;
				if (map.TryGetValue(type, out action)) {
					action(dependencyObject);
				}
			}

		}
	}
}

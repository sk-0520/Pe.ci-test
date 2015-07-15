namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Windows;
	using System.Windows.Controls;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Pe.PeMain.Logic.Extension;

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

			foreach (var dependencyObject in UIUtility.FindLogicalChildren<DependencyObject>(root)) {
				var type = dependencyObject.GetType();
				
				Action<DependencyObject> action;
				if (map.TryGetValue(type, out action)) {
					action(dependencyObject);
				}
			}

		}


	}
}

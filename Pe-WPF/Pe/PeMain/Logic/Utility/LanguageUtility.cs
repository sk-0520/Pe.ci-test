namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Media;
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
				{ typeof(TextBlock), ui => ((TextBlock)ui).SetUI(language) },
				{ typeof(GridViewColumnHeader), ui => ((GridViewColumnHeader)ui).SetUI(language) },
			};

			var window = root as Window;
			if (window != null) {
				window.SetUI(language);
			}

			foreach (var dependencyObject in UIUtility.FindLogicalChildren<DependencyObject>(root)) {
				var type = dependencyObject.GetType();
				//Debug.WriteLine("L: " + type.ToString());
				Action<DependencyObject> action;
				if (map.TryGetValue(type, out action)) {
					action(dependencyObject);
				}
				if (dependencyObject is Visual)
				foreach (var visualElement in UIUtility.FindVisualChildren<Visual>(dependencyObject)) {
					var visualType = visualElement.GetType();
					//Debug.WriteLine("V: " + visualType.ToString());
					if (map.TryGetValue(visualType, out action)) {
						action(visualElement);
					}
				}
			}

		}


	}
}

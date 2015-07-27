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
	using ContentTypeTextNet.Pe.PeMain.View.Parts.Attached;

	public static class LanguageUtility
	{
		static void SetUI_Impl(DependencyObject baseElement, LanguageManager language, IReadOnlyDictionary<string, string> map, Action<string, string> action)
		{
			var key = Language.GetKey(baseElement);
			var hint = Language.GetHint(baseElement);
			if(!string.IsNullOrWhiteSpace(key)) {
				action(key, hint);
			}
		}

		public static void SetTitle(Window ui, LanguageManager language, IReadOnlyDictionary<string, string> map = null)
		{
			SetUI_Impl(ui, language, map, (key, hint) => ui.Title = language[key, map]);
		}

		public static void SetContent(ContentControl ui, LanguageManager language, IReadOnlyDictionary<string, string> map = null)
		{
			SetUI_Impl(ui, language, map, (key, hint) => {
				if(!ui.HasContent || ui.Content is string) {
					ui.Content = language[key, map];
				}
			});
		}

		//public static void SetUI(this CheckBox ui, LanguageManager language, IReadOnlyDictionary<string, string> map = null)
		//{
		//	SetUI_Impl(ui, language, map, key => {
		//		if(!ui.HasContent || ui.Content is string) {
		//			ui.Content = language[key, map];
		//		}
		//	});
		//}

		public static void SetText(TextBlock ui, LanguageManager language, IReadOnlyDictionary<string, string> map = null)
		{
			SetUI_Impl(ui, language, map, (key, hint) => {
				ui.Text = language[key, map];
			});
		}

		public static void SetColumn(GridViewColumnHeader ui, LanguageManager language, IReadOnlyDictionary<string, string> map = null)
		{
			if(ui.Column != null) {
				SetUI_Impl(ui.Column, language, map, (key, hint) => {
					if(!ui.HasContent || ui.Content is string) {
						ui.Content = language[key, map];
					}
				});
			}
		}

		/// <summary>
		/// TODO: 後で実装はうまいことする。
		/// </summary>
		/// <param name="control"></param>
		/// <param name="language"></param>
		/// <param name="map"></param>
		static void SetLanguageItem(DependencyObject control, LanguageManager language, IReadOnlyDictionary<string, string> map)
		{
			var gridViewColumnHeader = control as GridViewColumnHeader;
			if(gridViewColumnHeader != null) {
				SetColumn(gridViewColumnHeader, language, map);
			} else {
				var contentControl = control as ContentControl;
				if(contentControl != null) {
					SetContent(contentControl, language, map);
				} else {
					var textBlock = control as TextBlock;
					if(textBlock != null) {
						SetText(textBlock, language, map);
					}
				}
			}
		}

		public static void SetLanguage(DependencyObject root, LanguageManager language, IReadOnlyDictionary<string, string> map = null)
		{
			var window = root as Window;
			if (window != null) {
				SetTitle(window, language, map);
			}

			foreach (var dependencyObject in UIUtility.FindLogicalChildren<DependencyObject>(root)) {
				//var type = dependencyObject.GetType();
				//Debug.WriteLine("L: " + type.ToString());
				//Action<DependencyObject> action;
				//if (map.TryGetValue(type, out action)) {
				//	action(dependencyObject);
				//}
				SetLanguageItem(dependencyObject, language, map);
				if (dependencyObject is Visual)
				foreach (var visualElement in UIUtility.FindVisualChildren<Visual>(dependencyObject)) {
					//var visualType = visualElement.GetType();
					//Debug.WriteLine("V: " + visualType.ToString());
					//if (map.TryGetValue(visualType, out action)) {
					//	action(visualElement);
					//}
					SetLanguageItem(visualElement, language, map);
				}
			}

		}


	}
}

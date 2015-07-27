namespace ContentTypeTextNet.Pe.PeMain.Logic.Extension
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.PeMain.View.Parts;
	using ContentTypeTextNet.Pe.PeMain.View.Parts.Attached;

	public static class LanguageExtension
	{
		static void SetUI_Impl(DependencyObject ui, LanguageManager language, IReadOnlyDictionary<string, string> map, Action<string> action)
		{
			var key = Language.GetKey(ui);
			if(!string.IsNullOrWhiteSpace(key)) {
				action(key);
			}
		}

		public static void SetUI(this Window ui, LanguageManager language, IReadOnlyDictionary<string, string> map = null)
		{
			SetUI_Impl(ui, language, map, key => ui.Title = language[key, map]);
		}

		public static void SetUI(this Button ui, LanguageManager language, IReadOnlyDictionary<string, string> map = null)
		{
			SetUI_Impl(ui, language, map, key => {
				if(!ui.HasContent || ui.Content is string) {
					ui.Content = language[key, map];
				}
			});
		}

		public static void SetUI(this CheckBox ui, LanguageManager language, IReadOnlyDictionary<string, string> map = null)
		{
			SetUI_Impl(ui, language, map, key => {
				if(!ui.HasContent || ui.Content is string) {
					ui.Content = language[key, map];
				}
			});
		}

		public static void SetUI(this TextBlock ui, LanguageManager language, IReadOnlyDictionary<string, string> map = null)
		{
			SetUI_Impl(ui, language, map, key => {
				ui.Text = language[key, map];
			});
		}

		public static void SetUI(this GridViewColumnHeader ui, LanguageManager language, IReadOnlyDictionary<string, string> map = null)
		{
			if (ui.Column != null) {
				SetUI_Impl(ui.Column, language, map, key => {
					if (!ui.HasContent || ui.Content is string) {
						ui.Content = language[key, map];
					}
				});
			}
		}
	}
}

namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.PeMain.View.Parts;

	public static class LanguageUtility
	{
		static void SetUI_Impl(UIElement ui, LanguageCollectionViewModel language, IReadOnlyDictionary<string,string> map, Action<string> action)
		{
			var key = ui.LanguageKey();
			if(!string.IsNullOrWhiteSpace(key)) {
				action(key);
			}
		}

		public static void SetUI(this Window ui, LanguageCollectionViewModel language, IReadOnlyDictionary<string,string> map = null)
		{
			SetUI_Impl(ui, language, map, key => ui.Title = language[key, map]);
		}

		public static void SetUI(this Button ui, LanguageCollectionViewModel language, IReadOnlyDictionary<string, string> map = null)
		{
			SetUI_Impl(ui, language, map, key => {
				if(!ui.HasContent || ui.Content is string) {
					ui.Content = language[key, map];
				}
			});
		}
	}
}

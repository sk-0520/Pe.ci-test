namespace ContentTypeTextNet.Pe.PeMain.View.Parts
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;

	public static class XamlTag
	{
		#region LanguageKey

		public static readonly DependencyProperty LanguageKeyProperty = DependencyProperty.RegisterAttached(
			"LanguageKey",
			typeof(string),
			typeof(XamlTag), 
			new FrameworkPropertyMetadata(null)
		);

		public static string GetLanguageKey(UIElement element)
		{
			Validate(element);

			return (string)element.GetValue(LanguageKeyProperty);
		}
		public static void SetLanguageKey(UIElement element, string value)
		{
			Validate(element);

			element.SetValue(LanguageKeyProperty, value);
		}

		#region extension

		public static string LanguageKey(this UIElement element)
		{
			return GetLanguageKey(element);
		}

		public static void LanguageKey(this UIElement element, string value)
		{
			SetLanguageKey(element, value);
		}

		#endregion

		#endregion

		#region function

		static void Validate(UIElement element)
		{
			if(element == null) {
				throw new ArgumentNullException("element");
			}
		}

		#endregion
	}
}

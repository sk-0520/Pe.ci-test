namespace ContentTypeTextNet.Pe.PeMain.View.Parts
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;

	public class XamlTag
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

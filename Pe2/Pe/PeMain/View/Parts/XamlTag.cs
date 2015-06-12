namespace ContentTypeTextNet.Pe.PeMain.View.Parts
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Pe.Library.PeData.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	public static class XamlTag
	{
		#region LanguageKey

		public static readonly DependencyProperty LanguageKeyProperty = DependencyProperty.RegisterAttached(
			"LanguageKey",
			typeof(string),
			typeof(XamlTag), 
			new FrameworkPropertyMetadata(null)
		);

		public static string GetLanguageKey(DependencyObject dependencyObject)
		{
			Validate(dependencyObject);

			return (string)dependencyObject.GetValue(LanguageKeyProperty);
		}
		public static void SetLanguageKey(DependencyObject dependencyObject, string value)
		{
			Validate(dependencyObject);

			dependencyObject.SetValue(LanguageKeyProperty, value);
		}

		#region extension

		public static string LanguageKey(this DependencyObject dependencyObject)
		{
			return GetLanguageKey(dependencyObject);
		}

		public static void LanguageKey(this DependencyObject dependencyObject, string value)
		{
			SetLanguageKey(dependencyObject, value);
		}

		#endregion

		#endregion

		#region function

		static void Validate(DependencyObject dependencyObject)
		{
			if (dependencyObject == null) {
				throw new ArgumentNullException("dependencyObject");
			}
		}

		#endregion
	}
}

namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Attached
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Pe.Library.PeData.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	public static class Language
	{
		#region LanguageKey

		public static readonly DependencyProperty KeyProperty = DependencyProperty.RegisterAttached(
			"Key",
			typeof(string),
			typeof(Language), 
			new FrameworkPropertyMetadata(null)
		);

		public static string GetKey(DependencyObject dependencyObject)
		{
			Validate(dependencyObject);

			return (string)dependencyObject.GetValue(KeyProperty);
		}
		public static void SetKey(DependencyObject dependencyObject, string value)
		{
			Validate(dependencyObject);

			dependencyObject.SetValue(KeyProperty, value);
		}

		//#region extension

		//public static string LanguageKey(this DependencyObject dependencyObject)
		//{
		//	return GetLanguageKey(dependencyObject);
		//}

		//public static void LanguageKey(this DependencyObject dependencyObject, string value)
		//{
		//	SetLanguageKey(dependencyObject, value);
		//}

		//#endregion

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

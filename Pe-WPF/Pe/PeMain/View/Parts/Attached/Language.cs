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
		#region KeyProperty

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

		#endregion

		#region HintProperty

		public static readonly DependencyProperty HintProperty = DependencyProperty.RegisterAttached(
			"Hint",
			typeof(string),
			typeof(Language),
			new FrameworkPropertyMetadata(null)
		);

		public static string GetHint(DependencyObject dependencyObject)
		{
			Validate(dependencyObject);

			return (string)dependencyObject.GetValue(HintProperty);
		}
		public static void SetHint(DependencyObject dependencyObject, string value)
		{
			Validate(dependencyObject);

			dependencyObject.SetValue(HintProperty, value);
		}

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

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
		#region WordProperty

		public static readonly DependencyProperty WordProperty = DependencyProperty.RegisterAttached(
			"Word",
			typeof(string),
			typeof(Language), 
			new FrameworkPropertyMetadata()
		);

		public static string GetWord(DependencyObject dependencyObject)
		{
			Validate(dependencyObject);

			return (string)dependencyObject.GetValue(WordProperty);
		}
		public static void SetWord(DependencyObject dependencyObject, string value)
		{
			Validate(dependencyObject);

			dependencyObject.SetValue(WordProperty, value);
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

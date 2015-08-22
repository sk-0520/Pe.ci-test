namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Attached
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;

	public static class MenuIcon
	{
		#region IsStrongProperty

		public static readonly DependencyProperty IsStrongProperty = DependencyProperty.RegisterAttached(
			"IsStrong",
			typeof(bool),
			typeof(MenuIcon),
			new PropertyMetadata(false, OnIsStrongChanged)
		);

		static void OnIsStrongChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			var webBrowser = dependencyObject as Image;
			if(webBrowser != null) {
				SetIsStrong(webBrowser, (bool)e.NewValue);
			}
		}
		public static bool GetIsStrong(DependencyObject dependencyObject)
		{
			return (bool)dependencyObject.GetValue(IsStrongProperty);
		}
		public static void SetIsStrong(DependencyObject dependencyObject, bool value)
		{
			dependencyObject.SetValue(IsStrongProperty, value);
			var img = dependencyObject as Image;
			if(img != null) {
				if(value) {
					img.Opacity = 1;
				} else {
					img.Opacity = 0.5;
				}
			}
		}

		#endregion
	}
}

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

		const double checkedIsStrong = 1;
		const double uncheckedIsStrong = 0.5;

		public static readonly DependencyProperty IsStrongProperty = DependencyProperty.RegisterAttached(
			"IsStrong",
			typeof(bool),
			typeof(MenuIcon),
			new FrameworkPropertyMetadata(true,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsStrongChanged)
		);

		static void OnIsStrongChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			var element = dependencyObject as UIElement;
			if(element != null) {
				SetIsStrong(element, (bool)e.NewValue);
			}
		}

		public static bool GetIsStrong(UIElement element)
		{
			return (bool)element.GetValue(IsStrongProperty);
		}
		public static void SetIsStrong(UIElement element, bool value)
		{
			element.SetValue(IsStrongProperty, value);
			//var img = imageObject as Image;
			if(element != null) {
				if(value) {
					element.Opacity = checkedIsStrong;
				} else {
					element.Opacity = uncheckedIsStrong;
				}
			}
		}

		#endregion
	}
}

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
			var imageObject = dependencyObject as Image;
			if(imageObject != null) {
				SetIsStrong(imageObject, (bool)e.NewValue);
			}
		}

		public static bool GetIsStrong(Image imageObject)
		{
			return (bool)imageObject.GetValue(IsStrongProperty);
		}
		public static void SetIsStrong(Image imageObject, bool value)
		{
			imageObject.SetValue(IsStrongProperty, value);
			//var img = imageObject as Image;
			if(imageObject != null) {
				if(value) {
					imageObject.Opacity = checkedIsStrong;
				} else {
					imageObject.Opacity = uncheckedIsStrong;
				}
			}
		}

		#endregion
	}
}

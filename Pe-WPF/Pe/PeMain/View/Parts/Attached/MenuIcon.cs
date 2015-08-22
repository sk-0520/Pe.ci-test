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
		#region define

		const double checkedOpacity = 1.0;
		const double uncheckedOpacity = 0.5;

		#endregion

		#region IsStrong

		public static readonly DependencyProperty IsStrongProperty = DependencyProperty.RegisterAttached(
			"IsStrong",
			typeof(bool),
			typeof(MenuIcon),
			new FrameworkPropertyMetadata(false,FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsStrongChanged)
		);

		private static void OnIsStrongChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var element = d as Image;
			if(element != null) {
				SetIsStrong(element, (bool)e.NewValue);
			}
		}

		public static bool GetIsStrong(Image imageObject)
		{
			return (bool)imageObject.GetValue(IsStrongProperty);
		}

		public static void SetIsStrong(Image imageObject, bool value)
		{
			imageObject.SetValue(IsStrongProperty, value);
			if(value) {
				imageObject.Opacity = checkedOpacity;
			} else {
				imageObject.Opacity = uncheckedOpacity;
			}
		}

		#endregion
	}
}

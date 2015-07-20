namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Attached
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;

	public class IconSize
	{
		public static readonly DependencyProperty IconScaleProperty = DependencyProperty.RegisterAttached(
			"IconScale",
			typeof(IconScale),
			typeof(Browser),
			new FrameworkPropertyMetadata(OnIconScaleChnaged)
		);

		public static IconScale GetIconScale(DependencyObject dependencyObject)
		{
			return (IconScale)dependencyObject.GetValue(IconScaleProperty);
		}
		public static void SetIconScale(DependencyObject dependencyObject, IconScale value)
		{
			dependencyObject.SetValue(IconScaleProperty, value);
		}

		private static void OnIconScaleChnaged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var iconScale = GetIconScale(d);
			var element = d as FrameworkElement;
			if(element != null) {
				var size  = iconScale.ToSize();
				element.Width = size.Width;
				element.Height = size.Height;
			}
		}


	}
}

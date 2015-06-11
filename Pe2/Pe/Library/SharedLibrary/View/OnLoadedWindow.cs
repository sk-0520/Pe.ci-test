namespace ContentTypeTextNet.Library.SharedLibrary.View
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;

	public class OnLoadedWindow: Window
	{
		public OnLoadedWindow() :
			base()
		{
			Loaded += OnLoadedWindow_Loaded;
		}

		void OnLoadedWindow_Loaded(object sender, RoutedEventArgs e)
		{
			Loaded -= OnLoadedWindow_Loaded;
			OnLoaded(sender, e);
		}

		protected virtual void OnLoaded(object sender, RoutedEventArgs e)
		{ }
	}
}

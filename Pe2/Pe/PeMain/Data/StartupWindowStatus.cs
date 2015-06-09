namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
using System.Windows;

	public class StartupWindowStatus
	{
		#region define

		double startupOpacity = 0;
		Visibility startupVisibility = Visibility.Visible;

		#endregion

		public StartupWindowStatus(Window window)
		{
			Initialize(window);
		}

		#region property

		Window Window { get; set; }
		double Opacity { get; set; }
		Visibility Visibility { get; set; }

		#endregion

		#region function

		protected virtual void Initialize(Window window)
		{
			Window = window;

			Opacity = Window.Opacity;
			Visibility = Window.Visibility;

			Window.Opacity = startupOpacity;
			Window.Visibility = startupVisibility;
		}

		public virtual void UndoWindowState()
		{
			Window.Opacity = Opacity;
			Window.Visibility = Visibility;
		}

		#endregion
	}
}

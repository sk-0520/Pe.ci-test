namespace ContentTypeTextNet.Pe.PeMain.Logic.Property
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Pe.Library.PeData.IF;

	public static class WindowStatusProperty
	{
		public static double GetWindowLeft(IWindowStatus model)
		{
			return model.WindowLeft;
		}

		public static void SetWindowLeft(IWindowStatus model, double value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			if (model.WindowLeft != value && model.WindowState == WindowState.Normal) {
				model.WindowLeft = value;
				onPropertyChanged(propertyName);
			}
		}

		public static double GetWindowTop(IWindowStatus model)
		{
			return model.WindowTop;
		}

		public static void SetWindowTop(IWindowStatus model, double value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			if (model.WindowTop != value && model.WindowState == WindowState.Normal) {
				model.WindowTop = value;
				onPropertyChanged(propertyName);
			}
		}

		public static double GetWindowWidth(IWindowStatus model)
		{
			return model.WindowWidth;
		}

		public static void SetWindowWidth(IWindowStatus model, double value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			if (model.WindowWidth != value && model.WindowState == WindowState.Normal) {
				model.WindowWidth = value;
				onPropertyChanged(propertyName);
			}
		}

		public static double GetWindowHeight(IWindowStatus model)
		{
			return model.WindowHeight;
		}

		public static void SetWindowHeight(IWindowStatus model, double value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			if (model.WindowHeight != value && model.WindowState == WindowState.Normal) {
				model.WindowHeight = value;
				onPropertyChanged(propertyName);
			}
		}

		public static WindowState GetWindowState(IWindowStatus model)
		{
			return model.WindowState;
		}

		public static void SetWindowState(IWindowStatus model, WindowState value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			if (model.WindowState != value) {
				model.WindowState = value;
				onPropertyChanged(propertyName);
			}
		}
	}
}

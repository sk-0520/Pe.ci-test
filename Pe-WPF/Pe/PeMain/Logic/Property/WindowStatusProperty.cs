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
			return WindowAreaProperty.GetWindowLeft(model);
		}

		public static void SetWindowLeft(IWindowStatus model, double value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			if (model.WindowState == WindowState.Normal) {
				WindowAreaProperty.SetWindowLeft(model, value, onPropertyChanged, propertyName);
			}
		}

		public static double GetWindowTop(IWindowStatus model)
		{
			return WindowAreaProperty.GetWindowTop(model);
		}

		public static void SetWindowTop(IWindowStatus model, double value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			if (model.WindowState == WindowState.Normal) {
				WindowAreaProperty.SetWindowTop(model, value, onPropertyChanged, propertyName);
			}
		}

		public static double GetWindowWidth(IWindowStatus model)
		{
			return WindowAreaProperty.GetWindowWidth(model);
		}

		public static void SetWindowWidth(IWindowStatus model, double value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			if (model.WindowState == WindowState.Normal) {
				WindowAreaProperty.SetWindowWidth(model, value, onPropertyChanged, propertyName);
			}
		}

		public static double GetWindowHeight(IWindowStatus model)
		{
			return WindowAreaProperty.GetWindowHeight(model);
		}

		public static void SetWindowHeight(IWindowStatus model, double value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			if (model.WindowState == WindowState.Normal) {
				WindowAreaProperty.SetWindowHeight(model, value, onPropertyChanged, propertyName);
			}
		}

		public static WindowState GetWindowState(IWindowState model)
		{
			return WindowStateProperty.GetWindowState(model);
		}

		public static void SetWindowState(IWindowState model, WindowState value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			WindowStateProperty.SetWindowState(model, value, onPropertyChanged, propertyName);
		}

		public static bool GetTopMost(ITopMost model)
		{
			return TopMostProperty.GetTopMost(model);
		}

		public static void SetTopMost(ITopMost model, bool value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			TopMostProperty.SetTopMost(model, value, onPropertyChanged, propertyName);
		}
	}
}

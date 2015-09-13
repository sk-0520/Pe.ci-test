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

		public static bool SetWindowLeft(IWindowStatus model, double value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			if (model.WindowState == WindowState.Normal) {
				return WindowAreaProperty.SetWindowLeft(model, value, onPropertyChanged, propertyName);
			}

			return false;
		}

		public static double GetWindowTop(IWindowStatus model)
		{
			return WindowAreaProperty.GetWindowTop(model);
		}

		public static bool SetWindowTop(IWindowStatus model, double value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			if (model.WindowState == WindowState.Normal) {
				return WindowAreaProperty.SetWindowTop(model, value, onPropertyChanged, propertyName);
			}

			return false;
		}

		public static double GetWindowWidth(IWindowStatus model)
		{
			return WindowAreaProperty.GetWindowWidth(model);
		}

		public static bool SetWindowWidth(IWindowStatus model, double value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			if (model.WindowState == WindowState.Normal) {
				return WindowAreaProperty.SetWindowWidth(model, value, onPropertyChanged, propertyName);
			}

			return false;
		}

		public static double GetWindowHeight(IWindowStatus model)
		{
			return WindowAreaProperty.GetWindowHeight(model);
		}

		public static bool SetWindowHeight(IWindowStatus model, double value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			if (model.WindowState == WindowState.Normal) {
				return WindowAreaProperty.SetWindowHeight(model, value, onPropertyChanged, propertyName);
			}

			return false;
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

		public static bool SetTopMost(ITopMost model, bool value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			return TopMostProperty.SetTopMost(model, value, onPropertyChanged, propertyName);
		}
	}
}

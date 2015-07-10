namespace ContentTypeTextNet.Pe.PeMain.Logic.Property
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.PeData.IF;

	public static class WindowAreaProperty
	{
		public static double GetWindowLeft(IWindowArea model)
		{
			return model.WindowLeft;
		}

		public static void SetWindowLeft(IWindowArea model, double value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			if(model.WindowLeft != value) {
				model.WindowLeft = value;
				onPropertyChanged(propertyName);
			}
		}

		public static double GetWindowTop(IWindowArea model)
		{
			return model.WindowTop;
		}

		public static void SetWindowTop(IWindowArea model, double value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			if(model.WindowTop != value) {
				model.WindowTop = value;
				onPropertyChanged(propertyName);
			}
		}

		public static double GetWindowWidth(IWindowArea model)
		{
			return model.WindowWidth;
		}

		public static void SetWindowWidth(IWindowArea model, double value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			if(model.WindowWidth != value) {
				model.WindowWidth = value;
				onPropertyChanged(propertyName);
			}
		}

		public static double GetWindowHeight(IWindowArea model)
		{
			return model.WindowHeight;
		}

		public static void SetWindowHeight(IWindowArea model, double value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			if(model.WindowHeight != value) {
				model.WindowHeight = value;
				onPropertyChanged(propertyName);
			}
		}
	}
}

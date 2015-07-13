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

	public static class WindowStateProperty
	{
		public static WindowState GetWindowState(IWindowState model)
		{
			return model.WindowState;
		}

		public static bool SetWindowState(IWindowState model, WindowState value, Action<string> onPropertyChanged, [CallerMemberName] string propertyName = "")
		{
			if(model.WindowState != value) {
				model.WindowState = value;
				onPropertyChanged(propertyName);

				return true;
			}

			return false;
		}
	}
}

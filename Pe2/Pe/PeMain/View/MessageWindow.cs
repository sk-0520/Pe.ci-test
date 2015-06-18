namespace ContentTypeTextNet.Pe.PeMain.View
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.PInvoke.Windows;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.View.Parts.Window;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;

	/// <summary>
	/// 将来的に別ウィンドウを本体として機能移植する。
	/// </summary>
	public class MessageWindow : CommonDataWindow
	{
		public MessageWindow ()
		{
			WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
			WindowStyle = System.Windows.WindowStyle.None;
			Width = 0;
			Height = 0;
			ResizeMode = System.Windows.ResizeMode.NoResize;
			ShowInTaskbar = false;
		}


		#region ViewModelCommonDataWindow

		protected override void OnLoaded(object sender, RoutedEventArgs e)
		{
			base.OnLoaded(sender, e);
			Visibility = System.Windows.Visibility.Collapsed;
		}
		
		protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			switch(msg) {
				case (int)WM.WM_DEVICECHANGE:
					{
						var changedDevice = new ChangedDevice(hwnd, msg, wParam, lParam);
						CommonData.AppSender.SendDeviceChanged(changedDevice);
					}
					break;
			}

			return base.WndProc(hwnd, msg, wParam, lParam, ref handled);
		}

		#endregion
	}
}

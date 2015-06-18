namespace ContentTypeTextNet.Pe.PeMain.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Pe.PeMain.Data;

	public interface IAppSender
	{
		/// <summary>
		/// ウィンドウを追加。
		/// </summary>
		/// <param name="window"></param>
		void SendWindowAppend(Window window);
		/// <summary>
		/// デバイスが変更されたことを通知。
		/// </summary>
		/// <param name="changedDevice"></param>
		void SendDeviceChanged(ChangedDevice changedDevice);
	}
}

namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.PInvoke.Windows;

	public class ChangedDevice
	{
		#region varable

		IntPtr _hWnd;
		int _msg;
		IntPtr _wParam;
		IntPtr _lParam;

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public ChangedDevice(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
		{
			this._hWnd = hWnd;
			this._msg = msg;
			this._wParam = wParam;
			this._lParam = lParam;

			DBT = (DBT)this._wParam.ToInt32();
		}

		#region property

		/// <summary>
		/// DBT! DBT!
		/// </summary>
		public DBT DBT { get; private set; }

		#endregion
	}
}

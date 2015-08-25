namespace ContentTypeTextNet.Pe.Library.PeData.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;

	/// <summary>
	/// ウィンドウ状態を保持。
	/// </summary>
	public interface IWindowState
	{
		/// <summary>
		/// ウィンドウ状態。
		/// </summary>
		WindowState WindowState { get; set; }
	}
}

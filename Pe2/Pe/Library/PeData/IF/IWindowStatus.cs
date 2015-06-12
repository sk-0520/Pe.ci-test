namespace ContentTypeTextNet.Pe.Library.PeData.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;

	public interface IWindowStatus
	{
		/// <summary>
		/// 論理ウィンドウ座標(X)
		/// </summary>
		double WindowLeft { get; set; }
		/// <summary>
		/// 論理ウィンドウ座標(Y)
		/// </summary>
		double WindowTop { get; set; }
		/// <summary>
		/// 論理ウィンドウサイズ(横)
		/// </summary>
		double WindowWidth { get; set; }
		/// <summary>
		/// 論理ウィンドウサイズ(縦)
		/// </summary>
		double WindowHeight { get; set; }
		/// <summary>
		/// ウィンドウ状態。
		/// </summary>
		WindowState WindowState { get; set; }
	}
}

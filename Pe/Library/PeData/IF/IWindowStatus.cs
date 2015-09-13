namespace ContentTypeTextNet.Pe.Library.PeData.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.Attribute;
	using ContentTypeTextNet.Library.SharedLibrary.Define;

	/// <summary>
	/// 各種ウィンドウ状態を保持するマーカー。
	/// </summary>
	public interface IWindowStatus: IVisible, ITopMost, IWindowArea, IWindowState
	{ }
}

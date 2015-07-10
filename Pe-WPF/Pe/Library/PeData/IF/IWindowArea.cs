namespace ContentTypeTextNet.Pe.Library.PeData.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Attribute;
	using ContentTypeTextNet.Library.SharedLibrary.Define;

	public interface IWindowArea
	{
		/// <summary>
		/// 論理ウィンドウ座標(X)
		/// </summary>
		[PixelKind(Px.Logical)]
		double WindowLeft { get; set; }
		/// <summary>
		/// 論理ウィンドウ座標(Y)
		/// </summary>
		[PixelKind(Px.Logical)]
		double WindowTop { get; set; }
		/// <summary>
		/// 論理ウィンドウサイズ(横)
		/// </summary>
		[PixelKind(Px.Logical)]
		double WindowWidth { get; set; }
		/// <summary>
		/// 論理ウィンドウサイズ(縦)
		/// </summary>
		[PixelKind(Px.Logical)]
		double WindowHeight { get; set; }
	}
}

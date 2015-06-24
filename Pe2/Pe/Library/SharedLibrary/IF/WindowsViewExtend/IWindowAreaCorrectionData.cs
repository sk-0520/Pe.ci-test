namespace ContentTypeTextNet.Library.SharedLibrary.IF.WindowsViewExtend
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.Attribute;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF.Marker;

	public interface IWindowAreaCorrectionData : IWindowsViewExtendRestrictionViewModelMarker
	{
		/// <summary>
		/// ウィンドウサイズの倍数制御を行うか。
		/// </summary>
		bool UsingMultipleResize { get; }
		/// <summary>
		/// ウィンドウサイズの倍数制御に使用する元となる論理サイズ。
		/// </summary>
		[PixelKind(Px.Logical)]
		Size MultipleSize { get; }
		/// <summary>
		/// タイトルバーとかボーダーを含んだ領域。
		/// </summary>
		[PixelKind(Px.Logical)]
		Thickness MultipleThickness { get; }
		/// <summary>
		/// 移動制限を行うか。
		/// </summary>
		bool UsingMoveLimitArea { get; }
		/// <summary>
		/// 移動制限に使用する論理領域。
		/// </summary>
		[PixelKind(Px.Logical)]
		Rect MoveLimitArea { get; }
	}
}

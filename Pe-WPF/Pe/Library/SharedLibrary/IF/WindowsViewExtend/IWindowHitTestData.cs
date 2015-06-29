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

	public interface IWindowHitTestData : IWindowsViewExtendRestrictionViewModelMarker
	{
		/// <summary>
		/// ヒットテストを行うか
		/// </summary>
		bool UsingHitTest { get; }

		/// <summary>
		/// タイトルバーとして認識される領域。
		/// </summary>
		[PixelKind(Px.Logical)]
		Rect CaptionArea { get; }
		/// <summary>
		/// サイズ変更に使用する境界線。
		/// </summary>
		[PixelKind(Px.Logical)]
		Thickness ResizeThickness { get; }
	}
}

namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.Attribute;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	/// <summary>
	/// Windowsのウィンドウデータを保持する。
	/// </summary>
	public class WindowItemModel: PeDataBase, IName
	{
		/// <summary>
		/// 対象プロセス。
		/// </summary>
		public Process Process { get; set; }
		/// <summary>
		/// 対象ウィンドウハンドル。
		/// </summary>
		public IntPtr WindowHandle { get; set; }
		/// <summary>
		/// ウィンドウの領域。
		/// </summary>
		[PixelKind(Px.Device)]
		public Rect WindowArea { get; set; }

		#region IName

		public string Name { get; set; }

		#endregion
	}
}

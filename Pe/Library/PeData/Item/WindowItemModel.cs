namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Xml.Serialization;
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
		/// <summary>
		/// ウィンドウ状態。
		/// </summary>
		public WindowState WindowState { get;set;}

		#region IName

		[DataMember, XmlAttribute]
		public string Name { get; set; }

		#endregion
	}
}

namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;

	/// <summary>
	/// 使用データは全て物理ピクセル。
	/// </summary>
	[Serializable]
	public class ScreenModel: ModelBase
	{
		#region property
		/// <summary>
		/// 1 ピクセルのデータに関連付けられているメモリのビット数を取得します。
		/// </summary>
		public int BitsPerPixel { get; protected internal set; }
		/// <summary>
		/// ディスプレイの範囲を取得します。
		/// </summary>
		public Rect Bounds { get; protected internal set; }
		/// <summary>
		/// ディスプレイに関連付けられているデバイス名を取得します。
		/// </summary>
		public string DeviceName { get; protected internal set; }
		/// <summary>
		/// 特定のディスプレイがプライマリ デバイスかどうかを示す値を取得します。
		/// </summary>
		public bool Primary { get; protected internal set; }
		/// <summary>
		/// ディスプレイの作業領域を取得します。 作業領域とは、ディスプレイのデスクトップ領域からタスクバー、ドッキングされたウィンドウ、およびドッキングされたツール バーを除いた部分です。 
		/// </summary>
		public Rect WorkingArea { get; protected internal set; }

		#endregion
	}
}

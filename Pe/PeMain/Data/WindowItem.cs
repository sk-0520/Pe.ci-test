using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace ContentTypeTextNet.Pe.PeMain.Data
{
	/// <summary>
	/// ウィンドウ保存用アイテム。
	/// 
	/// シリアライズはつけるけどファイルには落さない。
	/// </summary>
	[Serializable]
	public class WindowItem: NameItem
	{
		public WindowItem()
		{ }
		
		public Process Process { get; set; }
		public IntPtr WindowHandle { get; set; }
		public Rectangle Rectangle { get; set; }
	}
	
	[Serializable]
	public class WindowListItem: NameItem
	{
		public WindowListItem()
		{
			Items = new List<WindowItem>();
		}
		
		public List<WindowItem> Items { get; set; }
	}
}

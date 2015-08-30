namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Media.Imaging;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	public static class NoteUtility
	{
		/// <summary>
		/// ノートメニュー表示テキスト
		/// </summary>
		/// <param name="indexItem"></param>
		/// <returns></returns>
		public static string MakeMenuText(NoteIndexItemModel indexItem)
		{
			return DisplayTextUtility.GetDisplayName(indexItem);
		}
		/// <summary>
		/// ノートメニューアイコンの生成。
		/// TODO: 未実装
		/// </summary>
		/// <param name="indexItem"></param>
		/// <returns></returns>
		public static FrameworkElement MakeMenuIcon(NoteIndexItemModel indexItem)
		{
			var size = IconScale.Small.ToSize();
			if(indexItem.IsCompacted) {
				size.Height /= 3;
			}
			var element = ImageUtility.CreateBox(indexItem.ForeColor, indexItem.BackColor, size);
			//var image = ImageUtility.MakeBitmapBitmapSourceDefualtDpi(element);
			//return image;
			return element;
		}
	}
}

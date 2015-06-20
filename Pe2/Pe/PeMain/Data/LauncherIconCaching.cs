namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media.Imaging;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	/// <summary>
	/// ランチャーアイテムのアイコン保存庫。
	/// </summary>
	public sealed class LauncherIconCaching: Dictionary<IconScale, Caching<LauncherItemModel, BitmapSource>>
	{
		public LauncherIconCaching()
		{
			foreach(var iconScale in EnumUtility.GetMembers<IconScale>()) {
				this.Add(iconScale, new Caching<LauncherItemModel, BitmapSource>());
			}
		}
	}
}

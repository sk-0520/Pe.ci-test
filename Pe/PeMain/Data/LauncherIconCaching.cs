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
	using ContentTypeTextNet.Pe.PeMain.Logic;

	/// <summary>
	/// ランチャーアイテムのアイコン保存庫。
	/// </summary>
	public sealed class LauncherIconCaching: IconCaching<LauncherItemModel>
	{
		public LauncherIconCaching()
			: base()
		{ }

		#region function

		public void Remove(LauncherItemModel model)
		{
			foreach(var cache in this.Values) {
				cache.Remove(model);
			}
		}

		#endregion
	}
}

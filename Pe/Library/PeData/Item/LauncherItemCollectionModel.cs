namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	/// <summary>
	/// ランチャーアイテム統括データ。
	/// </summary>
	[Serializable]
	public class LauncherItemCollectionModel: GuidCollectionBase<LauncherItemModel>
	{
		public LauncherItemCollectionModel()
			: base()
		{ }
	}
}

namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	public class LauncherToolbarDataModel : ItemModelBase
	{
		public LauncherToolbarDataModel()
			: this(new ToolbarItemModel(), new LauncherItemCollectionModel(), new LauncherGroupItemCollectionModel())
		{ }

		public LauncherToolbarDataModel(ToolbarItemModel toolbar, LauncherItemCollectionModel item, LauncherGroupItemCollectionModel group)
			:base()
		{
			Toolbar = toolbar;
			LauncherItems = item;
			GroupItems = group;
		}

		/// <summary>
		/// ツールバー設定。
		/// </summary>
		public ToolbarItemModel Toolbar { get; set; }
		/// <summary>
		/// ランチャーアイテム。
		/// </summary>
		public LauncherItemCollectionModel LauncherItems { get; set; }
		/// <summary>
		/// グループアイテム。
		/// </summary>
		public LauncherGroupItemCollectionModel GroupItems { get; set; }
	}
}

namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class LauncherToolbarItemModel : ItemModelBase
	{
		public LauncherToolbarItemModel()
			: this(new ToolbarItemModel(), new LauncherItemCollectionModel(), new LauncherGroupItemCollectionModel())
		{ }

		public LauncherToolbarItemModel(ToolbarItemModel toolbar, LauncherItemCollectionModel item, LauncherGroupItemCollectionModel group)
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

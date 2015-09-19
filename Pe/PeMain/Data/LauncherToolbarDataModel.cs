namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.Library.PeData.Setting;

	public class LauncherToolbarDataModel : ItemModelBase
	{
		//private LauncherToolbarDataModel()
		//	: this(new ToolbarItemModel(), new LauncherItemSettingModel(), new LauncherItemCollectionModel(), new LauncherGroupItemCollectionModel())
		//{ }

		public LauncherToolbarDataModel(ToolbarItemModel toolbar, LauncherItemSettingModel launcherItemSetting, LauncherItemCollectionModel item, LauncherGroupItemCollectionModel group)
			:base()
		{
			Toolbar = toolbar;
			LauncherItemSetting = launcherItemSetting;
			LauncherItems = item;
			GroupItems = group;
		}

		//public LauncherToolbarDataModel(LauncherItemSettingModel launcherItemSetting, LauncherItemCollectionModel item, LauncherGroupItemCollectionModel group)
		//	: this(null, launcherItemSetting, item, group)
		//{ }

		#region property

		/// <summary>
		/// ツールバー設定。
		/// </summary>
		public ToolbarItemModel Toolbar { get; private set; }
		/// <summary>
		/// ランチャーアイテム。
		/// </summary>
		public LauncherItemCollectionModel LauncherItems { get; private set; }
		/// <summary>
		/// ランチャー設定。
		/// </summary>
		public LauncherItemSettingModel LauncherItemSetting { get; private set; }
		/// <summary>
		/// グループアイテム。
		/// </summary>
		public LauncherGroupItemCollectionModel GroupItems { get; private set; }

		#endregion
	}
}

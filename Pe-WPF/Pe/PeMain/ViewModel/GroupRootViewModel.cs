namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Define;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class GroupRootViewModel: GroupViewModelBase<LauncherGroupItemModel>
	{
		#region variable
		CollectionModel<GroupItemViewMode> _nodes;
		#endregion

		public GroupRootViewModel(LauncherGroupItemModel model, LauncherItemCollectionModel items, IAppNonProcess appNonProcess)
			: base(model, appNonProcess)
		{
			Items = items;
		}

		#region proeprty

		LauncherItemCollectionModel Items { get; set; }

		public CollectionModel<GroupItemViewMode> Nodes
		{
			get
			{
				if (this._nodes == null) {
					var list = new List<GroupItemViewMode>(Model.LauncherItems.Count);
					foreach(var s in Model.LauncherItems) {
						if (Items.Contains(s)) {
							var item = Items[s];
							list.Add(new GroupItemViewMode(item, AppNonProcess));
						} else {
							//TODO: 表記
							var item = new LauncherItemModel();
							item.Id = s;
							item.Name = s.ToString("B");
							list.Add(new GroupItemViewMode(item, AppNonProcess));
						}
					}

					this._nodes = new CollectionModel<GroupItemViewMode>(list);
				}
				return this._nodes;
			}
		}

		#endregion

		#region IToolbarNode

		public override ToolbarNodeKind ToolbarNodeKind { get { return ToolbarNodeKind.Group; } }

		#endregion
	}
}

namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Define;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class GroupRootViewModel: GroupViewModelBase<LauncherGroupItemModel>
	{
		#region variable
		ObservableCollection<GroupItemViewMode> _nodes;
		#endregion

		public GroupRootViewModel(LauncherGroupItemModel model, LauncherItemCollectionModel items, LauncherIconCaching launcherIconCaching, INonProcess nonProcess)
			: base(model, launcherIconCaching, nonProcess)
		{
			Items = items;
		}

		#region proeprty

		LauncherItemCollectionModel Items { get; set; }

		public ObservableCollection<GroupItemViewMode> Nodes
		{
			get
			{
				if (this._nodes == null) {
					var list = new List<GroupItemViewMode>(Model.LauncherItems.Count);
					foreach(var s in Model.LauncherItems) {
						var item = Items[s];
						list.Add(new GroupItemViewMode(item, LauncherIconCaching, NonProcess));
					}

					this._nodes = new ObservableCollection<GroupItemViewMode>(list);
				}
				return this._nodes;
			}
		}

		public string Id { get { return Model.Id; } }

		#endregion

		#region IToolbarNode

		public override ToolbarNodeKind ToolbarNodeKind { get { return ToolbarNodeKind.Group; } }

		#endregion

		#region SingleModelWrapperViewModelBase

		protected override bool CanOutputModel { get { return true; } }

		public override string DisplayText { get { return DisplayTextUtility.GetDisplayName(Model); } }

		#endregion
	}
}

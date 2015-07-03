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

	public class GroupViewModel : SingleModelWrapperViewModelBase<LauncherGroupItemModel>, IHavingNonProcess, IHavingLauncherIconCaching, IToolbarNode
	{
		#region variable
		ObservableCollection<GroupItemViewMode> _nodes;
		#endregion

		public GroupViewModel(LauncherGroupItemModel group, LauncherItemCollectionModel items, LauncherIconCaching launcherIconCaching, INonProcess nonProcess)
			:base(group)
		{
			Items = items;

			LauncherIconCaching = launcherIconCaching;
			NonProcess = nonProcess;
		}

		#region proeprty

		LauncherItemCollectionModel Items { get; set; }

		#region IToolbarNode

		public ToolbarNodeKind ToolbarNodeKind { get { return ToolbarNodeKind.Group; } }

		#endregion

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

		#endregion

		#region IHavingLauncherIconCaching

		public LauncherIconCaching LauncherIconCaching { get; private set; }

		#endregion

		#region IHavingNonProcess

		public INonProcess NonProcess { get; private set; }

		#endregion

		#region SingleModelWrapperViewModelBase

		protected override bool CanOutputModel { get { return true; } }

		#endregion
	}
}

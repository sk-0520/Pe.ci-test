namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public class GroupViewModel : ViewModelBase, IHavingNonProcess, IHavingLauncherIconCaching
	{
		public GroupViewModel(LauncherGroupItemModel group, LauncherItemCollectionModel items, LauncherIconCaching launcherIconCaching, INonProcess nonProcess)
		{
			Group = group;
			Items = items;

			LauncherIconCaching = launcherIconCaching;
			NonProcess = nonProcess;
		}

		#region proeprty

		LauncherGroupItemModel Group { get; set; }
		LauncherItemCollectionModel Items { get; set; }

		#endregion

		#region IHavingLauncherIconCaching

		public LauncherIconCaching LauncherIconCaching { get; private set; }

		#endregion

		#region IHavingNonProcess

		public INonProcess NonProcess { get; private set; }

		#endregion
	}
}

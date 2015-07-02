namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class ToolbarViewModel : ViewModelBase, IHavingNonProcess
	{
		public ToolbarViewModel(ToolbarItemModel toolbarItemModel, LauncherGroupItemCollectionModel group, INonProcess nonProcess)
			: base()
		{
			Toolbar = toolbarItemModel;
			Group = group;
			NonProcess = nonProcess;
		}

		#region property

		ToolbarItemModel Toolbar { get; set; }
		LauncherGroupItemCollectionModel Group { get; set; }

		#endregion

		#region IHavingNonProcess

		public INonProcess NonProcess { get; private set; }

		#endregion

		#region SingleModelWrapperViewModelBase

		public override string DisplayText
		{
			get
			{
				return ScreenUtility.GetScreenName(Toolbar.Id, NonProcess.Logger);
			}
		}

		#endregion
	}
}

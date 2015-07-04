namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Define;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public abstract class GroupViewModelBase<TModel>: SingleModelWrapperViewModelBase<TModel>, IHavingNonProcess, IHavingLauncherIconCaching, IToolbarNode
		where TModel: IModel, ITId<string>, IName
	{
		public GroupViewModelBase(TModel model, LauncherIconCaching launcherIconCaching, INonProcess nonProcess)
			:base(model)
		{
			LauncherIconCaching = launcherIconCaching;
			NonProcess = nonProcess;
		}

		#region property

		public bool IsExpanded { get { return true; } }

		public string Id { get { return Model.Id; } }

		#endregion

		#region IToolbarNode

		public abstract ToolbarNodeKind ToolbarNodeKind { get; }

		#endregion

		#region IHavingLauncherIconCaching

		public LauncherIconCaching LauncherIconCaching { get; private set; }

		#endregion

		#region IHavingNonProcess

		public INonProcess NonProcess { get; private set; }

		#endregion

		#region SingleModelWrapperViewModelBase

		public override string DisplayText { get { return DisplayTextUtility.GetDisplayName(Model); } }

		#endregion
	}
}

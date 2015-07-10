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
		where TModel: IModel, ITId<Guid>, IName
	{
		#region variable

		bool _isSelected;

		#endregion

		public GroupViewModelBase(TModel model, LauncherIconCaching launcherIconCaching, INonProcess nonProcess)
			:base(model)
		{
			LauncherIconCaching = launcherIconCaching;
			NonProcess = nonProcess;
		}

		#region property

		public Guid Id { get { return Model.Id; } }

		#endregion

		#region IToolbarNode

		public abstract ToolbarNodeKind ToolbarNodeKind { get; }
		public bool IsExpanded { get { return true; } }
		public bool IsSelected {
			get { return this._isSelected; }
			set { SetVariableValue(ref this._isSelected, value); }
		}


		#endregion

		#region IHavingLauncherIconCaching

		public LauncherIconCaching LauncherIconCaching { get; private set; }

		#endregion

		#region IHavingNonProcess

		public INonProcess NonProcess { get; private set; }

		#endregion

		#region SingleModelWrapperViewModelBase

		public override string DisplayText { get { return DisplayTextUtility.GetDisplayName(Model); } }

		protected override bool CanOutputModel { get { return true; } }

		#endregion
	}
}

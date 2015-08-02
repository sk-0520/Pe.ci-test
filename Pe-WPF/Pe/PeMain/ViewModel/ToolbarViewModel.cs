namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Logic.Property;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;

	public class ToolbarViewModel : SingleModelWrapperViewModelBase<ToolbarItemModel>, IHavingAppNonProcess
	{
		public ToolbarViewModel(ToolbarItemModel toolbarItemModel, LauncherGroupItemCollectionModel group, IAppNonProcess appNonProcess)
			: base(toolbarItemModel)
		{
			Group = group;
			AppNonProcess = appNonProcess;
		}

		#region property

		LauncherGroupItemCollectionModel Group { get; set; }

		public DockType DockType
		{
			get { return Model.DockType; }
			set { SetModelValue(value); }
		}

		public IconScale IconScale
		{
			get { return Model.IconScale; }
			set { SetModelValue(value); }
		}

		public bool TextVisible
		{
			get { return Model.TextVisible; }
			set { SetModelValue(value); }
		}

		public double TextWidth
		{
			get { return Model.TextWidth; }
			set { SetModelValue(value); }
		}

		public bool AutoHide
		{
			get { return Model.AutoHide; }
			set { SetModelValue(value); }
		}

		#region ITopMost

		public bool IsTopmost
		{
			get { return TopMostProperty.GetTopMost(Model); }
			set { TopMostProperty.SetTopMost(Model, value, OnPropertyChanged); }
		}

		#endregion

		public bool Visible
		{
			get { return VisibleVisibilityProperty.GetVisible(Model); }
			set { VisibleVisibilityProperty.SetVisible(Model, value, OnPropertyChanged); }
		}

		#endregion

		#region IHavingAppNonProcess

		public IAppNonProcess AppNonProcess { get; set; }

		#endregion

		#region SingleModelWrapperViewModelBase

		public override string DisplayText
		{
			get
			{
				return ScreenUtility.GetScreenName(Model.Id, AppNonProcess.Logger);
			}
		}

		#endregion
	}
}

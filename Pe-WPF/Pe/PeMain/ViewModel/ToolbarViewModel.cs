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

	public class ToolbarViewModel : ViewModelBase, IHavingNonProcess, IHavingLauncherIconCaching
	{
		public ToolbarViewModel(ToolbarItemModel toolbarItemModel, LauncherGroupItemCollectionModel group, LauncherIconCaching launcherIconCaching, INonProcess nonProcess)
			: base(toolbarItemModel)
		{
			Group = group;
			LauncherIconCaching = launcherIconCaching;
			NonProcess = nonProcess;
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

		public bool TopMost
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

		#region IHavingNonProcess

		public INonProcess NonProcess { get; private set; }

		#endregion

		#region IHavingLauncherIconCaching

		public LauncherIconCaching LauncherIconCaching { get; private set; }

		#endregion

		#region SingleModelWrapperViewModelBase

		public override string DisplayText
		{
			get
			{
				return ScreenUtility.GetScreenName(Model.Id, NonProcess.Logger);
			}
		}

		#endregion
	}
}

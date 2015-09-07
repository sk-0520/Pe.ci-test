namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media.Imaging;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Define;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class GroupItemViewMode: GroupViewModelBase<LauncherItemModel>
	{
		public GroupItemViewMode(LauncherItemModel model, IAppNonProcess appNonProcess)
			: base(model, appNonProcess)
		{ }

		#region property

		public CollectionModel<GroupItemViewMode> Nodes
		{
			get { return null; }
		}

		#endregion

		#region IToolbarNode

		public override ToolbarNodeKind ToolbarNodeKind { get { return ToolbarNodeKind.Item; } }

		public override BitmapSource Image
		{
			get
			{
				return AppUtility.LoadLauncherItemIcon(IconScale.Small, Model, AppNonProcess.LauncherIconCaching, AppNonProcess);
			}
		}

		public override string Name
		{
			get { return Model.Name; }
			set { }
		}

		public override bool CanEdit { get { return false; } }


		#endregion

		#region GroupViewModelBase

		protected override void CallOnPropertyChangeDisplayItem()
		{
			base.CallOnPropertyChangeDisplayItem();
			OnPropertyChanged("Image");
		}

		#endregion
	}
}

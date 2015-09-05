namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media.Imaging;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class LauncherListItemViewModel: LauncherItemViewModelBase, IRefreshFromViewModel
	{
		public LauncherListItemViewModel(LauncherItemModel model, IAppNonProcess nonPorocess, IAppSender appSender)
			: base(model, nonPorocess, appSender)
		{ }

		#region property

		public BitmapSource Image
		{
			get { return GetIcon(IconScale.Small); }
		}

		public override string DisplayText
		{
			get
			{
				return DisplayTextUtility.GetDisplayName(Model);
			}
		}

		#endregion

		public void Refresh()
		{
			CallOnPropertyChangeDisplayItem();
			AppNonProcess.LauncherIconCaching.Remove(Model);
			OnPropertyChanged("Image");
		}
	}
}

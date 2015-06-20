namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media.Imaging;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class LauncherViewModelBase: SingleModelWrapperViewModelBase<LauncherItemModel>
	{
		public LauncherViewModelBase(LauncherItemModel model, LauncherIconCaching launcherIconCaching)
			: base(model)
		{
			LauncherIcons = launcherIconCaching;
		}

		#region property

		protected LauncherIconCaching LauncherIcons { get; private set; }

		#endregion

		#region function

		public BitmapSource GetIcon(IconScale iconScale, ILogger logger = null)
		{
			CheckUtility.DebugEnforceNotNull(LauncherIcons);

			return LauncherIcons[iconScale].Get(Model, () => LauncherItemUtility.GetIcon(Model, iconScale, logger));
		}

		#endregion
	}
}

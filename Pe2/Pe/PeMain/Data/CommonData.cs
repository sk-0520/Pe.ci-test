namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.Library.PeData.Setting;

	public sealed class CommonData: DisposeFinalizeModelBase
	{
		public CommonData()
			: base()
		{ }

		#region property

		public MainSettingModel MainSetting { get; set; }
		public LauncherItemSettingModel LauncherItemSetting { get; set; }
		public LauncherGroupItemSettingModel LauncherGroupItemSetting { get; set; }
		public LanguageCollectionViewModel Language { get; set; }
		public ILogger Logger { get; set; }

		#endregion

		#region DisposeFinalizeModelBase

		protected override void Dispose(bool disposing)
		{
			if (!IsDisposed) {
				MainSetting.Dispose();
				LauncherItemSetting.Dispose();
				LauncherGroupItemSetting.Dispose();
				Logger.Dispose();
			}
			base.Dispose(disposing);
		}

		#endregion
	}
}

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
		public CommonData(MainSettingModel mainSetting, LauncherItemSettingModel launcherItemSetting, LauncherGroupItemSettingModel launcherGroupItemSetting, LanguageCollectionModel language, string languageFilePath, ILogger logger)
		{
			MainSetting = mainSetting;
		
			LauncherItemSetting = launcherItemSetting;
			LauncherGroupItemSetting = launcherGroupItemSetting;
			Language = new LanguageCollectionViewModel(language, languageFilePath);

			Logger = logger;
		}

		#region property

		public MainSettingModel MainSetting { get; private set; }
		public LauncherItemSettingModel LauncherItemSetting { get; private set; }
		public LauncherGroupItemSettingModel LauncherGroupItemSetting { get; private set; }
		public LanguageCollectionViewModel Language { get; private set; }
		public ILogger Logger { get; private set; }

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

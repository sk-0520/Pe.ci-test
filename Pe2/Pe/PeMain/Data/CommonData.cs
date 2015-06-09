namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;

	public sealed class CommonData
	{
		public CommonData(MainSettingModel mainSetting, LauncherItemSettingModel launcherItemSetting, LauncherGroupItemSettingModel launcherGroupItemSetting, LanguageCollectionModel language)
		{
			MainSetting = mainSetting;
		
			LauncherItemSetting = launcherItemSetting;
			LauncherGroupItemSetting = launcherGroupItemSetting;
			Language = new LanguageCollectionViewModel(language);
		}

		public MainSettingModel MainSetting { get; private set; }
		public LauncherItemSettingModel LauncherItemSetting { get; private set; }
		public LauncherGroupItemSettingModel LauncherGroupItemSetting { get; private set; }
		public LanguageCollectionViewModel Language { get; private set; }
	}
}

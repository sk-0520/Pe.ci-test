namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;

	public class MainSettingViewModel: SettingPageViewModelBase
	{
		public MainSettingViewModel(RunningInformationSettingModel runningInformation, LanguageSettingModel language, LoggingSettingModel logging, INonProcess nonProcess, VariableConstants variableConstants, SettingNotifiyItem settingNotifiyItem)
			: base(nonProcess, variableConstants, settingNotifiyItem)
		{
			RunningInformation = runningInformation;
			Language = language;
			Logging = logging;
		}

		#region property

		RunningInformationSettingModel RunningInformation { get; set; }
		LanguageSettingModel Language { get; set; }
		LoggingSettingModel Logging { get; set; }

		public bool Startup
		{
			get
			{
				if(!SettingNotifiyItem.StartupRegist.HasValue) {
					var path = Environment.ExpandEnvironmentVariables(Constants.startupShortcutPath);
					SettingNotifiyItem.StartupRegist = File.Exists(path);
				}

				return SettingNotifiyItem.StartupRegist.Value;
			}
			set
			{
				if (SettingNotifiyItem.StartupRegist != value) {
					SettingNotifiyItem.StartupRegist = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion
	}
}

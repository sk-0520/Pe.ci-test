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

	public class MainSettingViewModel: SettingPageViewModelBase
	{
		#region variable

		bool? _startup = null;

		#endregion

		public MainSettingViewModel(RunningInformationSettingModel runningInformation, LanguageSettingModel language, LoggingSettingModel logging, INonProcess nonProcess, VariableConstants variableConstants)
			: base(nonProcess, variableConstants)
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
				if(!this._startup.HasValue) {
					var path = Environment.ExpandEnvironmentVariables(Constants.startupShortcutPath);
					this._startup = File.Exists(path);
				}

				return this._startup.Value;
			}
			set
			{
				if(this._startup != value) {
					this._startup = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion
	}
}

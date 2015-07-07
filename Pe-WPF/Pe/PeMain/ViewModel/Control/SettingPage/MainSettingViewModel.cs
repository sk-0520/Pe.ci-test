namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

	public class MainSettingViewModel: SettingPageViewModelBase
	{
		public MainSettingViewModel(RunningInformationSettingModel runningInformation, LanguageSettingModel language, LoggingSettingModel logging, INonProcess nonProcess)
			: base(nonProcess)
		{
			RunningInformation = runningInformation;
			Language = language;
			Logging = logging;
		}

		#region property

		RunningInformationSettingModel RunningInformation { get; set; }
		LanguageSettingModel Language { get; set; }
		LoggingSettingModel Logging { get; set; }

		#endregion
	}
}

namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;

	public class MainSettingViewModel : SettingPageViewModelBase
	{
		public MainSettingViewModel(RunningInformationSettingModel runningInformation, LanguageSettingModel language, LoggingSettingModel logging, SystemEnvironmentSettingModel systemEnvironment, INonProcess nonProcess, VariableConstants variableConstants, SettingNotifiyItem settingNotifiyItem)
			: base(nonProcess, variableConstants, settingNotifiyItem)
		{
			RunningInformation = runningInformation;
			Language = language;
			Logging = logging;
			SystemEnvironment = systemEnvironment;
		}

		#region property

		RunningInformationSettingModel RunningInformation { get; set; }
		LanguageSettingModel Language { get; set; }
		LoggingSettingModel Logging { get; set; }
		SystemEnvironmentSettingModel SystemEnvironment { get; set; }

		public bool Startup
		{
			get
			{
				if (!SettingNotifiyItem.StartupRegist.HasValue) {
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



		#region logging

		public bool LogVisible
		{
			get { return Logging.Visible; }
			set { SetPropertyValue(Logging, value, "Visible"); }
		}

		public bool LogAddShow
		{
			get { return Logging.AddShow; }
			set { SetPropertyValue(Logging, value, "AddShow"); }
		}

		public bool LogTriggerDebug
		{
			get { return Logging.ShowTriggerDebug; }
			set { SetPropertyValue(Logging, value, "ShowTriggerDebug"); }
		}
		public bool LogTriggerTrace
		{
			get { return Logging.ShowTriggerTrace; }
			set { SetPropertyValue(Logging, value, "ShowTriggerTrace"); }
		}
		public bool LogTriggerInformation
		{
			get { return Logging.ShowTriggerInformation; }
			set { SetPropertyValue(Logging, value, "ShowTriggerInformation"); }
		}
		public bool LogTriggerWarning
		{
			get { return Logging.ShowTriggerWarning; }
			set { SetPropertyValue(Logging, value, "ShowTriggerWarning"); }
		}
		public bool LogTriggerError
		{
			get { return Logging.ShowTriggerError; }
			set { SetPropertyValue(Logging, value, "ShowTriggerError"); }
		}
		public bool LogTriggerFatal
		{
			get { return Logging.ShowTriggerFatal; }
			set { SetPropertyValue(Logging, value, "ShowTriggerFatal"); }
		}

		#endregion

		#region runnunginfo

		public bool RunningCheckUpdateRelease
		{
			get { return RunningInformation.CheckUpdateRelease; }
			set { SetPropertyValue(RunningInformation, value, "CheckUpdateRelease"); }
		}

		public bool RunningCheckUpdateRC
		{
			get { return RunningInformation.CheckUpdateRC; }
			set { SetPropertyValue(RunningInformation, value, "CheckUpdateRC"); }
		}

		#endregion

		#region

		public HotKeyModel SysEnvHideFileHotkey
		{
			get { return SystemEnvironment.HideFileHotkey; }
			set { SetPropertyValue(SystemEnvironment, value, "HideFileHotkey"); }
		}

		public HotKeyModel SysEnvExtensionHotkey
		{
			get { return SystemEnvironment.ExtensionHotkey; }
			set { SetPropertyValue(SystemEnvironment, value, "ExtensionHotkey"); }
		}

		#endregion

		#endregion
	}
}

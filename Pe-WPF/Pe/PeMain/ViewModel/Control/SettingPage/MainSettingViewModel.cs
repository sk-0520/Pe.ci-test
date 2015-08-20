namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Property;

	public class MainSettingViewModel : SettingPageViewModelBase
	{
		public MainSettingViewModel(RunningInformationSettingModel runningInformation, LanguageSettingModel language, LoggingSettingModel logging, SystemEnvironmentSettingModel systemEnvironment, StreamSettingModel stream, WindowSaveSettingModel windowSave, IAppNonProcess appNonProcess, SettingNotifiyItem settingNotifiyItem)
			: base(appNonProcess, settingNotifiyItem)
		{
			RunningInformation = runningInformation;
			Language = language;
			Logging = logging;
			SystemEnvironment = systemEnvironment;
			Stream = stream;
			WindowSave = windowSave;
		}

		#region property

		RunningInformationSettingModel RunningInformation { get; set; }
		LanguageSettingModel Language { get; set; }
		LoggingSettingModel Logging { get; set; }
		SystemEnvironmentSettingModel SystemEnvironment { get; set; }
		StreamSettingModel Stream { get; set; }
		WindowSaveSettingModel WindowSave { get; set; }

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
			get { return Logging.IsVisible; }
			set { SetPropertyValue(Logging, value, "IsVisible"); }
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

		#region SystemEnvironment

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

		#region StreamSettingModel

		public Color StdOutputForeColor
		{
			get { return Stream.OutputColor.ForeColor; }
			set { SetPropertyValue(Stream.OutputColor, value, "ForeColor"); }
		}
		public Color StdOutputBackColor
		{
			get { return Stream.OutputColor.BackColor; }
			set { SetPropertyValue(Stream.OutputColor, value, "BackColor"); }
		}

		public Color ErrOutputForeColor
		{
			get { return Stream.ErrorColor.ForeColor; }
			set { SetPropertyValue(Stream.ErrorColor, value, "ForeColor"); }
		}
		public Color ErrOutputBackColor
		{
			get { return Stream.ErrorColor.BackColor; }
			set { SetPropertyValue(Stream.ErrorColor, value, "BackColor"); }
		}

		//public FontFamily StreamFontFamily
		//{
		//	get { return FontUtility.MakeFontFamily(Stream.Font.Family, SystemFonts.MessageFontFamily); }
		//	set
		//	{
		//		if(value != null) {
		//			var fontFamily = FontUtility.GetOriginalFontFamilyName(value);
		//			SetPropertyValue(Stream.Font, fontFamily, "Family");
		//		}
		//	}
		//}

		//public bool StreamFontBold
		//{
		//	get { return Stream.Font.Bold; }
		//	set { SetPropertyValue(Stream.Font, value, "Bold"); }
		//}

		//public bool StreamFontItalic
		//{
		//	get { return Stream.Font.Italic; }
		//	set { SetPropertyValue(Stream.Font, value, "Italic"); }
		//}

		//public double StreamFontSize
		//{
		//	get { return Stream.Font.Size; }
		//	set { SetPropertyValue(Stream.Font, value, "Size"); }
		//}
		#region font

		public FontFamily StreamFontFamily
		{
			get { return FontModelProperty.GetFamilyDefault(Stream.Font); }
			set { FontModelProperty.SetFamily(Stream.Font, value, OnPropertyChanged); }
		}

		public bool StreamFontBold
		{
			get { return FontModelProperty.GetBold(Stream.Font); }
			set { FontModelProperty.SetBold(Stream.Font, value, OnPropertyChanged); }
		}

		public bool StreamFontItalic
		{
			get { return FontModelProperty.GetItalic(Stream.Font); }
			set { FontModelProperty.SetItalic(Stream.Font, value, OnPropertyChanged); }
		}

		public double StreamFontSize
		{
			get { return FontModelProperty.GetSize(Stream.Font); }
			set { FontModelProperty.SetSize(Stream.Font, value, OnPropertyChanged); }
		}

		#endregion

		#endregion

		#region WindowSaveSettingModel

		public bool WindowSaveIsEnabled
		{
			get { return WindowSave.IsEnabled; }
			set { SetPropertyValue(WindowSave, value, "IsEnabled"); }
		}

		public TimeSpan WindowSaveIntervalTime
		{
			get { return WindowSave.SaveIntervalTime; }
			set { SetPropertyValue(WindowSave, value, "SaveIntervalTime"); }
		}

		public int WindowSaveCount
		{
			get { return WindowSave.SaveCount; }
			set { SetPropertyValue(WindowSave, value, "SaveCount"); }
		}


		#endregion

		#endregion
	}
}

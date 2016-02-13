/**
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Data;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.Library.PeData.Setting;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic.Property;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
using ContentTypeTextNet.Pe.PeMain.View.Parts.Control.SettingPage;

namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
    public class MainSettingViewModel: SettingPageViewModelBase<MainSettingControl>
    {
        #region variavle

        IEnumerable<DisplayData<string>> _languageList;
        int _languageSelectedIndex = -1;

        #endregion

        public MainSettingViewModel(RunningInformationSettingModel runningInformation, LanguageSettingModel language, LoggingSettingModel logging, SystemEnvironmentSettingModel systemEnvironment, StreamSettingModel stream, WindowSaveSettingModel windowSave, GeneralSettingModel general, MainSettingControl view, IAppNonProcess appNonProcess, SettingNotifyData settingNotifiyItem)
            : base(view, appNonProcess, settingNotifiyItem)
        {
            RunningInformation = runningInformation;
            Language = language;
            Logging = logging;
            SystemEnvironment = systemEnvironment;
            Stream = stream;
            WindowSave = windowSave;
            General = general;
        }

        #region property

        RunningInformationSettingModel RunningInformation { get; set; }
        LanguageSettingModel Language { get; set; }
        LoggingSettingModel Logging { get; set; }
        SystemEnvironmentSettingModel SystemEnvironment { get; set; }
        StreamSettingModel Stream { get; set; }
        WindowSaveSettingModel WindowSave { get; set; }
        GeneralSettingModel General { get; set; }

        public bool Startup
        {
            get
            {
                if(!SettingNotifiyItem.StartupRegist.HasValue) {
                    var path = Environment.ExpandEnvironmentVariables(Constants.StartupShortcutPath);
                    SettingNotifiyItem.StartupRegist = File.Exists(path);
                }

                return SettingNotifiyItem.StartupRegist.Value;
            }
            set
            {
                if(SettingNotifiyItem.StartupRegist != value) {
                    SettingNotifiyItem.StartupRegist = value;
                    OnPropertyChanged();
                }
            }
        }

        public IEnumerable<DisplayData<string>> LanguageList
        {
            get
            {
                if(this._languageList == null) {
                    var list = AppUtility.GetLanguageFiles(Constants.ApplicationLanguageDirectoryPath, AppNonProcess.Logger)
                        .Where(p => string.Compare(Path.GetFileName(p.Value.Key), Constants.languageDefaultFileName, true) != 0)
                    ;
                    //this._languageList = list.Select(
                    //	p => new ListItem<LanguageSettingModel>(
                    //		string.Format("{0}({1})", p.Value.Value.Name, p.Value.Value.CultureCode),
                    //		new LanguageSettingModel() { Name = p.Value.Value.CultureCode }
                    //	)
                    //);
                    bool isSelectedLanguage = false;
                    var langList = new List<DisplayData<string>>();
                    foreach(var item in list.Select((l, i) => new { Language = l, Index = i })) {
                        var displayText = string.Format("{0}({1})", item.Language.Value.Value.Name, item.Language.Value.Value.CultureCode);
                        if(!isSelectedLanguage && item.Language.Value.Value.Name == Language.Name) {
                            this._languageSelectedIndex = item.Index;
                            langList.Add(new DisplayData<string>(displayText, Language.Name));
                            isSelectedLanguage = true;
                        } else {
                            var i = new DisplayData<string>(
                                displayText,
                                item.Language.Value.Value.Name
                            );
                            langList.Add(i);
                        }
                    }
                    this._languageList = langList;
                }

                return this._languageList;
            }
        }

        public string SelectedLanguage
        {
            get { return Language.Name; }
            set
            {
                Language.Name = value;
                OnPropertyChanged();
            }
        }

        public int LanguageSelectedIndex
        {
            get { return this._languageSelectedIndex; }
            set { SetVariableValue(ref _languageSelectedIndex, value); }
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
            set { SetPropertyValue(RunningInformation, value, nameof(RunningInformation.CheckUpdateRelease)); }
        }

        public bool RunningCheckUpdateRC
        {
            get { return RunningInformation.CheckUpdateRC; }
            set { SetPropertyValue(RunningInformation, value, nameof(RunningInformation.CheckUpdateRC)); }
        }

        public string UserId
        {
            get { return RunningInformation.UserId; }
            set { SetPropertyValue(RunningInformation, value); }
        }
        
        public bool SendPersonalInformation
        {
            get { return RunningInformation.SendPersonalInformation; }
            set { SetPropertyValue(RunningInformation, value); }
        }

        #endregion

        #region SystemEnvironment

        public HotKeyModel SysEnvHideFileHotkey
        {
            get { return SystemEnvironment.HideFileHotkey; }
            set { SetPropertyValue(SystemEnvironment, value, nameof(SystemEnvironment.HideFileHotkey)); }
        }

        public HotKeyModel SysEnvExtensionHotkey
        {
            get { return SystemEnvironment.ExtensionHotkey; }
            set { SetPropertyValue(SystemEnvironment, value, nameof(SystemEnvironment.ExtensionHotkey)); }
        }

        #endregion

        #region StreamSettingModel

        public Color StdOutputForeColor
        {
            get { return Stream.OutputColor.ForeColor; }
            set { SetPropertyValue(Stream.OutputColor, value, nameof(Stream.OutputColor.ForeColor)); }
        }
        public Color StdOutputBackColor
        {
            get { return Stream.OutputColor.BackColor; }
            set { SetPropertyValue(Stream.OutputColor, value, nameof(Stream.OutputColor.BackColor)); }
        }

        public Color ErrOutputForeColor
        {
            get { return Stream.ErrorColor.ForeColor; }
            set { SetPropertyValue(Stream.ErrorColor, value, nameof(Stream.ErrorColor.ForeColor)); }
        }
        public Color ErrOutputBackColor
        {
            get { return Stream.ErrorColor.BackColor; }
            set { SetPropertyValue(Stream.ErrorColor, value, nameof(Stream.ErrorColor.BackColor)); }
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
            set { SetPropertyValue(WindowSave, value, nameof(WindowSave.IsEnabled)); }
        }

        public TimeSpan WindowSaveIntervalTime
        {
            get { return WindowSave.SaveIntervalTime; }
            set { SetPropertyValue(WindowSave, value, nameof(WindowSave.SaveIntervalTime)); }
        }

        public int WindowSaveCount
        {
            get { return WindowSave.SaveCount; }
            set { SetPropertyValue(WindowSave, value, nameof(WindowSave.SaveCount)); }
        }


        #endregion

        #region GeneralSettingModel

        public Notification Notification
        {
            get { return General.Notification; }
            set { SetPropertyValue(General, value); }
        }

        #endregion

        #endregion

        #region command

        #region runnunginfo

        public ICommand CreateUserIdFromEnvironmentCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        UserId = SettingUtility.CreateUserIdFromEnvironment();
                    }
                );

                return result;
            }
        }

        public ICommand CreateUserIdFromRandomCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        UserId = SettingUtility.CreateUserIdFromDateTime(DateTime.Now);
                    }
                );

                return result;
            }
        }

        #endregion

        #endregion
    }
}

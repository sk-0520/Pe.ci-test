using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Embedded.Abstract;
using ContentTypeTextNet.Pe.Plugins.Reference.FileFinder.Models.Data;
using ContentTypeTextNet.Pe.Plugins.Reference.FileFinder.ViewModels;
using ContentTypeTextNet.Pe.Plugins.Reference.FileFinder.Views;

namespace ContentTypeTextNet.Pe.Plugins.Reference.FileFinder.Preferences
{
    public class FileFinderPreferences: PreferencesBase
    {
        public FileFinderPreferences(IPlugin plugin)
            : base(plugin)
        { }

        #region proeprty

        private FileFinderSettingViewModel? SettingViewModel { get; set; }

        #endregion

        #region function
        #endregion

        #region PreferencesBase

        public override UserControl BeginPreferences(IPreferencesLoadContext preferencesLoadContext, IPreferencesParameter preferencesParameter)
        {
            FileFinderSetting? setting;
            if(!preferencesLoadContext.Storage.Persistence.Normal.TryGet<FileFinderSetting>(FileFinderConstants.MainSettengKey, out setting)) {
                setting = new FileFinderSetting();
            }

            SettingViewModel = new FileFinderSettingViewModel(setting, preferencesParameter.SkeletonImplements, preferencesParameter.DispatcherWrapper, preferencesParameter.LoggerFactory);

            var control = new FileFinderSettingControl() {
                DataContext = SettingViewModel,
            };

            return control;
        }

        public override void CheckPreferences(IPreferencesCheckContext preferencesCheckContext)
        {
        }

        public override void SavePreferences(IPreferencesSaveContext preferencesSaveContext)
        {
            Debug.Assert(SettingViewModel != null);

            var setting = new FileFinderSetting() {
                IncludeHiddenFile = SettingViewModel.IncludeHiddenFile,
                IncludePath = SettingViewModel.IncludePath,
                MaximumPathItem = SettingViewModel.MaximumPathItem,
                PathEnabledInputCharCount = SettingViewModel.PathEnabledInputCharCount,
            };
            preferencesSaveContext.Storage.Persistence.Normal.Set(FileFinderConstants.MainSettengKey, setting);
        }

        public override void EndPreferences(IPreferencesEndContext preferencesEndContext)
        {
            SettingViewModel = null;
        }

        #endregion
    }
}

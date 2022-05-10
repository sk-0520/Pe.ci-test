using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public class LauncherItemCustomizeAddonViewModel: LauncherItemCustomizeDetailViewModelBase
    {
        public LauncherItemCustomizeAddonViewModel(LauncherItemCustomizeEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        { }

        #region property

        public UserControl? PreferencesControl { get; set; }
        private INotifyPropertyChanged? PreferencesNotifyPropertyChanged { get; set; }
        public string LauncherItemHeader => Model.GetLauncherItemPluginHeader();

        public bool LauncherItemSupportedPreferences => Model.LauncherItemSupportedPreferences;

        #endregion

        #region command

        #endregion

        #region function

        #endregion

        #region LauncherItemCustomizeDetailViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(PreferencesNotifyPropertyChanged != null) {
                    PreferencesNotifyPropertyChanged.PropertyChanged -= NotifyPropertyChanged_PropertyChanged;
                }
                PreferencesControl = null;
            }

            base.Dispose(disposing);
        }

        protected override void InitializeImpl()
        {
            if(Model.IsLazyLoad) {
                return;
            }

            if(Model.LauncherItemSupportedPreferences) {
                Debug.Assert(PreferencesControl == null);

                PreferencesControl = Model.BeginLauncherItemPreferences();
                if(PreferencesControl.DataContext is INotifyPropertyChanged notifyPropertyChanged) {
                    PreferencesNotifyPropertyChanged = notifyPropertyChanged;
                    PreferencesNotifyPropertyChanged.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
                }
            }
        }

        protected override void ValidateDomain()
        {
            base.ValidateDomain();

            if(PreferencesControl != null) {
                var hasError = Model.CheckLauncherItemPreferences();
                if(hasError) {
                    //TODO: エラーがあった際にどうすべきか
                    Logger.LogError("[TODO] エラーあり: {0}", LauncherItemId);
                    AddError("error", string.Empty);
                }
            }
        }

        #endregion

        private void NotifyPropertyChanged_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(e.PropertyName);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class PluginsSettingEditorViewModel: SettingEditorViewModelBase<PluginsSettingEditorElement>
    {
        #region variable

        PluginSettingEditorViewModel? _selectedPlugin;

        #endregion

        public PluginsSettingEditorViewModel(PluginsSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            PluginCollection = new ActionModelViewModelObservableCollectionManager<PluginSettingEditorElement, PluginSettingEditorViewModel>(Model.PluginItems) {
                ToViewModel = m => new PluginSettingEditorViewModel(m, DispatcherWrapper, LoggerFactory),
            };
            PluginItems = PluginCollection.GetDefaultView();
        }

        #region property

        ModelViewModelObservableCollectionManagerBase<PluginSettingEditorElement, PluginSettingEditorViewModel> PluginCollection { get; }
        public ICollectionView PluginItems { get; }

        public PluginSettingEditorViewModel? SelectedPlugin
        {
            get => this._selectedPlugin;
            set => SetProperty(ref this._selectedPlugin, value);
        }

        #endregion

        #region command

        #endregion

        #region function

        #endregion

        #region SettingEditorViewModelBase
        public override string Header => Properties.Resources.String_Setting_Plugins_Header;

        public override void Flush()
        { }

        public override void Refresh()
        { }

        protected override void ValidateDomain()
        {
            base.ValidateDomain();

            foreach(var plugin in PluginCollection.ViewModels) {
                plugin.Validate();
            }
        }


        #endregion

    }
}

using System;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public class LauncherItemCustomizeStoreAppViewModel: LauncherItemCustomizeDetailViewModelBase
    {
        public LauncherItemCustomizeStoreAppViewModel(LauncherItemCustomizeEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            if(Model.Kind != Models.Data.LauncherItemKind.StoreApp) {
                throw new ArgumentException(null, nameof(model) + "." + nameof(Model.Kind));
            }
            if(Model.StoreApp == null) {
                throw new ArgumentNullException(nameof(model) + "." + nameof(Model.StoreApp));
            }
            StoreApp = Model.StoreApp;
        }

        #region property

        private LauncherStoreAppData StoreApp { get; }

        public string ProtocolAlias
        {
            get => StoreApp.ProtocolAlias;
            set => SetPropertyValue(StoreApp, value);
        }

        public string Option
        {
            get => StoreApp.Option;
            set => SetPropertyValue(StoreApp, value);
        }

        #endregion

        #region command

        #endregion

        #region function

        #endregion

        #region LauncherItemCustomizeDetailViewModelBase

        protected override void InitializeImpl()
        {
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Element.CustomizeLauncherItem;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherItem;

namespace ContentTypeTextNet.Pe.Main.ViewModel.CustomizeLauncherItem
{
    public class CustomizeLauncherItemViewModel : SingleModelViewModelBase<CustomizeLauncherItemElement>, IViewLifecycleReceiver
    {
        #region variable

        List<CustomizeLauncherDetailViewModelBase> _customizeItems;

        #endregion

        public CustomizeLauncherItemViewModel(CustomizeLauncherItemElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        public List<CustomizeLauncherDetailViewModelBase> CustomizeItems
        {
            get
            {
                if(this._customizeItems == null) {
                    this._customizeItems = CreateCustomizeItems().ToList();
                    foreach(var item in this._customizeItems) {
                        item.Initialize();
                    }
                }

                return this._customizeItems;
            }
        }

        #endregion

        #region command
        #endregion

        #region function

        IEnumerable<CustomizeLauncherDetailViewModelBase> CreateCustomizeItems()
        {
            yield return new CustomizeLauncherCommonViewModel(Model, Logger.Factory);

            switch(Model.Kind) {
                case LauncherItemKind.File:
                    yield return new CustomizeLauncherFileViewModel(Model, Logger.Factory);
                    break;
            }
        }

        #endregion

        #region CustomizeLauncherItemViewModel
        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        { }

        public void ReceiveViewLoaded(Window window)
        { }

        public void ReceiveViewUserClosing(CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewUserClosing();
        }


        public void ReceiveViewClosing(CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewClosing();
        }

        public void ReceiveViewClosed()
        {
            Model.ReceiveViewClosed();
        }

        #endregion
    }
}

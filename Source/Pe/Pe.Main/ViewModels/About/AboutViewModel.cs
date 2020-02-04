using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.About;
using ContentTypeTextNet.Pe.Main.Models.UsageStatistics;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.About
{
    public class AboutViewModel : ElementViewModelBase<AboutElement>
    {
        public AboutViewModel(AboutElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
            ComponentCollection = new ObservableCollection<AboutComponentItemViewModel>(model.Components.Select(i => new AboutComponentItemViewModel(i, LoggerFactory)));
            ComponentItems = CollectionViewSource.GetDefaultView(ComponentCollection);
            ComponentItems.GroupDescriptions.Add(new PropertyGroupDescription(nameof(AboutComponentItemViewModel.Kind)));
            ComponentItems.SortDescriptions.Add(new SortDescription(nameof(AboutComponentItemViewModel.Sort), ListSortDirection.Ascending));
        }

        #region property

        public RequestSender CloseRequest { get; } = new RequestSender();
        ObservableCollection<AboutComponentItemViewModel> ComponentCollection { get; }
        public ICollectionView ComponentItems { get; }

        #endregion

        #region command

        public ICommand OpenLicenseCommand => GetOrCreateCommand(() => new DelegateCommand<AboutComponentItemViewModel>(
            o => {
                Model.OpenUri(o.LicenseUri);
            }
        ));

        public ICommand OpenUriCommand => GetOrCreateCommand(() => new DelegateCommand<AboutComponentItemViewModel>(
            o => {
                Model.OpenUri(o.Uri);
            }
        ));
        public ICommand OpenForumUriCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.OpenForumUri();
            }
        ));
        public ICommand OpenProjectUriCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.OpenProjectUri();
            }
        ));
        public ICommand CopyShortInformationCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.CopyShortInformation();
            }
        ));
        public ICommand CopyLongInformationCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.CopyLongInformation();
            }
        ));

        public ICommand OpenApplicationDirectoryCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.OpenApplicationDirectory();
            }
        ));
        public ICommand OpenUserDirectoryCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.OpenUserDirectory();
            }
        ));
        public ICommand OpenMachineDirectoryCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.OpenMachineDirectory();
            }
        ));
        public ICommand OpenTemporaryDirectoryCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.OpenTemporaryDirectory();
            }
        ));

        #endregion

        #region function

        #endregion
    }
}

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Startup;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Startup
{
    public class ImportProgramsViewModel: ElementViewModelBase<ImportProgramsElement>, IViewLifecycleReceiver
    {
        #region variable

        private bool _nowImporting;

        #endregion
        public ImportProgramsViewModel(ImportProgramsElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
            ProgramCollection = new ModelViewModelObservableCollectionManager<ProgramElement, ProgramViewModel>(Model.ProgramItems, new ModelViewModelObservableCollectionOptions<ProgramElement, ProgramViewModel>() {
                ToViewModel = m => new ProgramViewModel(m, UserTracker, DispatcherWrapper, LoggerFactory),
            });
        }

        #region property

        public RequestSender CloseRequest { get; } = new RequestSender();

        ModelViewModelObservableCollectionManager<ProgramElement, ProgramViewModel> ProgramCollection { get; }
        public ReadOnlyObservableCollection<ProgramViewModel> ProgramItems => ProgramCollection.ViewModels;

        public bool NowImporting
        {
            get => this._nowImporting;
            private set => SetProperty(ref this._nowImporting, value);
        }

        #endregion

        #region command

        public ICommand ViewLoadedCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.LoadProgramsAsync().ConfigureAwait(false);
            }
        ));

        public ICommand CloseCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                CloseRequest.Send();
            },
            () => !NowImporting
        ).ObservesProperty(() => NowImporting));

        public ICommand ImportCommand => GetOrCreateCommand(() => new DelegateCommand(
            async () => {
                UserTracker.Track(nameof(ImportCommand), new TrackProperties() {
                    ["TotalCount"] = Model.ProgramItems.Count.ToString(CultureInfo.InvariantCulture),
                    ["ImportCount"] = Model.ProgramItems.Count(i => i.IsImport).ToString(CultureInfo.InvariantCulture),
                });
                try {
                    NowImporting = true;
                    await Model.ImportAsync();
                    CloseRequest.Send();
                } finally {
                    NowImporting = false;
                }
            },
            () => !NowImporting
        ).ObservesProperty(() => NowImporting));

        #endregion

        #region SingleModelViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    foreach(var vm in ProgramCollection.ViewModels) {
                        vm.Dispose();
                    }
                    ProgramCollection.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region IViewLifecycleReceiver

        public virtual void ReceiveViewInitialized(Window window)
        { }

        public virtual void ReceiveViewLoaded(Window window)
        { }

        public void ReceiveViewUserClosing(Window window, CancelEventArgs e)
        {
            e.Cancel = NowImporting;
        }

        public void ReceiveViewClosing(Window window, CancelEventArgs e)
        { }

        /// <inheritdoc cref="IViewCloseReceiver.ReceiveViewClosed(bool)"/>
        public void ReceiveViewClosed(Window window, bool isUserOperation)
        { }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Input;
using Prism.Commands;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Bridge.Models;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Main.Models.Data;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItem
{
    public class LauncherFileViewModel : LauncherFileSystemViewModelBase
    {
        public LauncherFileViewModel(LauncherItemElement model, Screen screen, IDispatcherWapper dispatcherWapper, ILauncherToolbarTheme launcherToolbarTheme, ILoggerFactory loggerFactory)
            : base(model, screen, dispatcherWapper, launcherToolbarTheme, loggerFactory)
        { }

        #region property

        LauncherFileDetailData? Detail { get; set; }
        bool DelayWaiting { get; set; }

        #endregion

        #region command

        public ICommand ExecuteExtendsCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.ExecuteExtends(Screen);
            },
            () => !NowLoading && CanExecutePath
        ));

        #endregion

        #region function

        void StartDelayExecute()
        {
            if(DelayWaiting) {
                Logger.LogWarning("抑制待機中: {0}", Model.LauncherItemId);
                return;
            }
            DelayWaiting = true;

            if(!NowLoading) {
                ExecuteMainAsync();
            } else {
                PropertyChanged += LauncherFileViewModel_PropertyChanged;
            }
        }


        #endregion

        #region LauncherItemViewModelBase

        protected override void DetachModelEventsImpl()
        {
            base.DetachModelEventsImpl();

            PropertyChanged -= LauncherFileViewModel_PropertyChanged;
        }

        protected override void RaiseFileSystemInfoChanged() => RaisePropertyChanged(nameof(FileInfo));

        protected override Task InitializeFileSystemAsync()
        {
            return Task.Run(() => {
                Detail = Model.LoadFileDetail();
                FileSystemInfo = Detail.FileSystemInfo;

                var workingDirectoryPath = Environment.ExpandEnvironmentVariables(Detail.PathData?.WorkDirectoryPath ?? string.Empty);
                if(!string.IsNullOrWhiteSpace(workingDirectoryPath)) {
                    CanCopyWorkingDirectory = true;
                    if(Directory.Exists(workingDirectoryPath)) {
                        ExistsWorkingDirectory = true;
                    }
                }
                CanCopyOption = !string.IsNullOrEmpty(Detail.PathData?.Option);
            });
        }


        protected override Task ExecuteMainImplAsync()
        {
            if(NowLoading) {
                Logger.LogWarning("読み込み中のため抑制: {0}", Model.LauncherItemId);
                StartDelayExecute();
                return Task.CompletedTask;
            }

            Logger.LogTrace("TODO: 起動準備 {0}, {1}", Model.LauncherItemId, Detail?.FileSystemInfo);
            return Task.Run(() => {
                Model.Execute(Screen);
            });
        }

        #endregion

        void LauncherFileViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(NowLoading)) {
                if(NowLoading) {
                    PropertyChanged -= LauncherFileViewModel_PropertyChanged;
                    DelayWaiting = false;
                    ExecuteMainAsync();
                }
            }
        }
    }
}

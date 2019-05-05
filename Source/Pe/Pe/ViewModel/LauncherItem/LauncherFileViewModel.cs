using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Theme;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherItem;
using System.IO;

namespace ContentTypeTextNet.Pe.Main.ViewModel.LauncherItem
{
    public class LauncherFileViewModel : LauncherFileSystemViewModelBase
    {
        public LauncherFileViewModel(LauncherItemElement model, IDispatcherWapper dispatcherWapper, ILauncherToolbarTheme launcherToolbarTheme, ILoggerFactory loggerFactory)
            : base(model, dispatcherWapper, launcherToolbarTheme, loggerFactory)
        { }

        #region property

        public FileInfo FileInfo => (FileInfo)FileSystemInfo;

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region LauncherItemViewModelBase

        protected override void RaiseFileSystemInfoChanged() => RaisePropertyChanged(nameof(FileInfo));

        protected override Task InitializeFileSystemAsync()
        {
            var file = Model.LoadFile();
            return Task.CompletedTask;
        }


        protected override bool CanExecuteMain
        {
            get
            {
                return true;
            }
        }

        protected override Task ExecuteMainImplAsync()
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}

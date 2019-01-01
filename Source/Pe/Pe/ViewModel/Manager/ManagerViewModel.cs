using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Logic;
using ContentTypeTextNet.Pe.Main.Model.Manager;

namespace ContentTypeTextNet.Pe.Main.ViewModel.Manager
{
    public class ManagerViewModel : ViewModelBase, IBuildStatus
    {
        public ManagerViewModel(ApplicationManager applicationManager, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            ApplicationManager = applicationManager;
        }

        #region property

        ApplicationManager ApplicationManager { get; }

        #endregion

        #region IBuildStatus

        public BuildType BuildType => BuildStatus.BuildType;

        public Version Version => BuildStatus.Version;
        public string Revision => BuildStatus.Revision;


        #endregion
    }
}

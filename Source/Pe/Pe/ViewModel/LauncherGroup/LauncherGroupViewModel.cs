using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherGroup;
using ContentTypeTextNet.Pe.Main.Model.Launcher;

namespace ContentTypeTextNet.Pe.Main.ViewModel.LauncherGroup
{
    public class LauncherGroupViewModel : SingleModelViewModelBase<LauncherGroupElement>
    {
        public LauncherGroupViewModel(LauncherGroupElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        public int RowIndex { get; set; }

        public LauncherGroupImageName ImageName => Model.ImageName;

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region SingleModelViewModelBase
        #endregion
    }
}

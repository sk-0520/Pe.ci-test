using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element.CustomizeLauncherItem;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherItem;

namespace ContentTypeTextNet.Pe.Main.ViewModel.CustomizeLauncherItem
{
    public abstract class CustomizeLauncherDetailViewModelBase : SingleModelViewModelBase<CustomizeLauncherItemElement>
    {
        public CustomizeLauncherDetailViewModelBase(CustomizeLauncherItemElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property
        #endregion

        #region command
        #endregion

        #region function

        protected abstract void InitializeImpl();

        public void Initialize()
        {
            InitializeImpl();
        }

        #endregion

        #region SingleModelViewModelBase
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.CustomizeLauncherItem;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.CustomizeLauncherItem
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

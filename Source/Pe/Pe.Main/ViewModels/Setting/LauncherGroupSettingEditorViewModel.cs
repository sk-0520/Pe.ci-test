using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class LauncherGroupSettingEditorViewModel : SingleModelViewModelBase<LauncherGroupElement>
    {
        #region variable

        string _name;

        #endregion

        public LauncherGroupSettingEditorViewModel(LauncherGroupElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            DispatcherWrapper = dispatcherWrapper;

            this._name = Model.Name;
        }

        #region property

        IDispatcherWrapper DispatcherWrapper { get; }

        public string Name
        {
            get => this._name;
            set
            {
                SetProperty(ref this._name, value);
                ValidateProperty(value);
            }
        }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

    }
}

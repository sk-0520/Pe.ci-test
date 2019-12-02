using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public interface ISettingEditorViewModel: IFlushable
    {
        #region property

        public string Header { get; }

        #endregion

        #region function

        void Load();

        #endregion
    }

    public abstract class SettingEditorViewModelBase<TSettingEditorElement> : SingleModelViewModelBase<TSettingEditorElement>, ISettingEditorViewModel
        where TSettingEditorElement : SettingEditorElementBase
    {
        public SettingEditorViewModelBase(TSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            if(!Model.IsInitialized) {
                throw new ArgumentException(nameof(Model.IsInitialized));
            }

            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        protected IDispatcherWrapper DispatcherWrapper { get; }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region ISettingEditorViewModel

        public abstract string Header { get; }

        public virtual void Load()
        {
            Model.Load();
        }

        public abstract void Flush();

        #endregion

    }
}

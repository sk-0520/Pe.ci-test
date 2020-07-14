using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.ViewModels.IconViewer;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherIcon;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public sealed class LauncherItemSettingEditorViewModel: LauncherItemCustomizeEditorViewModel
    {
        public LauncherItemSettingEditorViewModel(LauncherItemSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            Editor = model;
        }

        #region property

        public bool IsLazyLoad => Editor.IsLazyLoad;

        [IgnoreValidation]
        LauncherItemSettingEditorElement Editor { get; }

        [IgnoreValidation]
        public object Icon
        {
            get
            {
                var factory = Editor.CreateLauncherIconFactory();
                return factory.CreateView(DispatcherWrapper);
            }
        }

        #endregion

        #region function

        public LauncherItemSettingEditorViewModel Clone()
        {
            return new LauncherItemSettingEditorViewModel(Editor, DispatcherWrapper, LoggerFactory);
        }

        internal void LazyLoad()
        {
            if(!IsLazyLoad) {
                throw new InvalidOperationException(nameof(IsLazyLoad));
            }

            Editor.LazyLoad();
        }

        #endregion

        #region LauncherItemCustomizeEditorViewModel

        protected override bool SkipValidation => IsLazyLoad;

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}

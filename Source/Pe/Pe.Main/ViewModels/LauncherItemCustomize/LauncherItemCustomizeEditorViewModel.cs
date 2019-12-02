using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public class LauncherItemCustomizeEditorViewModel : SingleModelViewModelBase<LauncherItemCustomizeEditorElement>, ILauncherItemId, IFlushable
    {
        #region variable

        //List<LauncherItemCustomizeDetailViewModelBase>? _customizeItems;
        bool _isChanged;
        #endregion

        public LauncherItemCustomizeEditorViewModel(LauncherItemCustomizeEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory) : base(model, loggerFactory)
        {
            DispatcherWrapper = dispatcherWrapper;

            Common = new LauncherItemCustomizeCommonViewModel(Model, DispatcherWrapper, LoggerFactory);

            var items = new List<LauncherItemCustomizeDetailViewModelBase>();
            items.Add(Common);

            switch(Model.Kind) {
                case LauncherItemKind.File: {
                        var file = new LauncherItemCustomizeFileViewModel(Model, DispatcherWrapper, LoggerFactory);
                        var env = new LauncherItemCustomizeEnvironmentVariableViewModel(Model, DispatcherWrapper, LoggerFactory);

                        items.Add(file);
                        items.Add(env);
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }

            Tag = new LauncherItemCustomizeTagViewModel(Model, DispatcherWrapper, LoggerFactory);
            Comment = new LauncherItemCustomizeCommentViewModel(Model, DispatcherWrapper, LoggerFactory);

            items.Add(Tag);
            items.Add(Comment);

            CustomizeItems = items;

            foreach(var item in CustomizeItems) {
                item.Initialize();
                item.PropertyChanged += Item_PropertyChanged;
            }
        }

        #region property

        protected IDispatcherWrapper DispatcherWrapper { get; }

        public List<LauncherItemCustomizeDetailViewModelBase> CustomizeItems { get; }

        public LauncherItemCustomizeCommonViewModel Common { get; }
        public LauncherItemCustomizeTagViewModel Tag { get; }
        public LauncherItemCustomizeCommentViewModel Comment { get; }

        public bool IsChanged
        {
            get => this._isChanged;
            set => SetProperty(ref this._isChanged, value);
        }

        #endregion


        #region function


        #endregion

        #region SingleModelViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                foreach(var item in CustomizeItems) {
                    item.PropertyChanged -= Item_PropertyChanged;
                    if(disposing) {
                        item.Dispose();
                    }
                }
                IsChanged = false;
            }
            base.Dispose(disposing);
        }

        #endregion

        #region ILauncherItemId

        public Guid LauncherItemId => Model.LauncherItemId;

        #endregion

        #region IFlushable

        public void Flush()
        {
            foreach(var flushable in CustomizeItems.OfType<IFlushable>()) {
                flushable.Flush();
            }
        }

        #endregion

        private void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            IsChanged = true;
        }

    }
}

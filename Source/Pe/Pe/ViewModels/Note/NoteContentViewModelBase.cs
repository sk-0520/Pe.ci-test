using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Note;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Note
{
    public abstract class NoteContentViewModelBase : SingleModelViewModelBase<NoteContentElement>
    {
        #region variable

        bool _canVisible;

        #endregion

        public NoteContentViewModelBase(NoteContentElement model, IClipboardManager clipboardManager, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            ClipboardManager = clipboardManager;
            DispatcherWapper = dispatcherWapper;
        }

        #region property

        public NoteContentKind Kind => Model.ContentKind;
        protected IClipboardManager ClipboardManager { get; }
        protected IDispatcherWapper DispatcherWapper { get; }
        public bool CanVisible
        {
            get => this._canVisible;
            private set => SetProperty(ref this._canVisible, value);
        }

        private Control? Control { get; set; }

        #endregion

        #region command

        public ICommand LoadedCommand => GetOrCreateCommand(() => new DelegateCommand<Control>(
            async o => {
                if(CanVisible) {
                    return;
                }

                AttachControl(o);

                try {
                    Logger.LogDebug("読み込み開始");
                    await LoadContentAsync(o);
                    Logger.LogDebug("読み込み終了");
                    CanVisible = true;
                } catch(Exception ex) {
                    Logger.LogError(ex, "読み込み失敗");
                    throw; // 投げなくてもいいかも
                }
            }
        ));

        public ICommand CopyCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                var data = GetContentData();
                ClipboardManager.Set(data);
            }
        ));

        #endregion

        #region function

        private void AttachControl(Control o)
        {
            Control = o;
            Control.Unloaded += Control_Unloaded;
        }

        private void DetachControl()
        {
            if(Control != null) {
                Control.Unloaded -= Control_Unloaded;
            }
        }

        protected abstract Task LoadContentAsync(Control control);
        protected abstract void UnloadContent();

        protected abstract IDataObject GetContentData();

        #endregion

        private void Control_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            UnloadContent();
            DetachControl();
            CanVisible = false;
        }
    }

    public static class NoteContentViewModelFactory
    {
        #region function

        public static NoteContentViewModelBase Create(NoteContentElement model, IClipboardManager clipboardManager, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
        {
            switch(model.ContentKind) {
                case NoteContentKind.Plain:
                    return new NotePlainContentViewModel(model, clipboardManager, dispatcherWapper, loggerFactory);

                case NoteContentKind.RichText:
                    return new NoteRichTextContentViewModel(model, clipboardManager, dispatcherWapper, loggerFactory);

                //case NoteContentKind.Link:
                //    return new NoteLinkContentViewModel(model, clipboardManager, dispatcherWapper, loggerFactory);

                default:
                    throw new NotImplementedException();
            }

        }


        #endregion
    }

}

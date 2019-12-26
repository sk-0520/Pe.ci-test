using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
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

        public NoteContentViewModelBase(NoteContentElement model, IClipboardManager clipboardManager, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            ClipboardManager = clipboardManager;
            DispatcherWrapper = dispatcherWrapper;

            PropertyChangedHooker = new PropertyChangedHooker(DispatcherWrapper, LoggerFactory);
            PropertyChangedHooker.AddHook(nameof(IsLink), nameof(IsLink));
        }

        #region property

        public NoteContentKind Kind => Model.ContentKind;
        protected IClipboardManager ClipboardManager { get; }
        protected IDispatcherWrapper DispatcherWrapper { get; }
        public bool CanVisible
        {
            get => this._canVisible;
            private set => SetProperty(ref this._canVisible, value);
        }

        private Control? Control { get; set; }

        public bool IsLink => Model.IsLink;

        PropertyChangedHooker PropertyChangedHooker { get; }

        protected bool EnabledUpdate { get; private set; } = true;

        #endregion

        #region command

        public ICommand LoadedCommand => GetOrCreateCommand(() => new DelegateCommand<Control>(
            async o => {
                if(CanVisible) {
                    return;
                }

                AttachControlCore(o);

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
                var data = GetClipbordContentData();
                ClipboardManager.Set(data);
            }
        ));

        #endregion

        #region function

        private void AttachControlCore(Control o)
        {
            Control = o;
            Control.Unloaded += Control_Unloaded;
        }

        private void DetachControlCore()
        {
            if(Control != null) {
                Control.Unloaded -= Control_Unloaded;
            }
        }

        /// <summary>
        /// コンテンツが必要になった際に呼び出される。
        /// <para>UI要素への購買処理も実施すること。</para>
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        protected abstract Task LoadContentAsync(Control control);
        /// <summary>
        /// コンテンツが不要になった際に呼び出される。
        /// <para>UI要素への解除処理も実施すること。</para>
        /// </summary>
        protected abstract void UnloadContent();

        /// <summary>
        /// クリップボード用データの取得。
        /// </summary>
        /// <returns></returns>
        protected abstract IDataObject GetClipbordContentData();

        #endregion

        #region SingleModelViewModelBase

        protected override void AttachModelEventsImpl()
        {
            Model.LinkContentChanged += Model_LinkContentChanged;
        }

        protected override void DetachModelEventsImpl()
        {
            Model.LinkContentChanged -= Model_LinkContentChanged;
        }

        #endregion

        private void Control_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            UnloadContent();
            DetachControlCore();
            CanVisible = false;
        }

        private void Model_LinkContentChanged(object? sender, EventArgs e)
        {
            if(Control == null) {
                Logger.LogTrace("change ...");
                return;
            }

            Logger.LogTrace("change!");
            EnabledUpdate = false;
            UnloadContent();
            LoadContentAsync(Control).ContinueWith(t => {
                EnabledUpdate = true;
            }).ConfigureAwait(false);
        }


    }

    public static class NoteContentViewModelFactory
    {
        #region function

        public static NoteContentViewModelBase Create(NoteContentElement model, IClipboardManager clipboardManager, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            switch(model.ContentKind) {
                case NoteContentKind.Plain:
                    return new NotePlainContentViewModel(model, clipboardManager, dispatcherWrapper, loggerFactory);

                case NoteContentKind.RichText:
                    return new NoteRichTextContentViewModel(model, clipboardManager, dispatcherWrapper, loggerFactory);

                //case NoteContentKind.Link:
                //    return new NoteLinkContentViewModel(model, clipboardManager, dispatcherWapper, loggerFactory);

                default:
                    throw new NotImplementedException();
            }

        }


        #endregion
    }

}

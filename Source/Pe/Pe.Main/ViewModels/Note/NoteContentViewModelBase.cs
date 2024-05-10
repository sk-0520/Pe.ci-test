using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Note;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Note
{
    public abstract class NoteContentViewModelBase: SingleModelViewModelBase<NoteContentElement>
    {
        #region variable

        private bool _canVisible;

        #endregion

        protected NoteContentViewModelBase(NoteContentElement model, NoteConfiguration noteConfiguration, IClipboardManager clipboardManager, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            ClipboardManager = clipboardManager;
            DispatcherWrapper = dispatcherWrapper;
            NoteConfiguration = noteConfiguration;

            PropertyChangedHooker = new PropertyChangedHooker(DispatcherWrapper, LoggerFactory);
            PropertyChangedHooker.AddHook(nameof(IsLink), nameof(IsLink));
        }

        #region property

        public NoteContentKind Kind => Model.ContentKind;
        protected NoteConfiguration NoteConfiguration { get; }
        protected IClipboardManager ClipboardManager { get; }
        protected IDispatcherWrapper DispatcherWrapper { get; }
        public bool CanVisible
        {
            get => this._canVisible;
            private set => SetProperty(ref this._canVisible, value);
        }

        protected FrameworkElement? BaseElement { get; private set; }

        public bool IsLink => Model.IsLink;

        private PropertyChangedHooker PropertyChangedHooker { get; }

        protected bool EnabledUpdate { get; private set; } = true;

        #endregion

        #region command

        private ICommand? _LoadedCommand;
        public ICommand LoadedCommand => this._LoadedCommand ??= new DelegateCommand<FrameworkElement>(
            async o => {
                if(CanVisible) {
                    return;
                }

                AttachControlCore(o);

                try {
                    Logger.LogDebug("読み込み開始");
                    Model.IsLinkLoadError = !await LoadContentAsync();
                    Logger.LogDebug("読み込み終了");
                    CanVisible = true;
                } catch(Exception ex) {
                    Logger.LogError(ex, "読み込み失敗");
                    throw; // 投げなくてもいいかも
                }
            }
        );

        private ICommand? _CopyCommand;
        public ICommand CopyCommand => this._CopyCommand ??= new DelegateCommand(
            () => {
                var data = GetClipboardContentData();
                ClipboardManager.CopyData(data, ClipboardNotify.None);
            }
        );

        #endregion

        #region function

        protected void AttachControlCore(FrameworkElement o)
        {
            BaseElement = o;
            BaseElement.Unloaded += Control_Unloaded;
        }

        private void DetachControlCore()
        {
            if(BaseElement is not null) {
                BaseElement.Unloaded -= Control_Unloaded;
            }

            BaseElement = null;
        }

        /// <summary>
        /// コンテンツが必要になった際に呼び出される。
        /// </summary>
        /// <remarks>
        /// <para>UI要素への購買処理も実施すること。</para>
        /// <para>例外処理も対応が必要。</para>
        /// </remarks>
        /// <param name="baseElement"></param>
        /// <returns>正常に読み込めたか</returns>
        protected abstract Task<bool> LoadContentAsync();
        /// <summary>
        /// コンテンツが不要になった際に呼び出される。
        /// </summary>
        /// <remarks>
        /// <para>UI要素への解除処理も実施すること。</para>
        /// </remarks>
        protected abstract void UnloadContent();

        /// <summary>
        /// クリップボード用データの取得。
        /// </summary>
        /// <returns></returns>
        protected abstract IDataObject GetClipboardContentData();

        public abstract void SearchContent(string searchValue, bool searchNext);

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

        protected override void Dispose(bool disposing)
        {
            UnloadContent();
            DetachControlCore();

            base.Dispose(disposing);
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
            if(BaseElement == null) {
                Logger.LogTrace("リンク先内容変更を検知したが無効");
                return;
            }
            if(!CanVisible) {
                return;
            }

            Logger.LogDebug("リンク先内容変更検知");
            EnabledUpdate = false;
            UnloadContent();
            LoadContentAsync().ContinueWith(t => {
                if(t.IsCompletedSuccessfully) {
                    Model.IsLinkLoadError = !t.Result;
                }
                EnabledUpdate = true;
            }).ConfigureAwait(false);
        }
    }

    public abstract class NoteContentViewModelBase<TControlElement>: NoteContentViewModelBase
        where TControlElement: FrameworkElement
    {
        protected NoteContentViewModelBase(NoteContentElement model, NoteConfiguration noteConfiguration, IClipboardManager clipboardManager, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, noteConfiguration, clipboardManager, dispatcherWrapper, loggerFactory)
        { }

        #region property

        /// <summary>
        /// <see cref="NoteContentViewModelBase.BaseElement"/> を特定の非 <see langword="null" /> コントロールで取得。
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="NoteContentViewModelBase.BaseElement"/> が <see langword="null" /></exception>
        protected TControlElement ControlElement
        {
            get
            {
                if(BaseElement is null) {
                    throw new InvalidOperationException();
                }

                return (TControlElement)BaseElement;
            }
        }

        #endregion
    }

    public static class NoteContentViewModelFactory
    {
        #region function

        public static NoteContentViewModelBase Create(NoteContentElement model, NoteConfiguration noteConfiguration, IClipboardManager clipboardManager, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            switch(model.ContentKind) {
                case NoteContentKind.Plain:
                    return new NotePlainContentViewModel(model, noteConfiguration, clipboardManager, dispatcherWrapper, loggerFactory);

                case NoteContentKind.RichText:
                    return new NoteRichTextContentViewModel(model, noteConfiguration, clipboardManager, dispatcherWrapper, loggerFactory);

                //case NoteContentKind.Link:
                //    return new NoteLinkContentViewModel(model, clipboardManager, dispatcherWapper, loggerFactory);

                default:
                    throw new NotImplementedException();
            }

        }

        #endregion
    }
}

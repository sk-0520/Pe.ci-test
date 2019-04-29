using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Element.Note;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModel.Note
{
    public abstract class NoteContentViewModelBase : SingleModelViewModelBase<NoteContentElement>
    {
        #region variable

        bool _canVisible;

        #endregion

        public NoteContentViewModelBase(NoteContentElement model, IDispatcherWapper dispatcherWapper, ILogger logger)
            : base(model, logger)
        {
            DispatcherWapper = dispatcherWapper;
        }

        public NoteContentViewModelBase(NoteContentElement model, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            DispatcherWapper = dispatcherWapper;
        }

        #region property

        public NoteContentKind Kind => Model.ContentKind;
        protected IDispatcherWapper DispatcherWapper { get; }
        public bool CanVisible
        {
            get => this._canVisible;
            private set => SetProperty(ref this._canVisible, value);
        }

        private Control Control { get; set; }

        #endregion

        #region command

        public ICommand LoadedCommand => GetOrCreateCommand(() => new DelegateCommand<Control>(
            async o => {
                if(CanVisible) {
                    return;
                }

                AttachControl(o);

                try {
                    Logger.Debug("読み込み開始");
                    await LoadContentAsync(o);
                    Logger.Debug("読み込み終了");
                    CanVisible = true;
                } catch(Exception ex) {
                    Logger.Debug("読み込み失敗");
                    Logger.Error(ex);
                    throw; // 投げなくてもいいかも
                }
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

        #endregion
        private void Control_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Model.Flush();
            DetachControl();
            CanVisible = false;
        }
    }

    public static class NoteContentViewModelFactory
    {
        #region function

        public static NoteContentViewModelBase Create(NoteContentElement model, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
        {
            switch(model.ContentKind) {
                case NoteContentKind.Plain:
                    return new NotePlainContentViewModel(model, dispatcherWapper, loggerFactory);

                case NoteContentKind.RichText:
                    return new NoteRichTextContentViewModel(model, dispatcherWapper, loggerFactory);

                case NoteContentKind.Link:
                    return new NoteLinkContentViewModel(model, dispatcherWapper, loggerFactory);

                default:
                    throw new NotImplementedException();
            }

        }


        #endregion
    }

}

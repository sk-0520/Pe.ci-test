using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public NoteContentViewModelBase(NoteContentElement model, ILogger logger)
            : base(model, logger)
        { }

        public NoteContentViewModelBase(NoteContentElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        public NoteContentKind Kind => Model.ContentKind;

        public bool CanVisible
        {
            get => this._canVisible;
            private set => SetProperty(ref this._canVisible, value);
        }

        #endregion

        #region command

        public ICommand LoadedCommand => GetOrCreateCommand(() => new DelegateCommand(
            async () => {
                if(CanVisible) {
                    return;
                }
                try {
                    Logger.Debug("読み込み開始");
                    await LoadContentAsync();
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

        protected abstract Task LoadContentAsync();

        #endregion
    }

    public static class NoteContentViewModelFactory
    {
        #region function

        public static NoteContentViewModelBase Create(NoteContentElement model, ILoggerFactory loggerFactory)
        {
            switch(model.ContentKind) {
                case NoteContentKind.Plain:
                    return new NotePlainContentViewModel(model, loggerFactory);

                case NoteContentKind.RichText:
                    return new NoteRichTextContentViewModel(model, loggerFactory);

                case NoteContentKind.Link:
                    return new NoteLinkContentViewModel(model, loggerFactory);

                default:
                    throw new NotImplementedException();
            }

        }


        #endregion
    }

}

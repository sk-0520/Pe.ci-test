using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element.Note;
using ContentTypeTextNet.Pe.Main.Model.Theme;

namespace ContentTypeTextNet.Pe.Main.ViewModel.Note
{
    public class NoteNotifyAreaViewModel : SingleModelViewModelBase<NoteElement>
    {
        public NoteNotifyAreaViewModel(NoteElement model, IDispatcherWapper dispatcherWapper, INoteTheme noteTheme, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            DispatcherWapper = dispatcherWapper;
            NoteTheme = noteTheme;

            PropertyChangedHooker = new PropertyChangedHooker(DispatcherWapper, Logger.Factory);
            PropertyChangedHooker.AddHook(nameof(Model.IsVisible), nameof(IsVisible));
        }

        #region property

        IDispatcherWapper DispatcherWapper { get; }
        INoteTheme NoteTheme { get; }

        PropertyChangedHooker PropertyChangedHooker { get; }

        public bool IsVisible => Model.IsVisible;

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region SingleModelViewModelBase

        protected override void AttachModelEventsImpl()
        {
            base.AttachModelEventsImpl();

            Model.PropertyChanged += Model_PropertyChanged;
        }

        protected override void DetachModelEventsImpl()
        {
            base.DetachModelEventsImpl();

            Model.PropertyChanged -= Model_PropertyChanged;
        }

        #endregion

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyChangedHooker.Execute(e, RaisePropertyChanged);
        }


    }
}

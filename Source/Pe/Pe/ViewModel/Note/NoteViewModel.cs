using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element.Note;
using ContentTypeTextNet.Pe.Main.Model.Theme;

namespace ContentTypeTextNet.Pe.Main.ViewModel.Note
{
    public class NoteViewModel: SingleModelViewModelBase<NoteElement>, IViewLifecycleReceiver
    {
        public NoteViewModel(NoteElement model, INoteTheme noteTheme, ILoggerFactory loggerFactory)
            :base(model, loggerFactory)
        {
            NoteTheme = noteTheme;
        }

        #region property

        INoteTheme NoteTheme { get; }

        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetModelValue(value);
        }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewLoaded(Window window)
        {
            if(!IsVisible) {
                window.Visibility = Visibility.Collapsed;
            }
        }

        public void ReceiveViewClosing(CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewClosing();
        }

        public void ReceiveViewClosed()
        {
            Model.ReceiveViewClosed();
        }


        #endregion

    }
}

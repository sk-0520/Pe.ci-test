using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.View
{
    public class ViewExtendBase<TView, TExtendData> : DisposerBase
        where TView : UIElement
        where TExtendData: INotifyPropertyChanged
    {
        public ViewExtendBase(TView view, TExtendData extendData, ILoggerFactory loggerFactory)
        {
            View = view;
        }

        #region property

        protected TView View { get; private set; }
        protected TExtendData ExtendData { get; private set; }

        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                View = null;
            }

            base.Dispose(disposing);
        }

        #endregion

    }
}

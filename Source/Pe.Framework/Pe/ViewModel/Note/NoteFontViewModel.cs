using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element.Font;
using ContentTypeTextNet.Pe.Main.ViewModel.Font;

namespace ContentTypeTextNet.Pe.Main.ViewModel.Note
{
    public class NoteFontViewModel : FontViewModel
    {
        public NoteFontViewModel(FontElement model, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWapper, loggerFactory)
        { }

        #region property
        #endregion
    }
}

using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.Font;
using ContentTypeTextNet.Pe.Main.ViewModels.Font;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Note
{
    public class NoteFontViewModel: FontViewModel
    {
        public NoteFontViewModel(SavingFontElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        { }

        #region property


        #endregion
    }
}

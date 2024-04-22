using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.Widget;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Widget
{
    /// <summary>
    /// だだっと表示させるための暫定的VM。
    /// </summary>
    /// <remarks>
    /// <para>バグのにほい</para>
    /// </remarks>
    internal class TemporaryWidgetViewModel: WidgetViewModelBase<WidgetElement>
    {
        public TemporaryWidgetViewModel(WidgetElement model, IUserTracker userTracker, IWindowManager windowManager, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, windowManager, dispatcherWrapper, loggerFactory)
        { }
    }
}

using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.ExtendsExecute
{
    public class OptionDragAndDropGuideline: DragAndDropGuidelineBase
    {
        public OptionDragAndDropGuideline(IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(dispatcherWrapper, loggerFactory)
        { }

        #region function

        public bool CanDragStart(UIElement sender, MouseEventArgs e) => false;

        #endregion
    }
}

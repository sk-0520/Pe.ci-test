using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Views.CustomizeLauncherItem
{
    /// <summary>
    /// CustomizeLauncherItemWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class CustomizeLauncherItemWindow : Window
    {
        public CustomizeLauncherItemWindow()
        {
            InitializeComponent();
        }

        #region property

        [Injection]
        ILogger? Logger { get; set; }

        #endregion

        #region Window

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            var avalonEdit = UIUtility.GetVisualClosest<ICSharpCode.AvalonEdit.TextEditor>((DependencyObject)e.OriginalSource);
            if(avalonEdit != null) {
                var scrollViewer = UIUtility.GetVisualClosest<System.Windows.Controls.ScrollViewer>((DependencyObject)e.OriginalSource);
                if(scrollViewer != null) {
                    Logger.LogDebug(e.OriginalSource.ToString());
                    Logger.LogDebug("HorizontalOffset: {0}, ContentHorizontalOffset: {1}", scrollViewer.HorizontalOffset, scrollViewer.ContentHorizontalOffset);
                    Logger.LogDebug("VerticalOffset: {0}, ContentVerticalOffset: {1}", scrollViewer.VerticalOffset, scrollViewer.ContentVerticalOffset);
                    Logger.LogDebug("ScrollableHeight: {0}", scrollViewer.ScrollableHeight);
                    if(scrollViewer.VerticalOffset == 0) {
                        Logger.LogDebug("top");
                        e.Handled = true;
                    }
                    if(scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight) {
                        Logger.LogDebug("bottom");
                        e.Handled = true;
                    }
                }
            }

            base.OnPreviewMouseWheel(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            var avalonEdit = UIUtility.GetVisualClosest<ICSharpCode.AvalonEdit.TextEditor>((DependencyObject)e.OriginalSource);
            if(avalonEdit != null) {
                var scrollViewer = UIUtility.GetVisualClosest<System.Windows.Controls.ScrollViewer>((DependencyObject)e.OriginalSource);
                if(scrollViewer != null) {
                    Logger.LogDebug(e.OriginalSource.ToString());
                    Logger.LogDebug("HorizontalOffset: {0}, ContentHorizontalOffset: {1}", scrollViewer.HorizontalOffset, scrollViewer.ContentHorizontalOffset);
                    Logger.LogDebug("VerticalOffset: {0}, ContentVerticalOffset: {1}", scrollViewer.VerticalOffset, scrollViewer.ContentVerticalOffset);
                    Logger.LogDebug("ScrollableHeight: {0}", scrollViewer.ScrollableHeight);
                    if(scrollViewer.VerticalOffset == 0) {
                        Logger.LogDebug("top");
                        e.Handled = true;
                    }
                    if(scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight) {
                        Logger.LogDebug("bottom");
                        e.Handled = true;
                    }
                }
            }

            base.OnMouseWheel(e);
        }

        #endregion
    }
}

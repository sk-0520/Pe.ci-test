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
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.ViewModel.LauncherItem;
using ContentTypeTextNet.Pe.Main.ViewModel.LauncherToolbar;

namespace ContentTypeTextNet.Pe.Main.View.LauncherToolbar
{
    /// <summary>
    /// LauncherToolbarWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class LauncherToolbarWindow : Window
    {
        public LauncherToolbarWindow()
        {
            InitializeComponent();
        }

        #region property

        [Injection]
        ILogger Logger { get; set; }

        #endregion

        #region function

        LauncherToolbarViewModel ViewModel => (LauncherToolbarViewModel)DataContext;

        #endregion

        private void ContentPresenter_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {

            if(e.Source is LauncherContentControl control) {
                if(((Control)control.Parent).DataContext is LauncherItemViewModelBase launcherItem) {
                    //ViewModel.ContextMenuOpendItem = launcherItem;
                    //((FrameworkElement)sender).ContextMenu.DataContext = launcherItem;
                }
            }
        }

        private void ContentPresenter_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            ViewModel.ContextMenuOpendItem = null;
            //((FrameworkElement)sender).ContextMenu.DataContext = null;
        }

    }
}

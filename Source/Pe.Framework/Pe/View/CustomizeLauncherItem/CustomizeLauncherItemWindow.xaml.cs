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

namespace ContentTypeTextNet.Pe.Main.View.CustomizeLauncherItem
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
        ILogger Logger { get; set; }

        #endregion

    }
}

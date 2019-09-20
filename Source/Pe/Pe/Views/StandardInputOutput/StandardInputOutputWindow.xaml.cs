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
using ContentTypeTextNet.Pe.Main.ViewModels.StandardInputOutput;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Views.StandardInputOutput
{
    /// <summary>
    /// StandardInputOutputWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class StandardInputOutputWindow : Window
    {
        public StandardInputOutputWindow()
        {
            InitializeComponent();
        }

        #region property

        [Injection]
        ILogger? Logger { get; set; }
        StandardInputOutputViewModel ViewModel => (StandardInputOutputViewModel)DataContext;

        #endregion
    }
}

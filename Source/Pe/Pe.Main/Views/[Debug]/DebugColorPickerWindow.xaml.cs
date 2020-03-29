using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Views._Debug_
{
    /// <summary>
    /// DebugColorPickerWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class DebugColorPickerWindow : Window
    {
        public DebugColorPickerWindow()
        {
            InitializeComponent();
        }

        [Inject]
        ILogger? Logger { get; set; }
    }
}

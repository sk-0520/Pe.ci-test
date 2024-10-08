using System.Windows;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Views._Debug_
{
    /// <summary>
    /// DebugColorPickerWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class DebugColorPickerWindow: Window
    {
        public DebugColorPickerWindow()
        {
            InitializeComponent();
        }

        [DiInjection]
        ILogger? Logger { get; set; }
    }
}

using System.Windows;
using ContentTypeTextNet.Pe.Standard.DependencyInjection;
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

        [Inject]
        ILogger? Logger { get; set; }
    }
}

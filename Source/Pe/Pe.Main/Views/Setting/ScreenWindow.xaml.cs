using System.Windows;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Main.ViewModels.Setting;
using ContentTypeTextNet.Pe.PInvoke.Windows;

namespace ContentTypeTextNet.Pe.Main.Views.Setting
{
    /// <summary>
    /// ScreenWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ScreenWindow: Window
    {
        public ScreenWindow()
        {
            InitializeComponent();

            Loaded += ScreenWindow_Loaded;
        }

        private void ScreenWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= ScreenWindow_Loaded;
            var vm = (ScreenViewModel)DataContext!;
            var deviceArea = PodStructUtility.Convert(vm.Screen.DeviceBounds);
            NativeMethods.MoveWindow(HandleUtility.GetWindowHandle(this), deviceArea.X, deviceArea.Y, deviceArea.Width, deviceArea.Height, true);
        }
    }
}

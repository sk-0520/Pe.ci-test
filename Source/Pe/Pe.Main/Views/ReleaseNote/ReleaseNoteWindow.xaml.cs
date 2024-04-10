using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.WebView;
using ContentTypeTextNet.Pe.Standard.DependencyInjection;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.Views.ReleaseNote
{
    /// <summary>
    /// ReleaseNoteWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ReleaseNoteWindow: Window
    {
        public ReleaseNoteWindow()
        {
            InitializeComponent();
        }

        #region property

        private CommandStore CommandStore { get; } = new CommandStore();

        [Inject]
        private WebViewInitializer WebViewInitializer { get; set; } = default!;
        [Inject]
        private EnvironmentParameters EnvironmentParameters { get; set; } = default!;
        [Inject]
        private CultureService CultureService { get; set; } = default!;

        #endregion

        #region command
        public ICommand CloseCommand => CommandStore.GetOrCreate(() => new DelegateCommand(
            () => Close()
        ));

        #endregion

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:命名スタイル", Justification = "<保留中>")]
        private async void root_SourceInitialized(object sender, System.EventArgs e)
        {
            await WebViewInitializer.InitializeAsync(this.webView, EnvironmentParameters, CultureService);
        }
    }
}

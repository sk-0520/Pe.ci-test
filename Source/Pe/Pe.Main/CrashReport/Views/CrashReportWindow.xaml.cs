using System;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.CrashReport.Views
{
    /// <summary>
    /// CrashReportWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class CrashReportWindow: Window
    {
        public CrashReportWindow()
        {
            InitializeComponent();
            Language = CultureService.Instance.GetXmlLanguage();
        }

        #region property

        #endregion

        #region command

        private ICommand? _CloseCommand;
        public ICommand CloseCommand => this._CloseCommand ??= new DelegateCommand(
            () => Close()
        );

        #endregion

        #region Window

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var hWnd = HandleUtility.GetWindowHandle(this);
            var style = (int)WindowsUtility.GetWindowLongPtr(hWnd, (int)GWL.GWL_STYLE);
            style &= ~(int)(WS.WS_MAXIMIZEBOX | WS.WS_MINIMIZEBOX);
            WindowsUtility.SetWindowLongPtr(hWnd, (int)GWL.GWL_STYLE, (IntPtr)style);
        }

        #endregion
    }
}

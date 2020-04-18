using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Views.NotifyLog
{
    /// <summary>
    /// NotifyLogWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class NotifyLogWindow : Window, IDpiScaleOutputor
    {
        public NotifyLogWindow()
        {
            InitializeComponent();
        }

        #region property

        CommandStore CommandStore { get; } = new CommandStore();
        [Inject]
        ILogger? Logger { get; set; }


        #endregion

        #region function
        #endregion

        #region Window

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            UIUtility.SetToolWindowStyle(this, false, false);
            //UIUtility.ChangeTransparent(this, true);
        }

        #endregion

        #region IDpiScaleOutputor

        public Point GetDpiScale() => UIUtility.GetDpiScale(this);
        public IScreen GetOwnerScreen() => Screen.FromHandle(HandleUtility.GetWindowHandle(this));

        #endregion


    }
}

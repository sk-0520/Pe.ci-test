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
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.Views.Command
{
    /// <summary>
    /// CommandWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class CommandWindow : Window, IDpiScaleOutputor
    {
        public CommandWindow()
        {
            InitializeComponent();
        }

        #region IDpiScaleOutputor

        public Point GetDpiScale() => UIUtility.GetDpiScale(this);
        public Screen GetOwnerScreen() => Screen.FromHandle(HandleUtility.GetWindowHandle(this));

        #endregion

    }
}

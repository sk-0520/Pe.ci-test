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

            PopupAttacher = new PopupAttacher(this, this.popupItems);
        }

        #region property

        PopupAttacher PopupAttacher { get; }

        #endregion

        #region IDpiScaleOutputor

        public Point GetDpiScale() => UIUtility.GetDpiScale(this);
        public Screen GetOwnerScreen() => Screen.FromHandle(HandleUtility.GetWindowHandle(this));

        #endregion

        #region Window

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if(e.Source == this.grip) {
                DragMove();
            }
        }

        /*
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            //this.popupItems.IsOpen = true;
        }

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);

            //this.popupItems.IsOpen = false;
        }
        */

        #endregion

    }
}

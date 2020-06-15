using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ContentTypeTextNet.Pe.Plugins.Clock.Views
{
    /// <summary>
    /// ClockWidgetWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ClockWidgetWindow: Window
    {
        #region define

        const int WM_SYSCOMMAND = 0x0112;
        const int SC_SIZE = 0xF000;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);
        [DllImport("User32.dll")]
        static extern bool SetCapture(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern bool ReleaseCapture();

        #endregion
        public ClockWidgetWindow()
        {
            InitializeComponent();
        }

        #region proeprty

        //Storyboard? PrevStoryboard { get; set; }

        //bool Dragging { get; set; }

        #endregion

        //private void Window_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    ////PrevStoryboard?.Stop();
        //    //PrevStoryboard = (Storyboard)FindResource("fadein");
        //    //this.background.BeginStoryboard(PrevStoryboard);
        //}

        //private void Window_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    //Debug.WriteLine(e.GetPosition(this));
        //    //var element = VisualTreeHelper.HitTest(this.resize, e.GetPosition(this));
        //    //if(element != null) {

        //    //}
        //    ////PrevStoryboard?.Stop();
        //    //PrevStoryboard = (Storyboard)FindResource("fadeout");
        //    //this.background.BeginStoryboard(PrevStoryboard);
        //}

        private void resize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(!IsEnabled) {
                return;
            }

            var helper = new System.Windows.Interop.WindowInteropHelper(this);
            var hWnd = helper.Handle;
            SetCapture(hWnd);
            ReleaseCapture();
            e.Handled = true;

            SendMessage(hWnd, WM_SYSCOMMAND, SC_SIZE + 8, IntPtr.Zero);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(!IsEnabled) {
                return;
            }

            DragMove();
        }

        //private void resize_MouseMove(object sender, MouseEventArgs e)
        //{

        //}
    }
}

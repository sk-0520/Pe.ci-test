using System;
using System.Collections.Generic;
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
        public ClockWidgetWindow()
        {
            InitializeComponent();
        }

        #region proeprty

        Storyboard? PrevStoryboard { get; set; }

        #endregion

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            PrevStoryboard?.Stop();
            PrevStoryboard = (Storyboard)FindResource("fadein");
            this.resize.BeginStoryboard(PrevStoryboard);
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            PrevStoryboard?.Stop();
            PrevStoryboard = (Storyboard)FindResource("fadeout");
            this.resize.BeginStoryboard(PrevStoryboard);
        }
    }
}

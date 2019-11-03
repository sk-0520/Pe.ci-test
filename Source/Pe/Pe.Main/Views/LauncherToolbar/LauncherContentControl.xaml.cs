using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Main.ViewModels.IconViewer;

namespace ContentTypeTextNet.Pe.Main.Views.LauncherToolbar
{
    /// <summary>
    /// LauncherContentControl.xaml の相互作用ロジック
    /// </summary>
    public partial class LauncherContentControl : UserControl
    {
        public LauncherContentControl()
        {
            InitializeComponent();
        }

        #region Icon

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            nameof(Icon),
            typeof(object),
            typeof(LauncherContentControl),
            new FrameworkPropertyMetadata(
                default(object),
                new PropertyChangedCallback(OnIconChanged)
            )
        );

        public object Icon
        {
            get { return (object)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is LauncherContentControl control) {
            }
        }

        #endregion

        #region IconMargin

        public static readonly DependencyProperty IconMarginProperty = DependencyProperty.Register(
            nameof(IconMargin),
            typeof(Thickness),
            typeof(LauncherContentControl),
            new FrameworkPropertyMetadata(
                default(Thickness),
                new PropertyChangedCallback(OnIconMarginChanged)
            )
        );

        public Thickness IconMargin
        {
            get { return (Thickness)GetValue(IconMarginProperty); }
            set { SetValue(IconMarginProperty, value); }
        }

        private static void OnIconMarginChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is LauncherContentControl control) {
            }
        }

        #endregion

        #region IsIconOnly

        public static readonly DependencyProperty IsIconOnlyProperty = DependencyProperty.Register(
            nameof(IsIconOnly),
            typeof(bool),
            typeof(LauncherContentControl),
            new FrameworkPropertyMetadata(
                default(bool),
                new PropertyChangedCallback(OnIsIconOnlyChanged)
            )
        );

        public bool IsIconOnly
        {
            get { return (bool)GetValue(IsIconOnlyProperty); }
            set { SetValue(IsIconOnlyProperty, value); }
        }

        private static void OnIsIconOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is LauncherContentControl control) {
            }
        }

        #endregion

        #region TextWidth

        public static readonly DependencyProperty TextWidthProperty = DependencyProperty.Register(
            nameof(TextWidth),
            typeof(double),
            typeof(LauncherContentControl),
            new FrameworkPropertyMetadata(
                default(double),
                new PropertyChangedCallback(OnTextWidthChanged)
            )
        );

        public double TextWidth
        {
            get { return (double)GetValue(TextWidthProperty); }
            set { SetValue(TextWidthProperty, value); }
        }

        private static void OnTextWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is LauncherContentControl control) {
            }
        }

        #endregion

        #region Title

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(LauncherContentControl),
            new FrameworkPropertyMetadata(
                default(string),
                new PropertyChangedCallback(OnTitleChanged)
            )
        );

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is LauncherContentControl control) {
            }
        }

        #endregion

    }
}

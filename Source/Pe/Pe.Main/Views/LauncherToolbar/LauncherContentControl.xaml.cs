using System.Windows;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Views.LauncherToolbar
{
    /// <summary>
    /// LauncherContentControl.xaml の相互作用ロジック
    /// </summary>
    public partial class LauncherContentControl: UserControl
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
            get { return GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is LauncherContentControl control) {
            }
        }

        #endregion

        #region IconBox

        public static readonly DependencyProperty IconBoxProperty = DependencyProperty.Register(
            nameof(IconBox),
            typeof(IconBox),
            typeof(LauncherContentControl),
            new FrameworkPropertyMetadata(
                IconBox.Small,
                new PropertyChangedCallback(OnIconBoxChanged)
            )
        );

        public IconBox IconBox
        {
            get { return (IconBox)GetValue(IconBoxProperty); }
            set { SetValue(IconBoxProperty, value); }
        }

        private static void OnIconBoxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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

using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ContentTypeTextNet.Pe.Main.Views
{
    /// <summary>
    /// FontSelectControl.xaml の相互作用ロジック
    /// </summary>
    public partial class FontSelectControl: UserControl
    {
        public FontSelectControl()
        {
            InitializeComponent();
        }

        #region SelectedFontFamilyProperty

        public static readonly DependencyProperty SelectedFontFamilyProperty = DependencyProperty.Register(
            nameof(SelectedFontFamily),
            typeof(FontFamily),
            typeof(FontSelectControl),
            new FrameworkPropertyMetadata(
                SystemFonts.MessageFontFamily,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnSelectedFontFamilyChanged)
            )
        );

        public FontFamily SelectedFontFamily
        {
            get { return (FontFamily)GetValue(SelectedFontFamilyProperty); }
            set { SetValue(SelectedFontFamilyProperty, value); }
        }

        static void OnSelectedFontFamilyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FontSelectControl;
            if(ctrl != null) {
                ctrl.SelectedFontFamily = (FontFamily)e.NewValue;
            }
        }

        #endregion

        #region IsBoldProperty

        public static readonly DependencyProperty IsBoldProperty = DependencyProperty.Register(
            nameof(IsBold),
            typeof(bool),
            typeof(FontSelectControl),
            new FrameworkPropertyMetadata(
                default(bool),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnIsBoldChanged)
            )
        );

        public bool IsBold
        {
            get { return (bool)GetValue(IsBoldProperty); }
            set { SetValue(IsBoldProperty, value); }
        }

        static void OnIsBoldChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FontSelectControl;
            if(ctrl != null) {
                ctrl.IsBold = (bool)e.NewValue;
            }
        }

        #endregion

        #region IsItalicProperty

        public static readonly DependencyProperty IsItalicProperty = DependencyProperty.Register(
            nameof(IsItalic),
            typeof(bool),
            typeof(FontSelectControl),
            new FrameworkPropertyMetadata(
                default(bool),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnIsItalicChanged)
            )
        );

        public bool IsItalic
        {
            get { return (bool)GetValue(IsItalicProperty); }
            set { SetValue(IsItalicProperty, value); }
        }

        static void OnIsItalicChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FontSelectControl;
            if(ctrl != null) {
                ctrl.IsItalic = (bool)e.NewValue;
            }
        }

        #endregion

        #region SizeProperty

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            nameof(Size),
            typeof(double),
            typeof(FontSelectControl),
            new FrameworkPropertyMetadata(
                SystemFonts.MessageFontSize,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnSizeChanged)
            )
        );

        public double Size
        {
            get { return (double)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        static void OnSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FontSelectControl;
            if(ctrl != null) {
                ctrl.Size = (double)e.NewValue;
            }
        }

        #endregion

        #region SizeMinimumProperty

        public static readonly DependencyProperty SizeMinimumProperty = DependencyProperty.Register(
            nameof(SizeMinimum),
            typeof(double),
            typeof(FontSelectControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnSizeMinimumChanged)
            )
        );

        public double SizeMinimum
        {
            get { return (double)GetValue(SizeMinimumProperty); }
            set { SetValue(SizeMinimumProperty, value); }
        }

        static void OnSizeMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FontSelectControl;
            if(ctrl != null) {
                ctrl.SizeMinimum = (double)e.NewValue;
            }
        }

        #endregion

        #region SizeMaximumProperty

        public static readonly DependencyProperty SizeMaximumProperty = DependencyProperty.Register(
            nameof(SizeMaximum),
            typeof(double),
            typeof(FontSelectControl),
            new FrameworkPropertyMetadata(
                100.0,
                new PropertyChangedCallback(OnSizeMaximumChanged)
            )
        );

        public double SizeMaximum
        {
            get { return (double)GetValue(SizeMaximumProperty); }
            set { SetValue(SizeMaximumProperty, value); }
        }

        static void OnSizeMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FontSelectControl;
            if(ctrl != null) {
                ctrl.SizeMaximum = (double)e.NewValue;
            }
        }

        #endregion

        #region IsEnabledBoldProperty

        public static readonly DependencyProperty IsEnabledBoldProperty = DependencyProperty.Register(
            nameof(IsEnabledBold),
            typeof(bool),
            typeof(FontSelectControl),
            new FrameworkPropertyMetadata(
                true,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnIsEnabledBoldChanged)
            )
        );

        public bool IsEnabledBold
        {
            get { return (bool)GetValue(IsEnabledBoldProperty); }
            set { SetValue(IsEnabledBoldProperty, value); }
        }

        static void OnIsEnabledBoldChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FontSelectControl;
            if(ctrl != null) {
                ctrl.IsEnabledBold = (bool)e.NewValue;
            }
        }

        #endregion

        #region IsEnabledItalicProperty

        public static readonly DependencyProperty IsEnabledItalicProperty = DependencyProperty.Register(
            nameof(IsEnabledItalic),
            typeof(bool),
            typeof(FontSelectControl),
            new FrameworkPropertyMetadata(
                true,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnIsEnabledItalicChanged)
            )
        );

        public bool IsEnabledItalic
        {
            get { return (bool)GetValue(IsEnabledItalicProperty); }
            set { SetValue(IsEnabledItalicProperty, value); }
        }

        static void OnIsEnabledItalicChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FontSelectControl;
            if(ctrl != null) {
                ctrl.IsEnabledItalic = (bool)e.NewValue;
            }
        }

        #endregion

        #region CustomContentProperty

        public static readonly DependencyProperty CustomContentProperty = DependencyProperty.Register(
            nameof(CustomContent),
            typeof(object),
            typeof(FontSelectControl),
            new FrameworkPropertyMetadata(
                null,
                new PropertyChangedCallback(OnCustomContentPropertyChanged)
            )
        );

        public object CustomContent
        {
            get { return GetValue(CustomContentProperty); }
            set { SetValue(CustomContentProperty, value); }
        }

        static void OnCustomContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FontSelectControl;
            if(ctrl != null) {
                ctrl.CustomContent = e.NewValue;
            }
        }

        #endregion

        #region ItemsSourceProperty

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            nameof(ItemsSource),
            typeof(IEnumerable),
            typeof(FontSelectControl),
            new PropertyMetadata(
                Fonts.SystemFontFamilies,
                new PropertyChangedCallback(OnItemsSourcePropertyChanged)
            )
        );

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void OnItemsSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as FontSelectControl;
            if(control != null) {
                control.OnItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
            }
        }



        private void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            // Remove handler for oldValue.CollectionChanged
            var oldValueINotifyCollectionChanged = oldValue as INotifyCollectionChanged;

            if(null != oldValueINotifyCollectionChanged) {
                oldValueINotifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(newValueINotifyCollectionChanged_CollectionChanged);
            }
            // Add handler for newValue.CollectionChanged (if possible)
            var newValueINotifyCollectionChanged = newValue as INotifyCollectionChanged;
            if(null != newValueINotifyCollectionChanged) {
                newValueINotifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(newValueINotifyCollectionChanged_CollectionChanged);
            }

        }

        void newValueINotifyCollectionChanged_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            //Do your stuff here.
        }

        #endregion

        #region IsStrongMonospaceProperty

        public static readonly DependencyProperty IsStrongMonospaceProperty = DependencyProperty.Register(
            nameof(IsStrongMonospace),
            typeof(bool),
            typeof(FontSelectControl),
            new FrameworkPropertyMetadata(
                false,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnIsStrongMonospaceChanged)
            )
        );

        public bool IsStrongMonospace
        {
            get { return (bool)GetValue(IsStrongMonospaceProperty); }
            set { SetValue(IsStrongMonospaceProperty, value); }
        }

        static void OnIsStrongMonospaceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FontSelectControl;
            if(ctrl != null) {
                ctrl.IsStrongMonospace = (bool)e.NewValue;
            }
        }

        #endregion
    }
}

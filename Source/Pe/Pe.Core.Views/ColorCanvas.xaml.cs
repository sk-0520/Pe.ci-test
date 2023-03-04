using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ContentTypeTextNet.Pe.Core.Views
{
    /// <summary>
    /// ColorCanvas.xaml の相互作用ロジック
    /// </summary>
    public partial class ColorCanvas: UserControl
    {
        public ColorCanvas()
        {
            InitializeComponent();

            // office color
            // https://www.linkedin.com/pulse/microsoft-office-standard-colors-magic-rgb-codes-david-gray
            // https://drive.google.com/file/d/0B9XNsD_NO5e6Q09oNFRsQllQaW8/view
            DefaultColorItemsSource = new List<Color>() {
                Color.FromRgb(255,255,255), Color.FromRgb(0,0,0), Color.FromRgb(238,236,225), Color.FromRgb(31,73,125), Color.FromRgb(79,129,189), Color.FromRgb(192,80,77), Color.FromRgb(155,187,89), Color.FromRgb(128,100,162), Color.FromRgb(75,172,198), Color.FromRgb(247,150,70),
                Color.FromRgb(242,242,242), Color.FromRgb(128,128,128), Color.FromRgb(221,217,196), Color.FromRgb(197,217,241), Color.FromRgb(220,230,241), Color.FromRgb(242,220,219), Color.FromRgb(235,241,222), Color.FromRgb(228,223,236), Color.FromRgb(218,238,243), Color.FromRgb(253,233,217),
                Color.FromRgb(217,217,217), Color.FromRgb(89,89,89), Color.FromRgb(196,189,151), Color.FromRgb(141,180,226), Color.FromRgb(184,204,228), Color.FromRgb(230,184,183), Color.FromRgb(216,228,188), Color.FromRgb(204,192,218), Color.FromRgb(183,222,232), Color.FromRgb(252,213,180),
                Color.FromRgb(191,191,191), Color.FromRgb(64,64,64), Color.FromRgb(148,138,84), Color.FromRgb(83,141,213), Color.FromRgb(149,179,215), Color.FromRgb(218,150,148), Color.FromRgb(196,215,155), Color.FromRgb(177,160,199), Color.FromRgb(146,205,220), Color.FromRgb(250,191,143),
                Color.FromRgb(166,166,166), Color.FromRgb(38,38,38), Color.FromRgb(73,69,41), Color.FromRgb(22,54,92), Color.FromRgb(54,96,146), Color.FromRgb(150,54,52), Color.FromRgb(118,147,60), Color.FromRgb(96,73,122), Color.FromRgb(49,134,155), Color.FromRgb(226,107,10),
                Color.FromRgb(128,128,128), Color.FromRgb(13,13,13), Color.FromRgb(29,27,16), Color.FromRgb(15,36,62), Color.FromRgb(36,64,98), Color.FromRgb(99,37,35), Color.FromRgb(79,98,40), Color.FromRgb(64,49,81), Color.FromRgb(33,89,103), Color.FromRgb(151,71,6),
                Color.FromRgb(192,0,0), Color.FromRgb(255,0,0), Color.FromRgb(255,192,0), Color.FromRgb(255,255,0), Color.FromRgb(146,208,80), Color.FromRgb(0,176,80), Color.FromRgb(0,176,240), Color.FromRgb(0,112,192), Color.FromRgb(0,32,96), Color.FromRgb(112,48,160),
            };

            DefaultColorsColumns = 10;
        }

        #region property

        private bool PausingElementChange { get; set; }

        #endregion

        //#region BaseColorProperty

        //public static readonly DependencyProperty BaseColorProperty = DependencyProperty.Register(
        //    nameof(BaseColor),
        //    typeof(Color),
        //    typeof(ColorCanvas),
        //    new PropertyMetadata(Colors.Red, OnBaseColorPropertyChanged)
        //);

        //public Color BaseColor
        //{
        //    get { return (Color)GetValue(BaseColorProperty); }
        //    set { SetValue(BaseColorProperty, value); }
        //}

        //private static void OnBaseColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var ctrl = (ColorCanvas)d;
        //    ctrl.BaseColor = (Color)e.NewValue;
        //}

        //#endregion

        #region HasAlphaProperty

        public static readonly DependencyProperty HasAlphaProperty = DependencyProperty.Register(
            nameof(HasAlpha),
            typeof(bool),
            typeof(ColorCanvas),
            new PropertyMetadata(false, OnHasAlphaPropertyChanged)
        );

        public bool HasAlpha
        {
            get { return (bool)GetValue(HasAlphaProperty); }
            set { SetValue(HasAlphaProperty, value); }
        }

        private static void OnHasAlphaPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ColorCanvas)d;
            ctrl.HasAlpha = (bool)e.NewValue;
        }

        #endregion

        #region SelectedColorProperty

        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(
            nameof(SelectedColor),
            typeof(Color),
            typeof(ColorCanvas),
            new FrameworkPropertyMetadata(
                Colors.Red,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSelectedColorPropertyChanged
            )
        );

        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        private static void OnSelectedColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ColorCanvas)d;
            var color = (Color)e.NewValue;
            if(ctrl.HasAlpha) {
                ctrl.SelectedColor = color;
            } else {
                ctrl.SelectedColor = Color.FromRgb(color.R, color.G, color.B);
            }

            if(!ctrl.PausingElementChange) {
                ctrl.PausingElementChange = true;

                ctrl.Red = color.R;
                ctrl.Green = color.G;
                ctrl.Blue = color.B;
                ctrl.Alpha = ctrl.HasAlpha ? color.A : (byte)255;

                ctrl.PausingElementChange = false;
            }
        }

        #endregion

        //#region HueGradationsProperty

        //public static readonly DependencyProperty HueGradationsProperty = DependencyProperty.Register(
        //    nameof(HueGradations),
        //    typeof(GradientStopCollection),
        //    typeof(ColorCanvas),
        //    new PropertyMetadata(
        //        new GradientStopCollection(
        //            new[] {
        //                new GradientStop(Color.FromRgb(0xff,0x00,0x00), 0),
        //                new GradientStop(Color.FromRgb(0xff,0xff,0x00), 0.167),
        //                new GradientStop(Color.FromRgb(0x00,0xff,0x00), 0.333),
        //                new GradientStop(Color.FromRgb(0x00,0xff,0xff), 0.5),
        //                new GradientStop(Color.FromRgb(0x00,0x00,0xff), 0.667),
        //                new GradientStop(Color.FromRgb(0xff,0x00,0xff), 0.833),
        //                new GradientStop(Color.FromRgb(0xff,0x00,0x00), 1),
        //            }
        //        ),
        //        OnHueGradationsPropertyChanged
        //    )
        //);

        //public GradientStopCollection HueGradations
        //{
        //    get { return (GradientStopCollection)GetValue(HueGradationsProperty); }
        //    set { SetValue(HueGradationsProperty, value); }
        //}

        //private static void OnHueGradationsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var ctrl = (ColorCanvas)d;
        //    ctrl.HueGradations = (GradientStopCollection)e.NewValue;
        //}

        //#endregion

        #region RedProperty

        public static readonly DependencyProperty RedProperty = DependencyProperty.Register(
            nameof(Red),
            typeof(byte),
            typeof(ColorCanvas),
            new FrameworkPropertyMetadata(
                (byte)255,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnRedPropertyChanged
            )
        );

        public byte Red
        {
            get { return (byte)GetValue(RedProperty); }
            set { SetValue(RedProperty, value); }
        }

        private static void OnRedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ColorCanvas)d;
            ctrl.Red = (byte)e.NewValue;

            ctrl.ChangeColor();
        }

        #endregion

        #region BlueProperty

        public static readonly DependencyProperty BlueProperty = DependencyProperty.Register(
            nameof(Blue),
            typeof(byte),
            typeof(ColorCanvas),
            new FrameworkPropertyMetadata(
                (byte)0,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnBluePropertyChanged
            )
        );

        public byte Blue
        {
            get { return (byte)GetValue(BlueProperty); }
            set { SetValue(BlueProperty, value); }
        }

        private static void OnBluePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ColorCanvas)d;
            ctrl.Blue = (byte)e.NewValue;

            ctrl.ChangeColor();
        }

        #endregion

        #region GreenProperty

        public static readonly DependencyProperty GreenProperty = DependencyProperty.Register(
            nameof(Green),
            typeof(byte),
            typeof(ColorCanvas),
            new FrameworkPropertyMetadata(
                (byte)0,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnGreenPropertyChanged
            )
        );

        public byte Green
        {
            get { return (byte)GetValue(GreenProperty); }
            set { SetValue(GreenProperty, value); }
        }

        private static void OnGreenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ColorCanvas)d;
            ctrl.Green = (byte)e.NewValue;

            ctrl.ChangeColor();
        }

        #endregion

        #region AlphaProperty

        public static readonly DependencyProperty AlphaProperty = DependencyProperty.Register(
            nameof(Alpha),
            typeof(byte),
            typeof(ColorCanvas),
            new FrameworkPropertyMetadata(
                (byte)255,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnAlphaPropertyChanged
            )
        );

        public byte Alpha
        {
            get { return (byte)GetValue(AlphaProperty); }
            set { SetValue(AlphaProperty, value); }
        }

        private static void OnAlphaPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ColorCanvas)d;
            ctrl.Alpha = (byte)e.NewValue;

            ctrl.ChangeColor();
        }

        #endregion

        #region DefaultColorItemsSourceProperty

        public static readonly DependencyProperty DefaultColorItemsSourceProperty = DependencyProperty.Register(
            nameof(DefaultColorItemsSource),
            typeof(IEnumerable<Color>),
            typeof(ColorCanvas),
            new PropertyMetadata(
                new List<Color>(),
                OnDefaultColorItemsSourcePropertyChanged
            )
        );

        public IEnumerable<Color> DefaultColorItemsSource
        {
            get { return (IEnumerable<Color>)GetValue(DefaultColorItemsSourceProperty); }
            set { SetValue(DefaultColorItemsSourceProperty, value); }
        }

        private static void OnDefaultColorItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ColorCanvas)d;
            ctrl.DefaultColorItemsSource = (IEnumerable<Color>)e.NewValue;
        }

        private void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            var oldValueINotifyCollectionChanged = oldValue as INotifyCollectionChanged;

            if(null != oldValueINotifyCollectionChanged) {
                oldValueINotifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(NewValueINotifyCollectionChanged_CollectionChanged);
            }

            var newValueINotifyCollectionChanged = newValue as INotifyCollectionChanged;
            if(null != newValueINotifyCollectionChanged) {
                newValueINotifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(NewValueINotifyCollectionChanged_CollectionChanged);
            }

        }

        void NewValueINotifyCollectionChanged_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            //Do your stuff here.
        }

        #endregion

        #region DefaultColorsColumnsProperty

        public static readonly DependencyProperty DefaultColorsColumnsProperty = DependencyProperty.Register(
            nameof(DefaultColorsColumns),
            typeof(int),
            typeof(ColorCanvas),
            new FrameworkPropertyMetadata(
                1,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnDefaultColorsColumnsPropertyChanged
            )
        );

        public int DefaultColorsColumns
        {
            get { return (int)GetValue(DefaultColorsColumnsProperty); }
            set { SetValue(DefaultColorsColumnsProperty, value); }
        }

        private static void OnDefaultColorsColumnsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ColorCanvas)d;
            ctrl.DefaultColorsColumns = (int)e.NewValue;
        }

        #endregion



        #region function

        ///// <summary>
        ///// https://stackoverflow.com/a/9651053
        ///// </summary>
        ///// <param name="gsc"></param>
        ///// <param name="offset"></param>
        ///// <returns></returns>
        //public static Color GetRelativeColor(GradientStopCollection gsc, double offset)
        //{
        //    var point = gsc.SingleOrDefault(f => f.Offset == offset);
        //    if(point != null) {
        //        return point.Color;
        //    }

        //    GradientStop before = gsc.Where(w => w.Offset == gsc.Min(m => m.Offset)).First();
        //    GradientStop after = gsc.Where(w => w.Offset == gsc.Max(m => m.Offset)).First();

        //    foreach(var gs in gsc) {
        //        if(gs.Offset < offset && gs.Offset > before.Offset) {
        //            before = gs;
        //        }
        //        if(gs.Offset > offset && gs.Offset < after.Offset) {
        //            after = gs;
        //        }
        //    }

        //    var color = new Color();

        //    color.ScA = (float)((offset - before.Offset) * (after.Color.ScA - before.Color.ScA) / (after.Offset - before.Offset) + before.Color.ScA);
        //    color.ScR = (float)((offset - before.Offset) * (after.Color.ScR - before.Color.ScR) / (after.Offset - before.Offset) + before.Color.ScR);
        //    color.ScG = (float)((offset - before.Offset) * (after.Color.ScG - before.Color.ScG) / (after.Offset - before.Offset) + before.Color.ScG);
        //    color.ScB = (float)((offset - before.Offset) * (after.Color.ScB - before.Color.ScB) / (after.Offset - before.Offset) + before.Color.ScB);

        //    return color;
        //}

        void ChangeColor()
        {
            if(HasAlpha) {
                SelectedColor = Color.FromArgb(Alpha, Red, Green, Blue);
            } else {
                SelectedColor = Color.FromRgb(Red, Green, Blue);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectedColor = (Color)((FrameworkElement)sender).Tag;
        }


        #endregion

        //private void PART_HueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        //{
        //    var color = GetRelativeColor(HueGradations, e.NewValue);
        //    BaseColor = color;
        //}
    }
}

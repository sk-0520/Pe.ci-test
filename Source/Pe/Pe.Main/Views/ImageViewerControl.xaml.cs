using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.ViewModels.IconViewer;

namespace ContentTypeTextNet.Pe.Main.Views
{
    /// <summary>
    /// ImageViewerControl.xaml の相互作用ロジック
    /// </summary>
    public partial class ImageViewerControl: UserControl
    {
        public ImageViewerControl()
        {
            InitializeComponent();

            Loaded += ImageViewerControl_Loaded;
        }

        #region IconViewer

        public static readonly DependencyProperty IconViewerProperty = DependencyProperty.Register(
            nameof(IconViewer),
            typeof(IconViewerViewModel),
            typeof(ImageViewerControl),
            new FrameworkPropertyMetadata(
                default(IconViewerViewModel),
                new PropertyChangedCallback(OnIconViewerChanged)
            )
        );

        public IconViewerViewModel IconViewer
        {
            get { return (IconViewerViewModel)GetValue(IconViewerProperty); }
            set { SetValue(IconViewerProperty, value); }
        }

        private static void OnIconViewerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is ImageViewerControl control) {
                if(e.NewValue is IconViewerViewModel iconViewer) {
                    /*
                    var rootVisual = UIUtility.GetClosest<Window>(control);
                    if(rootVisual == null) {
                        var popup = UIUtility.GetClosest<Popup>(control);
                        if(popup != null && popup.PlacementTarget != null) {
                            rootVisual = UIUtility.GetClosest<Window>(popup.PlacementTarget);
                        }
                    }
                    var iconScale = UIUtility.GetDpiScale(rootVisual ?? (Visual)control);
                    control.parent.Width = (int)iconViewer.IconBox;//.ToWidth();
                    control.parent.Height = (int)iconViewer.IconBox;//.ToHeight();
                    if(control.IsLoaded) {
                        iconViewer.LoadAsync(iconScale, CancellationToken.None).ConfigureAwait(false);
                    }
                    */
                    control.ApplyIcon();
                }
            }
        }

        #endregion

        #region IconBox

        public static readonly DependencyProperty IconBoxProperty = DependencyProperty.Register(
            nameof(IconBox),
            typeof(IconBox),
            typeof(ImageViewerControl),
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
            if(d is ImageViewerControl control) {
                control.ApplyIcon();
            }
        }

        #endregion

        #region function

        void ApplyIcon()
        {
            if(IconViewer == null) {
                return;
            }

            var rootVisual = UIUtility.GetClosest<Window>(this);
            if(rootVisual == null) {
                var popup = UIUtility.GetClosest<Popup>(this);
                if(popup != null && popup.PlacementTarget != null) {
                    rootVisual = UIUtility.GetClosest<Window>(popup.PlacementTarget);
                }
            }
            var iconScale = new IconScale(IconBox, UIUtility.GetDpiScale(rootVisual ?? (Visual)this));
            this.parent.Width = (int)IconBox;
            this.parent.Height = (int)IconBox;
            if(IsLoaded) {
                IconViewer.LoadAsync(iconScale, CancellationToken.None).ConfigureAwait(false);
            }
        }

        #endregion

        private void ImageViewerControl_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= ImageViewerControl_Loaded;

            var iconViewer = IconViewer;
            if(iconViewer != null) {
                //var iconScale = UIUtility.GetDpiScale(this);
                iconViewer.LoadAsync(new IconScale(IconBox, UIUtility.GetDpiScale(this)), CancellationToken.None).ConfigureAwait(false);
            }
        }
    }
}

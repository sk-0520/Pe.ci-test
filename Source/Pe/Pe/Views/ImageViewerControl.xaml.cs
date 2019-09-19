using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace ContentTypeTextNet.Pe.Main.Views
{
    /// <summary>
    /// ImageViewerControl.xaml の相互作用ロジック
    /// </summary>
    public partial class ImageViewerControl : UserControl
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
                    control.parent.Width = (int)iconViewer.IconScale;//.ToWidth();
                    control.parent.Height = (int)iconViewer.IconScale;//.ToHeight();
                    if(control.IsLoaded) {
                        iconViewer.LoadAsync(CancellationToken.None).ConfigureAwait(false);
                    }
                }
            }
        }

        #endregion

        private void ImageViewerControl_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= ImageViewerControl_Loaded;

            var iconViewer = IconViewer;
            if(iconViewer != null) {
                iconViewer.LoadAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }
    }
}

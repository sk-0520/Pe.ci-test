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
using ContentTypeTextNet.Pe.Main.ViewModel.CustomizeLauncherItem;

namespace ContentTypeTextNet.Pe.Main.View.CustomizeLauncherItem
{
    /// <summary>
    /// CustomizeLauncherItemControl.xaml の相互作用ロジック
    /// </summary>
    public partial class CustomizeLauncherItemControl : UserControl
    {
        public CustomizeLauncherItemControl()
        {
            InitializeComponent();
        }

        #region Item

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            nameof(Item),
            typeof(CustomizeLauncherItemViewModel),
            typeof(CustomizeLauncherItemControl),
            new FrameworkPropertyMetadata(
                default(CustomizeLauncherItemViewModel),
                new PropertyChangedCallback(OnItemChanged)
            )
        );

        public CustomizeLauncherItemViewModel Item
        {
            get { return (CustomizeLauncherItemViewModel)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        private static void OnItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is CustomizeLauncherItemControl control) {
            }
        }

        #endregion

    }
}

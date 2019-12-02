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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Main.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Views.Setting
{
    /// <summary>
    /// ModifierKeyControl.xaml の相互作用ロジック
    /// </summary>
    public partial class ModifierKeyControl : UserControl
    {
        public ModifierKeyControl()
        {
            InitializeComponent();
        }

        #region ModifierKeyProperty

        public static readonly DependencyProperty ModifierKeyProperty = DependencyProperty.Register(
            nameof(ModifierKey),
            typeof(ModifierKey),
            typeof(ModifierKeyControl),
            new FrameworkPropertyMetadata(
                default(ModifierKey),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnModifierKeyChanged)
            )
        );

        public ModifierKey ModifierKey
        {
            get { return (ModifierKey)GetValue(ModifierKeyProperty); }
            set { SetValue(ModifierKeyProperty, value); }
        }

        static void OnModifierKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as ModifierKeyControl;
            if(ctrl != null) {
                ctrl.ModifierKey = (ModifierKey)e.NewValue;
            }
        }

        #endregion

    }
}

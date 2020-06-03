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
using ContentTypeTextNet.Pe.Main.ViewModels.Setting;

namespace ContentTypeTextNet.Pe.Main.Views.Setting
{
    /// <summary>
    /// PluginsSettingControl.xaml の相互作用ロジック
    /// </summary>
    public partial class PluginsSettingControl: UserControl
    {
        public PluginsSettingControl()
        {
            InitializeComponent();
        }

        #region Editor

        public static readonly DependencyProperty EditorProperty = DependencyProperty.Register(
            nameof(Editor),
            typeof(PluginsSettingEditorViewModel),
            typeof(PluginsSettingControl),
            new FrameworkPropertyMetadata(
                default(PluginsSettingEditorViewModel),
                new PropertyChangedCallback(OnEditorChanged)
            )
        );

        public PluginsSettingEditorViewModel Editor
        {
            get { return (PluginsSettingEditorViewModel)GetValue(EditorProperty); }
            set { SetValue(EditorProperty, value); }
        }

        private static void OnEditorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is PluginsSettingControl control) {
            }
        }

        #endregion

    }
}

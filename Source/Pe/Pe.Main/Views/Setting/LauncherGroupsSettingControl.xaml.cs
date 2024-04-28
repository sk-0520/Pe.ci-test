using System.Windows;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.ViewModels.Setting;

namespace ContentTypeTextNet.Pe.Main.Views.Setting
{
    /// <summary>
    /// LauncherGroupsSettingControl.xaml の相互作用ロジック
    /// </summary>
    public partial class LauncherGroupsSettingControl: UserControl
    {
        public LauncherGroupsSettingControl()
        {
            InitializeComponent();
        }

        #region Editor

        public static readonly DependencyProperty EditorProperty = DependencyProperty.Register(
            nameof(Editor),
            typeof(LauncherGroupsSettingEditorViewModel),
            typeof(LauncherGroupsSettingControl),
            new FrameworkPropertyMetadata(
                default(LauncherGroupsSettingEditorViewModel),
                new PropertyChangedCallback(OnEditorChanged)
            )
        );

        public LauncherGroupsSettingEditorViewModel Editor
        {
            get { return (LauncherGroupsSettingEditorViewModel)GetValue(EditorProperty); }
            set { SetValue(EditorProperty, value); }
        }

        private static void OnEditorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is LauncherGroupsSettingControl control) {
            }
        }

        #endregion

        #region property

        #endregion
    }
}

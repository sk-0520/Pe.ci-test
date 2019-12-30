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
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.ViewModels.Setting;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.Views.Setting
{
    /// <summary>
    /// GeneralsSettingControl.xaml の相互作用ロジック
    /// </summary>
    public partial class GeneralsSettingControl : UserControl
    {
        public GeneralsSettingControl()
        {
            InitializeComponent();
            DialogRequestReceiver = new DialogRequestReceiver(this);
        }

        #region Editor

        public static readonly DependencyProperty EditorProperty = DependencyProperty.Register(
            nameof(Editor),
            typeof(GeneralsSettingEditorViewModel),
            typeof(GeneralsSettingControl),
            new FrameworkPropertyMetadata(
                default(GeneralsSettingEditorViewModel),
                new PropertyChangedCallback(OnEditorChanged)
            )
        );

        public GeneralsSettingEditorViewModel Editor
        {
            get { return (GeneralsSettingEditorViewModel)GetValue(EditorProperty); }
            set { SetValue(EditorProperty, value); }
        }

        private static void OnEditorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is GeneralsSettingControl control) {
            }
        }

        #endregion

        #region property

        DialogRequestReceiver DialogRequestReceiver { get; }
        CommandStore CommandStore { get; } = new CommandStore();

        #endregion

        #region command

        public ICommand FileSelectCommand => CommandStore.GetOrCreate(() => new DelegateCommand<RequestEventArgs>(
            o => {
                DialogRequestReceiver.ReceiveFileSystemSelectDialogRequest(o);
            }
        ));

        #endregion
    }
}

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
    /// PluginsSettingControl.xaml の相互作用ロジック
    /// </summary>
    public partial class PluginsSettingControl: UserControl
    {
        public PluginsSettingControl()
        {
            InitializeComponent();
            DialogRequestReceiver = new DialogRequestReceiver(this);
        }

        #region property

        DialogRequestReceiver DialogRequestReceiver { get; }
        CommandStore CommandStore { get; } = new CommandStore();

        #endregion

        #region command


        public ICommand SelectPluginFileCommand => CommandStore.GetOrCreate(() => new DelegateCommand<RequestEventArgs>(
            o => {
                DialogRequestReceiver.ReceiveFileSystemSelectDialogRequest(o);
            }
        ));

        public ICommand ShowMessageCommand => CommandStore.GetOrCreate(() => new DelegateCommand<RequestEventArgs>(
             o => {
                 var parameter = (CommonMessageDialogRequestParameter)o.Parameter;
                 var result = MessageBox.Show(UIUtility.GetLogicalClosest<Window>(this)!, parameter.Message, parameter.Caption, parameter.Button, parameter.Icon, parameter.DefaultResult, parameter.Options);
             }
         ));

        #endregion

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

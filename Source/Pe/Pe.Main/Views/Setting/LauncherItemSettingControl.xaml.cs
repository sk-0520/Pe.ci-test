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
    /// LauncherItemSettingControl.xaml の相互作用ロジック
    /// </summary>
    public partial class LauncherItemSettingControl : UserControl
    {
        public LauncherItemSettingControl()
        {
            InitializeComponent();
        }

        #region Item

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            nameof(Item),
            typeof(LauncherItemSettingViewModel),
            typeof(LauncherItemSettingControl),
            new FrameworkPropertyMetadata(
                default(LauncherItemSettingViewModel),
                new PropertyChangedCallback(OnItemChanged)
            )
        );

        public LauncherItemSettingViewModel Item
        {
            get { return (LauncherItemSettingViewModel)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        private static void OnItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is LauncherItemSettingControl control) {
            }
        }

        #endregion

        #region property

        CommandStore CommandStore { get; } = new CommandStore();

        #endregion

        #region command

        public ICommand ScrollItemCommand => CommandStore.GetOrCreate(() => new DelegateCommand<RequestEventArgs>(
            o => {
                if(Item.SelectedCustomizeItem != null) {
                    this.listItems.ScrollIntoView(Item.SelectedCustomizeItem);
                }
            }
        ));

        #endregion

    }
}

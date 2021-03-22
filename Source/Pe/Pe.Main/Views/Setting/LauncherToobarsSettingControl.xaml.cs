using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.ViewModels.Setting;
using Microsoft.Extensions.Logging.Abstractions;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.Views.Setting
{
    /// <summary>
    /// LauncherToobarsSettingControl.xaml の相互作用ロジック
    /// </summary>
    public partial class LauncherToobarsSettingControl: UserControl
    {
        public LauncherToobarsSettingControl()
        {
            InitializeComponent();
        }

        #region property

        CommandStore CommandStore { get; } = new CommandStore();

        IList<ScreenWindow> ScreenWindows { get; set; } = new List<ScreenWindow>();
        #endregion

        #region Editor

        public static readonly DependencyProperty EditorProperty = DependencyProperty.Register(
            nameof(Editor),
            typeof(LauncherToobarsSettingEditorViewModel),
            typeof(LauncherToobarsSettingControl),
            new FrameworkPropertyMetadata(
                default(LauncherToobarsSettingEditorViewModel),
                new PropertyChangedCallback(OnEditorChanged)
            )
        );

        public LauncherToobarsSettingEditorViewModel Editor
        {
            get { return (LauncherToobarsSettingEditorViewModel)GetValue(EditorProperty); }
            set { SetValue(EditorProperty, value); }
        }

        private static void OnEditorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is LauncherToobarsSettingControl control) {
            }
        }

        #endregion

        #region function

        void CloseWindows()
        {
            foreach(var window in ScreenWindows) {
                window.MouseUp -= CloseScreenWindow;
                window.KeyUp -= CloseScreenWindow;
                window.Close();
                window.DataContext = null;
            }
            ScreenWindows.Clear();
        }

        #endregion

        #region command

        public ICommand DisplayAllScreensCommand => CommandStore.GetOrCreate(() => new DelegateCommand(
            () => {
                if(ScreenWindows.Any()) {
                    CloseWindows();
                }

                var screenWindows = Screen.AllScreens
                    .OrderByDescending(i => i.Primary)
                    .Select(i => new ScreenViewModel(i, NullLoggerFactory.Instance))
                    .Select(i => new ScreenWindow() { DataContext = i })
                ;
                foreach(var window in screenWindows) {
                    ScreenWindows.Add(window);
                    window.MouseUp += CloseScreenWindow;
                    window.KeyUp += CloseScreenWindow;
                }
                foreach(var window in ScreenWindows) {
                    window.Show();
                    window.UpdateLayout();
                }
                ScreenWindows.First().Activate();
            }
        ));

        #endregion

        void CloseScreenWindow(object sender, EventArgs e)
        {
            CloseWindows();
        }
    }
}

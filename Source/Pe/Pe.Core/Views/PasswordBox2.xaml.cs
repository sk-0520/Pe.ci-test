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

namespace ContentTypeTextNet.Pe.Core.Views
{
    /// <summary>
    /// PasswordBox2.xaml の相互作用ロジック
    /// </summary>
    public partial class PasswordBox2: UserControl
    {
        public PasswordBox2()
        {
            InitializeComponent();
        }

        #region property

        private bool PasswordChanging { get; set; }

        #endregion

        #region Password

        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(
            nameof(Password),
            typeof(string),
            typeof(PasswordBox2),
            new FrameworkPropertyMetadata(
                string.Empty,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                PasswordPropertyChanged,
                null,
                false,
                UpdateSourceTrigger.PropertyChanged
            )
        );

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        private static void PasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is PasswordBox2 passwordBox) {
                passwordBox.UpdatePassword();
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordChanging = true;
            Password = this.passwordBox.Password;
            PasswordChanging = false;
        }

        private void UpdatePassword()
        {
            if(!PasswordChanging) {
                this.passwordBox.Password = Password;
            }
        }

        #endregion
    }
}

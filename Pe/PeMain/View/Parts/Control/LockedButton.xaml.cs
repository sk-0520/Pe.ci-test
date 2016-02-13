/**
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
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
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Control
{
    /// <summary>
    /// LockedButton.xaml の相互作用ロジック
    /// </summary>
    public partial class LockedButton: CommonDataUserControl
    {
        public LockedButton()
        {
            InitializeComponent();
        }

        #region CommandProperty

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(CommandProperty)),
            typeof(ICommand),
            typeof(LockedButton),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnCommandChanged))
        );

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as LockedButton;
            if(control != null) {
                control.Command = e.NewValue as ICommand;
            }
        }

        public ICommand Command
        {
            get { return GetValue(CommandProperty) as ICommand; }
            set { SetValue(CommandProperty, value); }
        }

        #endregion

        #region CommandParameterProperty

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(CommandParameterProperty)),
            typeof(object),
            typeof(LockedButton),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnCommandParameterChanged))
        );

        private static void OnCommandParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as LockedButton;
            control.CommandParameter = e.NewValue;
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        #endregion

    }
}

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
    /// TimeSliderControl.xaml の相互作用ロジック
    /// </summary>
    public partial class TimeSliderControl: CommonDataUserControl
    {
        public TimeSliderControl()
        {
            InitializeComponent();
        }

        #region ValueProperty

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(ValueProperty)),
            typeof(TimeSpan),
            typeof(TimeSliderControl),
            new FrameworkPropertyMetadata(
                TimeSpan.Zero,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnValueChanged)
            )
        );

        public TimeSpan Value
        {
            get { return (TimeSpan)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as TimeSliderControl;
            if(ctrl != null) {
                ctrl.Value = (TimeSpan)e.NewValue;
            }
        }

        #endregion

        #region MaximumProperty

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(MaximumProperty)),
            typeof(TimeSpan),
            typeof(TimeSliderControl),
            new FrameworkPropertyMetadata(
                TimeSpan.Zero,
                new PropertyChangedCallback(OnMaximumChanged)
            )
        );

        public TimeSpan Maximum
        {
            get { return (TimeSpan)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as TimeSliderControl;
            if(ctrl != null) {
                ctrl.Maximum = (TimeSpan)e.NewValue;
            }
        }

        #endregion

        #region MinimumProperty

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(MinimumProperty)),
            typeof(TimeSpan),
            typeof(TimeSliderControl),
            new FrameworkPropertyMetadata(
                TimeSpan.Zero,
                new PropertyChangedCallback(OnMinimumChanged)
            )
        );

        public TimeSpan Minimum
        {
            get { return (TimeSpan)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as TimeSliderControl;
            if(ctrl != null) {
                ctrl.Minimum = (TimeSpan)e.NewValue;
            }
        }

        #endregion
    }
}

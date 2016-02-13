/*
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
    /// DoubleSliderControl.xaml の相互作用ロジック
    /// </summary>
    public partial class DoubleSliderControl: CommonDataUserControl
    {
        public DoubleSliderControl()
        {
            InitializeComponent();
        }

        #region ValueProperty

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(ValueProperty)),
            typeof(double),
            typeof(DoubleSliderControl),
            new FrameworkPropertyMetadata(
                0.0,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnValueChanged)
            )
        );

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as DoubleSliderControl;
            if(ctrl != null) {
                ctrl.Value = (double)e.NewValue;
            }
        }

        #endregion

        #region MaximumProperty

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(MaximumProperty)),
            typeof(double),
            typeof(DoubleSliderControl),
            new FrameworkPropertyMetadata(
                double.MaxValue,
                new PropertyChangedCallback(OnMaximumChanged)
            )
        );

        public double Maximum
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as DoubleSliderControl;
            if(ctrl != null) {
                ctrl.Maximum = (double)e.NewValue;
            }
        }

        #endregion

        #region MinimumProperty

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(MinimumProperty)),
            typeof(double),
            typeof(DoubleSliderControl),
            new FrameworkPropertyMetadata(
                double.MinValue,
                new PropertyChangedCallback(OnMinimumChanged)
            )
        );

        public double Minimum
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as DoubleSliderControl;
            if(ctrl != null) {
                ctrl.Minimum = (double)e.NewValue;
            }
        }

        #endregion
    }
}

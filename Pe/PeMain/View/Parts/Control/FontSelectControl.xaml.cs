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
    /// FontSelectControl.xaml の相互作用ロジック
    /// </summary>
    public partial class FontSelectControl: CommonDataUserControl
    {
        public FontSelectControl()
        {
            InitializeComponent();
        }

        #region FamilyNameProperty

        public static readonly DependencyProperty FamilyNameProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(FamilyNameProperty)),
            typeof(FontFamily),
            typeof(FontSelectControl),
            new FrameworkPropertyMetadata(
                SystemFonts.MessageFontFamily,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnFamilyNameChanged)
            )
        );

        public FontFamily FamilyName
        {
            get { return (FontFamily)GetValue(FamilyNameProperty); }
            set { SetValue(FamilyNameProperty, value); }
        }

        static void OnFamilyNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FontSelectControl;
            if(ctrl != null) {
                ctrl.FamilyName = (FontFamily)e.NewValue;
            }
        }

        #endregion

        #region IsBoldProperty

        public static readonly DependencyProperty IsBoldProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(IsBoldProperty)),
            typeof(bool),
            typeof(FontSelectControl),
            new FrameworkPropertyMetadata(
                default(bool),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnIsBoldChanged)
            )
        );

        public bool IsBold
        {
            get { return (bool)GetValue(IsBoldProperty); }
            set { SetValue(IsBoldProperty, value); }
        }

        static void OnIsBoldChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FontSelectControl;
            if(ctrl != null) {
                ctrl.IsBold = (bool)e.NewValue;
            }
        }

        #endregion

        #region IsItalicProperty

        public static readonly DependencyProperty IsItalicProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(IsItalicProperty)),
            typeof(bool),
            typeof(FontSelectControl),
            new FrameworkPropertyMetadata(
                default(bool),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnIsItalicChanged)
            )
        );

        public bool IsItalic
        {
            get { return (bool)GetValue(IsItalicProperty); }
            set { SetValue(IsItalicProperty, value); }
        }

        static void OnIsItalicChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FontSelectControl;
            if(ctrl != null) {
                ctrl.IsItalic = (bool)e.NewValue;
            }
        }

        #endregion

        #region SizeProperty

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SizeProperty)),
            typeof(double),
            typeof(FontSelectControl),
            new FrameworkPropertyMetadata(
                SystemFonts.MessageFontSize,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnSizeChanged)
            )
        );

        public double Size
        {
            get { return (double)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        static void OnSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FontSelectControl;
            if(ctrl != null) {
                ctrl.Size = (double)e.NewValue;
            }
        }

        #endregion

        #region SizeMinimumProperty

        public static readonly DependencyProperty SizeMinimumProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SizeMinimumProperty)),
            typeof(double),
            typeof(FontSelectControl),
            new FrameworkPropertyMetadata(
                0.0,
                new PropertyChangedCallback(OnSizeMinimumChanged)
            )
        );

        public double SizeMinimum
        {
            get { return (double)GetValue(SizeMinimumProperty); }
            set { SetValue(SizeMinimumProperty, value); }
        }

        static void OnSizeMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FontSelectControl;
            if(ctrl != null) {
                ctrl.SizeMinimum = (double)e.NewValue;
            }
        }

        #endregion

        #region SizeMaximumProperty

        public static readonly DependencyProperty SizeMaximumProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(SizeMaximumProperty)),
            typeof(double),
            typeof(FontSelectControl),
            new FrameworkPropertyMetadata(
                100.0,
                new PropertyChangedCallback(OnSizeMaximumChanged)
            )
        );

        public double SizeMaximum
        {
            get { return (double)GetValue(SizeMaximumProperty); }
            set { SetValue(SizeMaximumProperty, value); }
        }

        static void OnSizeMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FontSelectControl;
            if(ctrl != null) {
                ctrl.SizeMaximum = (double)e.NewValue;
            }
        }

        #endregion

        #region IsEnabledBoldProperty

        public static readonly DependencyProperty IsEnabledBoldProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(IsEnabledBoldProperty)),
            typeof(bool),
            typeof(FontSelectControl),
            new FrameworkPropertyMetadata(
                true,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnIsEnabledBoldChanged)
            )
        );

        public bool IsEnabledBold
        {
            get { return (bool)GetValue(IsEnabledBoldProperty); }
            set { SetValue(IsEnabledBoldProperty, value); }
        }

        static void OnIsEnabledBoldChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FontSelectControl;
            if(ctrl != null) {
                ctrl.IsEnabledBold = (bool)e.NewValue;
            }
        }

        #endregion


        #region IsEnabledItalicProperty

        public static readonly DependencyProperty IsEnabledItalicProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(IsEnabledItalicProperty)),
            typeof(bool),
            typeof(FontSelectControl),
            new FrameworkPropertyMetadata(
                true,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnIsEnabledItalicChanged)
            )
        );

        public bool IsEnabledItalic
        {
            get { return (bool)GetValue(IsEnabledItalicProperty); }
            set { SetValue(IsEnabledItalicProperty, value); }
        }

        static void OnIsEnabledItalicChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FontSelectControl;
            if(ctrl != null) {
                ctrl.IsEnabledItalic = (bool)e.NewValue;
            }
        }

        #endregion

    }
}

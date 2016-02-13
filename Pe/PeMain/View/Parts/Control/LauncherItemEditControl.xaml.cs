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
    /// LauncherItemEditControl.xaml の相互作用ロジック
    /// </summary>
    public partial class LauncherItemEditControl: CommonDataUserControl
    {
        #region event

        public event EventHandler<DependencyPropertyChangedEventArgs> PropertyChanged = delegate { };

        #endregion

        public LauncherItemEditControl()
        {
            InitializeComponent();
        }

        #region IsEditedProperty

        public static readonly DependencyProperty IsEditedProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(IsEditedProperty)),
            typeof(bool),
            typeof(LauncherItemEditControl),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnIsEditedChanged))
        );

        private static void OnIsEditedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var view = obj as LauncherItemEditControl;
            if(view != null) {
                view.IsEdited = (bool)e.NewValue;
            }
        }

        public bool IsEdited
        {
            get { return (bool)GetValue(IsEditedProperty); }
            set { SetValue(IsEditedProperty, value); }
        }

        #endregion

        //#region CommonDataUserControl

        //protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        //{
        //	base.OnPropertyChanged(e);
        //	if(CommonData != null) {
        //		CommonData.Logger.Information(e.Property.Name);
        //		PropertyChanged(this, e);
        //	}
        //}

        //#endregion
    }
}

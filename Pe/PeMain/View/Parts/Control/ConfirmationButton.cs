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
namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Control
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

    public class ConfirmationButton: Button
    {
        public ConfirmationButton()
            : base()
        { }

        #region WaitTimeProperty

        public static readonly DependencyProperty WaitTimeProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName( nameof(WaitTimeProperty)),
            typeof(TimeSpan),
            typeof(ConfirmationButton),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnWaitTimeChanged))
        );

        private static void OnWaitTimeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var view = obj as LauncherItemEditControl;
            if(view != null) {
                view.IsEdited = (bool)e.NewValue;
            }
        }

        public bool IsEdited
        {
            get { return (bool)GetValue(WaitTimeProperty); }
            set { SetValue(WaitTimeProperty, value); }
        }

        #endregion

    }
}

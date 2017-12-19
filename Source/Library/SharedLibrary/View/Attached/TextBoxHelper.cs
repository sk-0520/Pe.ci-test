/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

namespace ContentTypeTextNet.Library.SharedLibrary.View.Attached
{
    /// <summary>
    /// <para>http://stackoverflow.com/questions/2245928/mvvm-and-the-textboxs-selectedtext-property</para>
    /// </summary>
    public static class TextBoxHelper
    {

        public static string GetSelectedText(DependencyObject obj)
        {
            return (string)obj.GetValue(SelectedTextProperty);
        }

        public static void SetSelectedText(DependencyObject obj, string value)
        {
            obj.SetValue(SelectedTextProperty, value);
        }

        // Using a DependencyProperty as the backing store for SelectedText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTextProperty = DependencyProperty.RegisterAttached(
            DependencyPropertyUtility.GetName(nameof(SelectedTextProperty)),
            typeof(string),
            typeof(TextBoxHelper),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectedTextChanged)
        );

        private static void SelectedTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TextBox tb = obj as TextBox;
            if(tb != null) {
                if(e.OldValue == null && e.NewValue != null) {
                    tb.SelectionChanged += tb_SelectionChanged;
                } else if(e.OldValue != null && e.NewValue == null) {
                    tb.SelectionChanged -= tb_SelectionChanged;
                }

                string newValue = e.NewValue as string;

                if(newValue != null && newValue != tb.SelectedText) {
                    tb.SelectedText = newValue as string;
                }
            }
        }

        static void tb_SelectionChanged(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if(tb != null) {
                SetSelectedText(tb, tb.SelectedText);
            }
        }
    }
}

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
using ContentTypeTextNet.Pe.Library.PeData.IF;
using ContentTypeTextNet.Pe.Library.PeData.Item;

namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Attached
{
    public static class Language
    {
        #region WordProperty

        public static readonly DependencyProperty WordProperty = DependencyProperty.RegisterAttached(
            "Word",
            typeof(string),
            typeof(Language),
            new FrameworkPropertyMetadata()
        );

        public static string GetWord(DependencyObject dependencyObject)
        {
            Validate(dependencyObject);

            return (string)dependencyObject.GetValue(WordProperty);
        }
        public static void SetWord(DependencyObject dependencyObject, string value)
        {
            Validate(dependencyObject);

            dependencyObject.SetValue(WordProperty, value);
        }

        #endregion

        #region HintProperty

        public static readonly DependencyProperty HintProperty = DependencyProperty.RegisterAttached(
            "Hint",
            typeof(string),
            typeof(Language),
            new FrameworkPropertyMetadata(null)
        );

        public static string GetHint(DependencyObject dependencyObject)
        {
            Validate(dependencyObject);

            return (string)dependencyObject.GetValue(HintProperty);
        }
        public static void SetHint(DependencyObject dependencyObject, string value)
        {
            Validate(dependencyObject);

            dependencyObject.SetValue(HintProperty, value);
        }

        #endregion

        #region function

        static void Validate(DependencyObject dependencyObject)
        {
            if(dependencyObject == null) {
                throw new ArgumentNullException(nameof(dependencyObject));
            }
        }

        #endregion
    }
}
